using AutoMapper;
using AutoMapper.QueryableExtensions;
using BarServices.DTOs;
using BarServices.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BarServices.Controllers
{
    [Route("api/elaboration")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    public class ElaborationController : CustomBaseController
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ElaborationController(ApplicationDBContext context, IMapper mapper) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ElaborationDTO>>> Get()
        {
            return await Get<Elaboration, ElaborationDTO>();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ElaborationDTOWithProducts>> Get(int id)
        {
            var elaboration = await context.Elaborations
                .Include(e => e.Products)
                .FirstOrDefaultAsync(e => e.Id == id);
            if(elaboration is null) { return NotFound(); }
            return mapper.Map<ElaborationDTOWithProducts>(elaboration);
        }

        [HttpGet("{id:int}/products")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN,COOK")]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts(int id, int lastId = 1, int amount = 5)
        {
            return await context.Products
                .OrderBy(p => p.OrderTime).ThenBy(p => p.Id)
                .Where(p => p.ElaborationId == id && p.Id > lastId && p.Status != "Done")
                .Take(amount)
                .ProjectTo<ProductDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ElaborationCreationDTO elaborationCreationDTO)
        {
            var elaborationDB = await context.Elaborations.AnyAsync(e => e.Name == elaborationCreationDTO.Name);
            if (elaborationDB) return BadRequest($"The elaboration station with name {elaborationCreationDTO.Name} already exists");

            var elaboration = mapper.Map<Elaboration>(elaborationCreationDTO);
            context.Add(elaboration);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ElaborationCreationDTO elaborationCreationDTO)
        {
            var elaboration = mapper.Map<Elaboration>(elaborationCreationDTO);
            elaboration.Id = id;
            context.Update(elaboration);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await context.Elaborations.Where(p => p.Id == id).ExecuteDeleteAsync();
            if (deleted == 0) return NotFound();

            return NoContent();
        }
    }
}

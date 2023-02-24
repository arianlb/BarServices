using AutoMapper;
using BarServices.DTOs;
using BarServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarServices.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ProductController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return await context.Products.ToListAsync();
        }

        [HttpGet("offer")]
        public async Task<ActionResult<List<Product>>> GetOffer()
        {
            return await context.Products.Where(p => p.Offer == true).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProductCreationDTO productCreationDTO)
        {
            var productDB = await context.Products.AnyAsync(p => p.Name == productCreationDTO.Name);
            if (productDB) return BadRequest($"The product with name {productCreationDTO.Name} already exists");

            var product = mapper.Map<Product>(productCreationDTO);
            context.Add(product);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProductUpdateDTO productUpdateDTO)
        {
            var product = mapper.Map<Product>(productUpdateDTO);
            product.Id = id;
            context.Update(product);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}/status/{value=Pending}")]
        public async Task<ActionResult> Put(int id, string value)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) { return NotFound(); }
            product.Status = value;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await context.Products.Where(p => p.Id == id).ExecuteDeleteAsync();
            if (deleted == 0) return NotFound();

            return NoContent();
        }
    }
}

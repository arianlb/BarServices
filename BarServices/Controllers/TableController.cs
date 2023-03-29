using AutoMapper;
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
    [Route("api/table")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN,DEPENDENT")]
    public class TableController : CustomBaseController
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public TableController(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TableDTO>>> Get()
        {
            return await Get<Table, TableDTO>();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TableDTOWithProducts>> Get(int id)
        {
            var table = await context.Tables
                .Include(t => t.Products)
                .Include(t => t.Bar)
                .Include(t => t.Kitchen)
                .FirstOrDefaultAsync(t => t.Id == id);

            if(table is null)
            {
                return NotFound();
            }

            return mapper.Map<TableDTOWithProducts>(table);
        }

        [HttpGet("{id:int}/check")]
        [AllowAnonymous]
        public async Task<ActionResult<decimal>> GetCheck(int id)
        {
            var orders = await context.Tables.Where(t => t.Id == id)
                .Select(t => t.Products.Select(p => p.Price)).FirstOrDefaultAsync();

            if(orders is null)
            {
                return NotFound();
            }

            return orders.Sum();
        }

        [HttpPost]
        public async Task<ActionResult> Post(TableCreationDTO tableCreationDTO)
        {
            bool existDB = await context.Tables.AnyAsync(t => t.Number == tableCreationDTO.Number);
            if (existDB) return BadRequest($"The table with number {tableCreationDTO.Number} already exists");

            existDB = await context.Elaborations.AnyAsync(e => e.Id == tableCreationDTO.BarId);
            if (!existDB) return BadRequest("The Bar ID does not exist in the database");

            existDB = await context.Elaborations.AnyAsync(e => e.Id == tableCreationDTO.KitchenId);
            if (!existDB) return BadRequest("The Kitchen ID does not exist in the database");

            var table = mapper.Map<Table>(tableCreationDTO);
            if(table.Bar is not null)
            {
                context.Entry(table.Bar).State = EntityState.Unchanged;
            }
            if(table.Kitchen is not null)
            {
                context.Entry(table.Kitchen).State = EntityState.Unchanged;
            }
            context.Add(table);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, TableUpdateDTO tableUpdateDTO)
        {
            var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == id);
            if (table is null) { return NotFound(); }
            table = mapper.Map(tableUpdateDTO, table);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}/add/products")]
        public async Task<ActionResult> AddProduts(int id, [FromBody] int[] productIds)
        {
            var table = await context.Tables
                .Include(t => t.Bar)
                .Include(t => t.Kitchen)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (table is null) return NotFound();

            var products = await context.Products
                .Where(p => productIds.Contains(p.Id) && p.Offer == true)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Category,
                    p.Picture
                })
                .ToListAsync();

            if(products.Count != productIds.Length) 
            { 
                return BadRequest("One of the products does not exist in the database or is not on sale"); 
            }

            foreach (var product in products)
            {
                Product newProduct = new()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    Picture = product.Picture,
                    Status = "Pending",
                    Offer = false,
                    OrderTime = DateTime.Now
                };

                table.Products.Add(newProduct);
                if(product.Category == "drink" && table.Bar is not null)
                {
                    table.Bar.Products.Add(newProduct);
                } else if(product.Category == "food" && table.Kitchen is not null)
                {
                    table.Kitchen.Products.Add(newProduct);
                }
            }

            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}/clean/products")]
        public async Task<ActionResult> CleanTable(int id)
        {
            var table = await context.Tables.Include(t => t.Products).FirstOrDefaultAsync(t => t.Id == id);
            if (table is null) { return NotFound(); }
            table.Products.Clear();
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var table = await context.Tables.Include(t => t.Products).FirstOrDefaultAsync(p => p.Id == id);
            if (table is null) { return NotFound(); }

            table.Products.Clear();
            context.Remove(table);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

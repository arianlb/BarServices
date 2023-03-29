using AutoMapper;
using BarServices.DTOs;
using BarServices.Models;
using BarServices.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarServices.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN,COOK")]
    public class ProductController : CustomBaseController
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IStoreFiles storeFiles;
        private readonly string folder = "products";

        public ProductController(ApplicationDBContext context, IMapper mapper, IStoreFiles storeFiles) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.storeFiles = storeFiles;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> Get()
        {
            return await Get<Product, ProductDTO>();
        }

        [HttpGet("offer")]
        [AllowAnonymous]
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

        [HttpPost("{id:int}/picture")]
        public async Task<ActionResult> Post(int id, [FromForm] ProductPictureDTO productPictureDTO)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product is null || productPictureDTO.Picture is null) { return NotFound(); }

            using var memoryStream = new MemoryStream();
            await productPictureDTO.Picture.CopyToAsync(memoryStream);
            var content = memoryStream.ToArray();
            var extension = Path.GetExtension(productPictureDTO.Picture.FileName);
            
            if(product.Picture.Length > 1)
            {
                product.Picture = await storeFiles.EditFileAsync(content, extension, folder, 
                    product.Picture, productPictureDTO.Picture.ContentType);
            }
            else
            {
                product.Picture = await storeFiles.SaveFileAsync(content, extension, folder,
                    productPictureDTO.Picture.ContentType);
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProductUpdateDTO productUpdateDTO)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product is null) { return NotFound(); }
            product = mapper.Map(productUpdateDTO, product);
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
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if( product is null) { return NotFound(); }

            if(product.Picture.Length > 0) { await storeFiles.DeleteFileAsync(product.Picture, folder); }
            context.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

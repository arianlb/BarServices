using AutoMapper;
using BarServices.DTOs;
using BarServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarServices.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public UsersController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await context.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(UserCreationDTO userCreationDTO)
        {
            var userDB = await context.Users.AnyAsync(u => u.UserName == userCreationDTO.UserName);

            if(userDB) return BadRequest($"There is already a user with the username {userCreationDTO.UserName}");

            User user = mapper.Map<User>(userCreationDTO);
            context.Add(user);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, UserUpdateDTO userUpdateDTO)
        {
            var user = mapper.Map<User>(userUpdateDTO);
            user.Id = id;
            context.Update(user);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await context.Users.Where(p => p.Id == id).ExecuteDeleteAsync();
            if (deleted == 0) return NotFound();

            return NoContent();
        }
    }
}

using DatingApp.Data;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
   
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsersById(int id)
        {
            return Ok(await _context.Users.FindAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<List<AppUser>>> AddUser([FromBody] AppUser appUserRequest)
        {
            _context.Add(appUserRequest);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<AppUser>>> UpdateProClimber([FromQuery] AppUser appUserRequest)
        {
            var updateUser = await _context.Users.FindAsync(appUserRequest.Id);
            if (updateUser == null)
            {
                return BadRequest("Id was not found");
            }
            else if (updateUser != null)
            {
               updateUser.UserName = appUserRequest.UserName;
            }

            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpDelete("(id)")]
        public async Task<ActionResult<List<AppUser>>> DeleteProClimber([FromQuery] int id)
        {
            var deleteUser = await _context.Users.FindAsync(id);
            if (deleteUser == null)
            {
                return BadRequest("Id does not exist");
            }
            else if (deleteUser != null)
            {
                _context.Users.Remove(deleteUser);
            }
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
        }


    }
}
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
        public async Task<ActionResult<List<AppUser>>> UpdateUser([FromQuery] AppUser appUserRequest)
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

        [HttpDelete("DeleteUser/(id)")]
        public async Task<ActionResult<List<AppUser>>> DeleteUser([FromQuery] int id)
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

        [HttpDelete("DeleteDuplicateUser")]
        public async Task<ActionResult<List<AppUser>>> DeleteDuplicateUser()
        {
            // Find users with the same username but different IDs
            var duplicateUsernames = await _context.Users
        .GroupBy(x => x.UserName)
        .Where(g => g.Count() > 1)
        .Select(g => g.Key) // Select the usernames
        .ToListAsync();

            if (duplicateUsernames != null && duplicateUsernames.Any())
            {
                foreach (var username in duplicateUsernames)
                {
                    var usersToDelete = await _context.Users
                        .Where(u => u.UserName == username)
                        .OrderByDescending(u => u.Id) // Keep the latest user and delete the rest
                        .Skip(1) // Skip the latest user
                        .ToListAsync();

                    foreach (var user in usersToDelete)
                    {
                        _context.Users.Remove(user);
                    }
                }
            }
            else 
            {
                return NotFound("No duplicate users found");
            }

            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
        }


    }
}
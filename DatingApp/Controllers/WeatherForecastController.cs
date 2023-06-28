using DatingApp.Data;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class ProClimberController : Controller
    {
        private readonly DataContext _context;

        public ProClimberController(DataContext dataContext)
        {
            _context = dataContext;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<AppUser>>> GetProClimber()
        //{
        //    //return Ok(await _context.ProClimbers.ToListAsync());
        //}
    }
}
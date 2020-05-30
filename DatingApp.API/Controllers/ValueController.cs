using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{ 
    [Authorize]
    [ApiController]
   [Route("api/[Controller]")]
    public class ValueController:ControllerBase
    {
    private readonly DataContext _context;
    public ValueController(DataContext context)
    {
        _context=context;
    }
    
        [HttpGet("{id}")]
        public async Task<IActionResult> getValue(int id)
        {
           var redult=await _context.Values.FirstOrDefaultAsync(x=>x.Id==id);
           return Ok(redult);
        }
[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> getValue()
        {
           var redult=await _context.Values.ToListAsync();
           return Ok(redult);
        }

    }
}
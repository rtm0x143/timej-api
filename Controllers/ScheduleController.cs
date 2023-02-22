using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TimejApi.Data;
using TimejApi.Data.Models;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ScheduleController : Controller
    {
        private readonly ScheduleDbContext _context;

        public ScheduleController(ScheduleDbContext context)
        {
            _context = context;
        }
        [HttpPost("addUser")]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            return Ok();
        }
    }
}

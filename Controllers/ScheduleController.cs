using Microsoft.AspNetCore.Mvc;

namespace TimejApi.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

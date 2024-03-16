using Microsoft.AspNetCore.Mvc;

namespace Q2.Controllers.Schedule
{
    public class ScheduleController : Controller
    {
        public IActionResult ByDate()
        {
            return View();
        }
    }
}

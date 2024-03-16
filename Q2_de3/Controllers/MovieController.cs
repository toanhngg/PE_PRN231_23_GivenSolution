using Microsoft.AspNetCore.Mvc;

namespace Q2_de3.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult AddMovie()
        {
            return View();
        }
    }
}

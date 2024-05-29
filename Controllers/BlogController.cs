using Microsoft.AspNetCore.Mvc;

namespace DACS_DAMH.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

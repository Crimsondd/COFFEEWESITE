
using DACS_DAMH.Models;
using DACS_DAMH.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace DACS_DAMH.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ApplicationDbContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _context = context;
        }
        //public IActionResult Phantrang(int page = 1)
        //{
        //    page = page < 1 ? 1 : page;
        //    int pagesize = 4;
        //    var products = _context.Products.ToPagedList(page, pagesize);
        //    return View(products);
        //}
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Statistic", new { area = "Admin" });
            }
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Index", "Home", new { area = "Employer" });
            }

            page = page < 1 ? 1 : page;
            int pagesize = 6;
            var products = _context.Products.ToPagedList(page, pagesize);
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
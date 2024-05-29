using DACS_DAMH.Models;
using DACS_DAMH.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS_DAMH.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public MenuController(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                  .Include(p => p.Category)
                                  .ToListAsync();

            var groupedProducts = products.GroupBy(p => p.Category.Name)
                                          .Select(g => new CategoryProducts
                                          {
                                              CategoryName = g.Key,
                                              Products = g.ToList()
                                          })
                                          .OrderBy(vm => vm.CategoryName)
                                          .ToList();



            return View(groupedProducts);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Categoryproduct(string categoryName)
        {
            //var products = await _context.Products
            //                 .Include(p => p.Category)
            //                 .Where(p => p.Category.Name.Contains(categoryName))
            //                 .ToListAsync();            
            //return View(products);

            var products = await _context.Products
                                  .Include(p => p.Category)
                                  .Where(p => p.Category.Name.Contains(categoryName))
                                  .ToListAsync();

            var groupedProducts = products.GroupBy(p => p.Category.Name)
                                          .Select(g => new CategoryProducts
                                          {
                                              CategoryName = g.Key,
                                              Products = g.ToList()
                                          })
                                          .OrderBy(vm => vm.CategoryName)
                                          .ToList();

            

            return View(groupedProducts);
        }
    }
}

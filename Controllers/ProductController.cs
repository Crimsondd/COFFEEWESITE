using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using DACS_DAMH.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DACS_DAMH.Repository;

namespace DACS_DAMH.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,
        ICategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            var cateProds = categories.Where(cp=>cp.Id == id).FirstOrDefault();
            
            var productNames = products.Where(m=>m.Id== cateProds.Id).ToList();

            return View();
        }
       
        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var size = await _context.Sizes.ToListAsync();
            var topping = await _context.Toppings.ToListAsync();
            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Sizes = size,
                Toppings = topping
            };
            return View(viewModel);
        }
        public async Task<IActionResult> NavMenu(string categoryName)
        {
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

            ViewData["Title"] = "NavMenu";

            return View(groupedProducts);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetUpdatedPrice(int productId, int sizePrice, int toppingId)
        {
            try
            {
                var product = _context.Products.Find(productId);
                var topping = _context.Toppings.Find(toppingId);

                if (product != null && topping != null && sizePrice != -1)
                {
                    var finalPrice = product.Price + sizePrice + topping.PriceTp;
                    return Ok(finalPrice);
                }
                else if (product != null && sizePrice != -1 && topping == null)
                {
                    var finalPrice = product.Price + sizePrice;
                    return Ok(finalPrice);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật giá.");
            }
        }


    }
}

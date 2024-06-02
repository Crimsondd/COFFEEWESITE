using DACS_DAMH.Models;
using DACS_DAMH.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DACS_DAMH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderRepository orderRepository, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<IActionResult> Order(string postTitle)
        {
            IEnumerable<Order> orders;
            var sizes = await _context.Sizes.ToListAsync();

            var toppings = await _context.Toppings.ToListAsync();
            if (postTitle != null)
            {
                orders = await _orderRepository.SearchAsync(postTitle);
            }
            else
            {
                orders = await _orderRepository.GetAllOrdersAsync();
            }

            return View(orders);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var orderToDelete = await _orderRepository.GetOrderByIdAsync(id);
            if (orderToDelete == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderAsync(orderToDelete);

            return RedirectToAction(nameof(Order)); // Chuyển hướng về trang danh sách đơn hàng sau khi xóa
        }

        

    }
}

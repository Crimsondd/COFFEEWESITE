using DACS_DAMH.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS_DAMH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Order()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
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
        public async Task<IActionResult> Search(int term)
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderIds = orders.Select(x => x.Id).ToList();
            var filteredOrder = orderIds.Where(p => p == term);
            return Json(filteredOrder);
        }


    }
}

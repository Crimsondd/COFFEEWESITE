using DACS_DAMH.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS_DAMH.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.ApplicationUser) // Đảm bảo rằng thông tin về người dùng cũng được load
                .Include(o => o.OrderDetails) // Load thông tin chi tiết của đơn hàng
                .ToListAsync();
        }

        //Phương thức xóa Order
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> SearchAsync(int searchTerm)
        {
            var orders = await _context.Orders
                .Where(p => p.Id == searchTerm)
                .ToListAsync();
            return orders;
        }
    }
}

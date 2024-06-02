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
                           .Include(o => o.OrderDetails)
                           .ThenInclude(od => od.Product)
                           .OrderByDescending(o => o.OrderDate)
                           .ToListAsync();

        }

        public async Task<IEnumerable<Order>> GetOrderByIdUserAsync(string id)
        {

            return await _context.Orders
                            .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                            .Where(o => o.UserId == id)
                            .OrderByDescending(o => o.OrderDate)
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

        public async Task<IEnumerable<Order>> SearchAsync(string searchTerm)
        {

            //    var orders = await _context.Orders
            //        .Include(o => o.OrderDetails)
            //        .ThenInclude(od => od.Product)
            //        .Where(p => p.Id == int.Parse(searchTerm) || p.OrderDate.ToShortDateString().Contains(searchTerm))
            //        .OrderByDescending(o => o.OrderDate)
            //        .ToListAsync();

            //    return orders;
            // Attempt to parse the searchTerm as an integer
            int orderId;
            DateTime searchDate;
            var ordersQuery = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .AsQueryable();

            // Check if searchTerm is an integer
            if (int.TryParse(searchTerm, out orderId))
            {
                ordersQuery = ordersQuery.Where(p => p.Id == orderId);
            }
            // Check if searchTerm is a valid date
            else if (DateTime.TryParse(searchTerm, out searchDate))
            {
                // Compare dates
                ordersQuery = ordersQuery.Where(p => p.OrderDate.Date == searchDate.Date);
            }
            else
            {
                // Handle invalid searchTerm format, return empty or log the issue as needed
                return Enumerable.Empty<Order>();
            }

            // Execute the query and return the result
            var orders = await ordersQuery
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders;
        }
    }
}

using DACS_DAMH.Models;

namespace DACS_DAMH.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<IEnumerable<Order>> GetOrderByIdUserAsync(string id);
        Task<Order> GetOrderByIdAsync(int id);
        Task DeleteOrderAsync(Order order);

        Task<IEnumerable<Order>> SearchAsync(string searchTerm);
    }
}

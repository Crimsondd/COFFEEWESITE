using DACS_DAMH.Models;

namespace DACS_DAMH.Repository
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Discount>> GetAllAsync();
        Task<Discount> GetByIdAsync(int id);
        Task AddAsync(Discount product);
        Task UpdateAsync(Discount product);
        Task DeleteAsync(Discount discount);
    }
}

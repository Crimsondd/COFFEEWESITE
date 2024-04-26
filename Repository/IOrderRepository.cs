﻿using DACS_DAMH.Models;

namespace DACS_DAMH.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<Order> GetOrderByIdAsync(int id);
        Task DeleteOrderAsync(Order order);

        Task<IEnumerable<Order>> SearchAsync(int searchTerm);
    }
}
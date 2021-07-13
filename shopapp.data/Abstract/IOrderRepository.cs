using shopapp.entity;
using System.Collections.Generic;

namespace shopapp.data.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductDetails(int id);
        List<Product> GetPopularProducts();
        List<Product> GetTop5Products();
    }
}
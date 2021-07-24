using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shopapp.business.Abstract
{
    public interface IProductService: IValidator<Product>
    {
        Task<Product> GetById(int id);
        Product GetByIdWithCategories(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        Task<List<Product>> GetAll();

        bool Create(Product entity);
        Task<Product> CreateAsync(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        bool Update(Product entity, int[] categoryIds);
    }
}

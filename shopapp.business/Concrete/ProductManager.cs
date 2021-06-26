using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace shopapp.business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public void Create(Product entity)
        {
            _productRepository.Create(entity);
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using shopapp.webui.Models;

namespace shopapp.webui.Data
{
    public static class ProductRepository
    {
        private static List<Product> _products = null;

        static ProductRepository()
        {
            _products = new List<Product>
            {
                new Product{ProductId=1,Name="Iphone 8",Price=3000,Description="128GB",IsApproved=true,ImageUrl="1.jpg"},
                new Product{ProductId=2,Name = "Iphone 7", Price = 2500, Description = "64GB",IsApproved=false,ImageUrl="2.jpg" },
                new Product{ProductId=3,Name="Iphone 6",Price=2000,Description="32GB",IsApproved=true,ImageUrl="3.jpg"},
                new Product{ProductId=4,Name = "Iphone 5", Price = 1500, Description = "16GB",ImageUrl="4.jpg" }
            };
        }

        public static List<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public static void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public static Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}
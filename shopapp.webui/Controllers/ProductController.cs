using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Data;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var product = new Product { Name = "Iphone 10", Price = 15000, Description = "256 GB" };
            // ViewData["Category"]="Telefonlar";
            // ViewData["Product"]=product;

            ViewBag.Category = "Telefonlar";
            // ViewBag.Product=product;

            return View(product);
        }

        public string About()
        {
            return "product/about";
        }
        public IActionResult List()
        {
            var products = new List<Product>()
            {
                new Product{Name="Iphone 8",Price=3000,Description="128GB",IsApproved=true},
                new Product { Name = "Iphone 7", Price = 2500, Description = "64GB",IsApproved=false },
                new Product{Name="Iphone 6",Price=2000,Description="32GB",IsApproved=true},
                new Product { Name = "Iphone 5", Price = 1500, Description = "16GB" }

            };


            var productViewModels = new ProductViewModels()
            {
                Products = ProductRepository.Products
            };

            return View(productViewModels);
        }

        public IActionResult Details(int id)
        {
            return View(ProductRepository.GetProductById(id));
        }
    }
}
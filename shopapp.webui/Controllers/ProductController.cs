using System.Collections.Generic;
using System.Linq;
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
        public IActionResult List(int? id)
        {
            var products = ProductRepository.Products;
            if (id != null)
            {
                products = products.Where(products => products.CategoryId == id).ToList();
            }

            var productViewModels = new ProductViewModels()
            {
                Products = products
            };

            return View(productViewModels);
        }

        public IActionResult Details(int id)
        {
            return View(ProductRepository.GetProductById(id));
        }
    }
}
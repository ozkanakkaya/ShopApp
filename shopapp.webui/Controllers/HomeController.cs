using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // int time = DateTime.Now.Hour;

            // ViewBag.Greeting = time>12?"İyi günler":"Günaydın";
            // ViewBag.UserName="Özkan";

            // return View();

            var products = new List<Product>()
            {
                new Product{Name="Iphone 8",Price=3000,Description="128GB",IsApproved=true},
                new Product { Name = "Iphone 7", Price = 2500, Description = "64GB",IsApproved=false },
                new Product{Name="Iphone 6",Price=2000,Description="32GB",IsApproved=true},
                new Product { Name = "Iphone 5", Price = 1500, Description = "16GB" }

            };

            var productViewModels = new ProductViewModels()
            {
                Products = products
            };

            return View(productViewModels);
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View("MyView");
        }
    }
}
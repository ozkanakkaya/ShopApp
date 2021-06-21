using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.data.Abstract;
using shopapp.webui.Data;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            IProductRepository _productRepository;

            var productViewModels = new ProductViewModels()
            {
                Products = ProductRepository.Products
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
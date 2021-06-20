using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult List(int? id, string q)
        {
            //QueryString
            // Console.WriteLine(q);//form ekranında arama kutusuna yazılan bilgi q da tutulur. bu q, _navbar.cshtml de tanımlanmıştır.
            // Console.WriteLine(HttpContext.Request.Query["q"].ToString());//diğeriyle aynıdır.

            var products = ProductRepository.Products;
            if (id != null)
            {//CategoryId sine göre sıralarken gelen id ye ait ürünleri listeler
                products = products.Where(products => products.CategoryId == id).ToList();
            }

            if (!string.IsNullOrEmpty(q))
            {//Formdaki arama kısmına yazılan veriyi içerenleri listeler
                products = products.Where(i => i.Name.ToLower().Contains(q.ToLower()) || i.Description.ToLower().Contains(q.ToLower())).ToList();
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

        public IActionResult Create()
        {
            //Bu viewbag ile Create.cshtml view ına bir select list gönderdik. 
            //Bu SelectList viewda category seçiminde Value alanına CategoryId, Text alanına ise Name bilgisini verecek.
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.AddProduct(p);
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");

            return View(p);

        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.EditProduct(p);
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");

            return View(p);
        }
    }
}
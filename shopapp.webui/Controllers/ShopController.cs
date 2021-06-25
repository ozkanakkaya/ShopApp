using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult List(string category)
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetProductsByCategory(category)
            };

            return View(productViewModel);
        }

        public IActionResult Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails((int)id);//Product, kategori bilgileriyle geliyor.

            if (product==null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel { 
                Product=product,
                Categories=product.ProductCategories.Select(i=>i.Category).ToList()//Burada, Select ile ProductCategories daki her bir eleman i dir. Her i nin Category sini Listeye atacak. Bunu foreach gibi düşünebiliriz.
            });
        }
    }
}

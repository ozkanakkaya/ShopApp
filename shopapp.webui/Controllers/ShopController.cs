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

        public IActionResult List(string category,int page=1)
        {
            const int pageSize = 3;//const ile bu değişken bilgisi aşağıda değiştirilemez.
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetProductsByCategory(category,page,pageSize)
            };

            return View(productViewModel);
        }

        public IActionResult Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);//Product, kategori bilgileriyle geliyor.

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

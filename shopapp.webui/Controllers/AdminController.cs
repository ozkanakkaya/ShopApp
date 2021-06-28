using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;

        public AdminController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()

            });
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product()
            {
                Name=model.Name,
                Url=model.Url,
                Price=model.Price,
                Description=model.Description,
                ImageUrl=model.ImageUrl
            };

            _productService.Create(entity);

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün eklendi!",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
            //{"Message":"samsung isimli ürün eklendi!","AlertType":"success"} jsonconvert ile bu şekile çevrilir(Layout ta bu bilgi alınacak)

            return RedirectToAction("ProductList");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetById((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {
            var entity = _productService.GetById(model.ProductId);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Url = model.Url;
            entity.ImageUrl = model.ImageUrl;
            entity.Description = model.Description;

            _productService.Update(entity);

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün güncellendi!",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            if (entity != null)
            {
                _productService.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün silindi!",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }
    }
}

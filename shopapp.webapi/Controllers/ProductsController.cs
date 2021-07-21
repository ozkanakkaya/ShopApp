using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webapi.Controllers
{
    // localhost:4200/api/products
    // localhost:4200/api/products/2
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var p = _productService.GetById(id);
            if (p == null)
            {
                return NotFound(); // 404
            }
            return Ok(p); // 200
        }
    }
}


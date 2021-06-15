using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.Controllers
{
    public class ProductController:Controller
    {
        public string Index()
        {
            return "product/index";
        }

        public string About()
        {
            return "product/about";
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
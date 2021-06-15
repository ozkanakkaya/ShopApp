using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return  View();
        }
        public IActionResult Contact()
        {
            return  View("MyView");
        }
    }
}
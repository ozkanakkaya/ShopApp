using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.WiewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //if (RouteData.Values["action"].ToString()=="List")//List sayfasından idyi almak için action List olmalı
            //    ViewBag.SelectedCategory=RouteData?.Values["id"];//seçilen menüyü aktif etmek için id yi ViewBag ile tutuyoruz
            //return View(CategoryRepository.Categories);
            return View();
        }
    }
}
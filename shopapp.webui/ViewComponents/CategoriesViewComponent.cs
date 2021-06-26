using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;

namespace shopapp.webui.WiewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            if (RouteData.Values["category"] != null)//List sayfasından idyi almak için category bilgisi olmalı
                ViewBag.SelectedCategory = RouteData?.Values["category"];//seçilen menüyü aktif etmek için urldeki category bilgisini ViewBag ile tutuyoruz
            return View(_categoryService.GetAll());

        }
    }
}
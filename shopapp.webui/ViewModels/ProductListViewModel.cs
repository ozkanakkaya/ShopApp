using shopapp.entity;
using System;
using System.Collections.Generic;

namespace shopapp.webui.WiewModels
{
    public class PageInfo
    {
        public int TotalItems { get; set; }//toplam ürün sayýsý
        public int ItemsPerPage { get; set; }//sayfa baþýna kaç tane ürün
        public int CurrentPage { get; set; }//o anki gösterilen sayfa
        public string CurrentCategory { get; set; }//kategori varsa burada olacak

        int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);//Math.Ceiling:yuvarlama iþlemi yapar
        }
    }
    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }


    }
}
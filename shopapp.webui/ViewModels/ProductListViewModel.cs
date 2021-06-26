using shopapp.entity;
using System;
using System.Collections.Generic;

namespace shopapp.webui.WiewModels
{
    public class PageInfo
    {
        public int TotalItems { get; set; }//toplam �r�n say�s�
        public int ItemsPerPage { get; set; }//sayfa ba��na ka� tane �r�n
        public int CurrentPage { get; set; }//o anki g�sterilen sayfa
        public string CurrentCategory { get; set; }//kategori varsa burada olacak

        int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);//Math.Ceiling:yuvarlama i�lemi yapar
        }
    }
    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }


    }
}
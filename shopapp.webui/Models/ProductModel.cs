using shopapp.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Url", Prompt = "Ürün url'sini giriniz")]
        public string Url { get; set; }

        [Display(Name="Adı",Prompt ="Ürün adını giriniz")]
        public string Name { get; set; }

        [Display(Name = "Fiyat", Prompt = "Ürün fiyatını giriniz")]
        public double? Price { get; set; }

        [Display(Name = "Açıklama", Prompt = "Ürün açıklamasını giriniz")]
        public string Description { get; set; }

        [Display(Name = "Fotoğraf", Prompt = "Ürün fotoğrafını seçiniz")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}

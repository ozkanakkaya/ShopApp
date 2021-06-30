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

        [Display(Name = "Name", Prompt = "Ürün ismi giriniz")]
        [Required(ErrorMessage = "Name zorunlu bir alan.")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "Ürün ismi 5-60 karakter aralığında olmalıdır.")]
        public string Name { get; set; }

        [StringLength(60, MinimumLength = 5, ErrorMessage = "Ürün url'si 5-60 karakter aralığında olmalıdır.")]
        [Required(ErrorMessage = "Url zorunlu bir alan.")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Price zorunlu bir alan.")]
        [Range(1, 10000, ErrorMessage = "Price için 1-10000 arasında değer girmelisiniz.")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Description zorunlu bir alan.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Description 5-100 karakter aralığında olmalıdır.")]

        public string Description { get; set; }

        [Required(ErrorMessage = "ImageUrl zorunlu bir alan.")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}

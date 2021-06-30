using shopapp.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Display(Name = "Name", Prompt = "Kategori ismini giriniz")]
        [Required(ErrorMessage = "Name zorunlu bir alan.")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "Kategori ismi 5-60 karakter aralığında olmalıdır.")]
        public string Name { get; set; }

        [StringLength(60, MinimumLength = 5, ErrorMessage = "Kategori url'si 5-60 karakter aralığında olmalıdır.")]
        [Required(ErrorMessage = "Url zorunlu bir alan.")]
        public string Url { get; set; }

        public List<Product> Products { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace shopapp.webui.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]//zorunluluk
        [StringLength(60,MinimumLength =10,ErrorMessage ="Ürün adı 10-60 karakter arası olmalıdır!")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Fiyat girmelisiniz!")]
        [Range(1,10000)]//aralık
        public double? Price { get; set; }
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }

        [Required]
        public int? CategoryId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAspShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [Range(1.0, 5000000.0, ErrorMessage = "Range 1 - 5000000")]
        public double Price { get; set; }
        [Required]
        public bool Available { get; set; }
        [MinLength(2)]
        [MaxLength(500)]
        public string Image { get; set; }
        [MinLength(2)]
        [MaxLength(25)]
        public string ShadeColor { get; set; }

        // Навигационное свойство связанной таблицы
        public int ProductTypeId { get; set; }
        [ForeignKey(nameof(ProductTypeId))]
        [Display(Name = "Product types")]
        public virtual ProductType ProductType { get; set; }

        public int SpecialTagsId { get; set; }
        [ForeignKey(nameof(SpecialTagsId))]
        [Display(Name = "Special tags")]
        public virtual SpecialTag SpecialTag { get; set; }
    }
}
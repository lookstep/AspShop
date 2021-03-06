using System.ComponentModel.DataAnnotations;

namespace CoreAspShop.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Lenght doesn't be less 3 characters")]
        [MaxLength(30, ErrorMessage = "Lenght doesn't be bigger 30 characters")]
        public string Name { get; set; }
    }
}

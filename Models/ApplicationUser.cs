using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Sales person")]
        [MaxLength(30)]
        public string  Name { get; set; }
        [NotMapped]
        public bool IsSuperAdmin { get; set; }

    }
}

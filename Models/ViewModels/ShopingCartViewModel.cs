using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Models.ViewModels
{
    public class ShopingCartViewModel
    {
        public List<Product> Products { get; set; }
        public Appointment Appointment { get; set; }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

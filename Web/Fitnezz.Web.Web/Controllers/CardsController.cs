using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class CardsController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}

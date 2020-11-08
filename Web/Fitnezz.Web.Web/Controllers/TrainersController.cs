using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class TrainersController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}

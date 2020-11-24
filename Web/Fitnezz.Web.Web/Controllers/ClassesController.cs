using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class ClassesController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Create()
        {
            var viewModel = new ClassCreateInputModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(ClassCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            return this.RedirectToAction("All");
        }
    }
}

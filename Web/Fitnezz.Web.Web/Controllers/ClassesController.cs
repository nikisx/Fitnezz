using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IClassesService classesService;

        public ClassesController(IWebHostEnvironment environment,IClassesService classesService)
        {
            this.environment = environment;
            this.classesService = classesService;
        }

        public IActionResult All()
        {
            var viewModel = this.classesService.GetAll();
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new ClassCreateInputModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClassCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var physicalPath = $"{this.environment.WebRootPath}/classesImgs/";
            Directory.CreateDirectory(physicalPath);

            await this.classesService.Create(input, physicalPath);

            return this.RedirectToAction("All");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IClassesService classesService;
        private readonly IUsersService usersService;

        public ClassesController(IWebHostEnvironment environment,IClassesService classesService,IUsersService usersService)
        {
            this.environment = environment;
            this.classesService = classesService;
            this.usersService = usersService;
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

        [Authorize]
        public async Task<IActionResult> Join(int id)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                var trainer = this.usersService.GetTrainer(this.User.Identity.Name);

                await this.classesService.AddTrainerToClass(trainer.Id, id);
            }

            return this.RedirectToAction("All");
        }
    }
}

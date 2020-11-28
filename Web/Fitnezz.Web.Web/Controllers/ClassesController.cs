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

                if (this.classesService.GetTrainersCount(id) >= 3)
                {
                    this.TempData["sErrMsg"] = "Max 3 trainers allowed to a class";
                    return this.View("All", this.classesService.GetAll());
                }

                if (this.classesService.IsTrainerJoinedAlready(trainer.Id, id))
                {
                    this.TempData["sErrMsg"] = "You can`t join the same class";
                    return this.View("All", this.classesService.GetAll());
                }

                await this.classesService.AddTrainerToClass(trainer.Id, id);
            }
            else
            {
                var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

                if (user.CardId == null)
                {
                    this.TempData["sErrMsg"] = "Only members can join classes";
                    return this.View("All", this.classesService.GetAll());
                }

                if (this.classesService.IsUserJoined(user.CardId,id))
                {
                    this.TempData["sErrMsg"] = "Already joined";
                    return this.View("All", this.classesService.GetAll());
                }

                if (this.classesService.GetUserClassesCount(user.CardId) >= 3)
                {
                    this.TempData["sErrMsg"] = "You can join max 3 class";
                    return this.View("All", this.classesService.GetAll());
                }

                await this.classesService.AddUserToClass(user.CardId, id);
            }

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Leave(int id)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                
            }
            else
            {
                var user = this.usersService.GetUserByUserName(this.User.Identity.Name);
                await this.classesService.LeaveClass(user.CardId, id);
            }

            return this.Redirect("/Users/Profile#test3");
        }

        public PartialViewResult ShowError(string sErrorMessage)
        {
            return this.PartialView("_ErrorPopup");
        }
    }
}

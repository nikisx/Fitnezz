using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainersService trainersService;
        private readonly IUsersService usersService;

        public TrainersController(ITrainersService trainersService, IUsersService usersService)
        {
            this.trainersService = trainersService;
            this.usersService = usersService;
        }

        public IActionResult All()
        {
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);
            var viewModel = this.trainersService.GetAll();
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Hire(string id)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

                if (user.CardId == null)
                {
                    this.TempData["sErrMsg"] = "You must be a member to hire a trainer";
                    return this.View("All", this.trainersService.GetAll());
                }

                if (user.TrainerId == id)
                {
                    this.TempData["sErrMsg"] = "Trainer already hired";
                    return this.View("All", this.trainersService.GetAll());
                }

                if (user.TrainerId != null)
                {
                    this.TempData["sErrMsg"] = "You can't have more than 1 trainer";
                    return this.View("All", this.trainersService.GetAll());
                }

                await this.trainersService.GetHired(id, user.Id);
            }

            return RedirectToAction("All");
        }

        public IActionResult Create()
        {
            var viewModel = new TrainerCreateInputModel();
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainerCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.trainersService.Create(input);

            return RedirectToAction("All");
        }

        public IActionResult Clients()
        {
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);
            var viewModel = this.trainersService.GetClients(trainer.Id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> DeleteWorkout(int workoutId, string userId)
        {
            await this.trainersService.DeleteUsersWorkout(userId, workoutId);
            return this.Redirect($"/Users/Workouts/{userId}");
        }

        public async Task<IActionResult> DeleteMealPlan(int mealPlanId, string userId)
        {
            await this.trainersService.DelteUserMealPlan(userId, mealPlanId);
            return this.Redirect($"/Users/MealPlans/{userId}");
        }

        public PartialViewResult ShowError(string sErrorMessage)
        {
            return this.PartialView("_ErrorPopup");
        }

        [Authorize]
        public async Task<IActionResult> Fire()
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                return this.NotFound();
            }

            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            if (user.TrainerId == null)
            {
                return this.NotFound();
            }

            await this.trainersService.DeleteTrainerForUser(user.Id);

            return this.Redirect("/Users/Profile#test2");
        }
    }
}

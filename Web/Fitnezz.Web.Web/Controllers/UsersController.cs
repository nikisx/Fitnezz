using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IWorkoutsService workoutsService;
        private readonly IMealPlansService mealPlansService;

        public UsersController(IUsersService usersService,IWorkoutsService workoutsService,IMealPlansService mealPlansService)
        {
            this.usersService = usersService;
            this.workoutsService = workoutsService;
            this.mealPlansService = mealPlansService;
        }

        public IActionResult Workouts(string id)
        {
            var userId = string.Empty;

            userId = id ?? this.usersService.GetUserByUserName(this.User.Identity.Name).Id;

            var viewModel = this.usersService.GetAllUsersWorkout(userId);
            return this.View(viewModel);
        }

        public IActionResult Workout(int id, string userId)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                var trainer = this.usersService.GetTrainer(this.User.Identity.Name);
                var user = this.usersService.GetUserById(userId);

                if (user == null || user.TrainerId != trainer.Id)
                {
                    return this.NotFound();
                }
            }

            var viewModel = this.workoutsService.GetWorkoutDetails(id);
            this.ViewBag.Workout = viewModel.Name;
            return this.View(viewModel);
        }

        public IActionResult MealPlans(string id)
        {
            var userId = string.Empty;

            userId = id ?? this.usersService.GetUserByUserName(this.User.Identity.Name).Id;

            var viewModel = this.usersService.GetUserMealPlans(userId);
            return this.View(viewModel);
        }

        public IActionResult MealPlan(int id, string userId)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                var trainer = this.usersService.GetTrainer(this.User.Identity.Name);
                var user = this.usersService.GetUserById(userId);

                if (user == null || user.TrainerId != trainer.Id)
                {
                    return this.NotFound();
                }
            }

            var viewModel = this.mealPlansService.GetDetails(id);
            this.ViewBag.Id = id;
            return this.View(viewModel);
        }

        public IActionResult Profile()
        {
            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            var inputModel = new ProfileUpdateInputModel()
            {
                UserName = user.UserName,
                Age = user.Age,
                Goal = user.Goal,
                Height = user.Height,
                Weight = user.Weight,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileUpdateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.usersService.GetUserByUserName(this.User.Identity.Name).Id;

            await this.usersService.UpdateProfile(input, userId);

            return this.Redirect("/Users/Profile#test1");
        }
    }
}

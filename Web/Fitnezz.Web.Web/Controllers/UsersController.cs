using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IWorkoutsService workoutsService;
        private readonly IMealPlansService mealPlansService;
        private readonly ICardsService cardsService;

        public UsersController(IUsersService usersService,IWorkoutsService workoutsService,IMealPlansService mealPlansService,ICardsService cardsService)
        {
            this.usersService = usersService;
            this.workoutsService = workoutsService;
            this.mealPlansService = mealPlansService;
            this.cardsService = cardsService;
        }

        [Authorize]
        public IActionResult Workouts(string id)
        {
            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && user.CardId == null)
            {
                return this.Redirect("/Cards/Create");
            }

            this.ViewBag.UserTrainer = user.TrainerId == null;

            var userId = string.Empty;

            userId = id ?? user.Id;

            var viewModel = this.usersService.GetAllUsersWorkout(userId);
            return this.View(viewModel);
        }

        [Authorize]
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

        [Authorize]
        public IActionResult MealPlans(string id)
        {
            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && user.CardId == null)
            {
                return this.Redirect("/Cards/Create");
            }

            this.ViewBag.UserTrainer = user.TrainerId == null;

            var userId = string.Empty;

            userId = id ?? user.Id;

            var viewModel = this.usersService.GetUserMealPlans(userId);
            return this.View(viewModel);
        }

        [Authorize]
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

        [Authorize]
        public IActionResult Profile()
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                return this.NotFound();
            }

            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            var model = new ComplexModel
            {
                InputModel = new ProfileUpdateInputModel()
                {
                    UserName = user.UserName,
                    Age = user.Age,
                    Goal = user.Goal,
                    Height = user.Height,
                    Weight = user.Weight,
                },

                ViewModel = user.CardId == null ? null : this.cardsService.GetCard(user.Id),

                ClassesViewModel = user.CardId == null ? null : this.cardsService.GetUserClasses(user.CardId),

                Trainer = user.TrainerId == null ? null : this.usersService.GetUserTrainer(user.TrainerId),
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ComplexModel input)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.usersService.GetUserByUserName(this.User.Identity.Name).Id;

            await this.usersService.UpdateProfile(input.InputModel, userId);

            return this.Redirect("/Users/Profile#test1");
        }
    }
}

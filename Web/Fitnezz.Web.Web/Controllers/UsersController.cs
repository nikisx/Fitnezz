﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
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
    }
}

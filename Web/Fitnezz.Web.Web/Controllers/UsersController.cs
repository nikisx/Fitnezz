﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IWorkoutsService workoutsService;

        public UsersController(IUsersService usersService,IWorkoutsService workoutsService)
        {
            this.usersService = usersService;
            this.workoutsService = workoutsService;
        }

        public IActionResult Workouts()
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name);
            var viewModel = this.usersService.GetAllUsersWorkout(userId.Id);
            return this.View(viewModel);
        }

        public IActionResult Workout(int id)
        {
            var viewModel = this.workoutsService.GetWorkoutDetails(id);
            this.ViewBag.Workout = viewModel.Name;
            return this.View(viewModel);
        }

        public IActionResult MealPlans()
        {
            return this.View();
        }

        public IActionResult MealPlan(int id)
        {
            this.ViewBag.Id = id;
            return this.View();
        }
    }
}

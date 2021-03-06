﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class MealPlansController : Controller
    {
        private readonly IMealPlansService mealPlansService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUsersService usersService;

        public MealPlansController(IMealPlansService mealPlansService, IUsersService usersService, SignInManager<ApplicationUser> signInManager)
        {
            this.mealPlansService = mealPlansService;
            this.signInManager = signInManager;
            this.usersService = usersService;
        }

        public IActionResult All(int pageNumber = 1)
        {
            PaginatedList<AllMealPLansViewModel> model = null;

            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                model = this.mealPlansService.GetAll(pageNumber);
            }
            else if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                model = this.mealPlansService.GetAllWithDeletedMealPlans(pageNumber);
            }
            else
            {
                model = this.mealPlansService.GetAllPublic(pageNumber);
            }

            var viewModel = new ComplexViewModelForMealPlans()
            {
              InputModel = new AddMealPlanInputModel(),

              ViewModel = model,
            };

            return this.View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.mealPlansService.GetDetails(id);

            if (!viewModel.IsPublic)
            {
                if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    if (!this.signInManager.IsSignedIn(this.User))
                    {
                        return this.NotFound();
                    }
                    var userId = this.usersService.GetUserByUserName(this.User.Identity.Name).Id;
                    if (!this.mealPlansService.UserHasMealPlan(userId, id))
                    {
                        return this.RedirectToAction("All");
                    }
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Authorize(Roles = GlobalConstants.TrainerRoleName)]
        public async Task<IActionResult> Create(AddMealPlanInputModel input)
        {
            var viewModel = new ComplexViewModelForMealPlans()
            {
                InputModel = input,
                ViewModel = this.mealPlansService.GetAll(1),
            };

            if (!this.ModelState.IsValid)
            {
                this.TempData["sErrMsg"] = this.ModelState.Values.SelectMany(modelState => modelState.Errors).FirstOrDefault().ErrorMessage;
                return this.View("All", viewModel);
            }

            await this.mealPlansService.CreateMealPLan(input);

            return this.RedirectToAction("All");
        }

        [Authorize(Roles = GlobalConstants.TrainerRoleName)]
        public async Task<IActionResult> AddMealPlanToUser(string username, int mealPlanId)
        {
            var user = this.usersService.GetUserByUserName(username);
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);

            if (user == null)
            {
                this.TempData["sErrMsg"] = "User not Found";
                return this.View("All", new ComplexViewModelForMealPlans()
                {
                    ViewModel = this.mealPlansService.GetAll(1),
                    InputModel = new AddMealPlanInputModel(),
                });
            }

            if (trainer.Clients.All(x => x.UserName != username))
            {
                this.TempData["sErrMsg"] = "This trainee is not yours ";
                return this.View("All", new ComplexViewModelForMealPlans()
                {
                    ViewModel = this.mealPlansService.GetAll(1),
                    InputModel = new AddMealPlanInputModel(),
                });
            }

            if (this.mealPlansService.UserHasMealPlan(user.Id, mealPlanId))
            {
                this.TempData["sErrMsg"] = "This trainee already has this meal plan ";
                return this.View("All", new ComplexViewModelForMealPlans()
                {
                    ViewModel = this.mealPlansService.GetAll(1),
                    InputModel = new AddMealPlanInputModel(),
                });
            }

            await this.mealPlansService.AddMealPlanToUser(user.Id, mealPlanId);

            return this.RedirectToAction("All");
        }

        [Authorize]
        public IActionResult CreateFood(int mealId)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }

            this.ViewBag.MealName = this.mealPlansService.GetMealName(mealId);
            var input = new AddFoodInputModel();
            //maybe a food controller
            return this.View(input);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFood(AddFoodInputModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            //maybe a food controller
            await this.mealPlansService.CreateFood(input);
            return this.RedirectToAction("All");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMeal(string mealName, int mealPlanId)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }

            if (mealName == null || mealName.Length < 5 || mealName.Length > 40 || string.IsNullOrWhiteSpace(mealName.TrimEnd()))
            {
                return this.Redirect($"/MealPlans/Details?id={mealPlanId}");
            }

            await this.mealPlansService.CreateMeal(mealName, mealPlanId);
            //maybe a food controller
            return this.Redirect($"/MealPlans/Details?id={mealPlanId}");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }
            await this.mealPlansService.DeleteMealPLan(id);
            return this.RedirectToAction("All");
        }

        [Authorize]
        public async Task<IActionResult> DeleteFood(int mealPlanId, int foodId)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }

            await this.mealPlansService.DeleteFood(foodId);

            return this.Redirect($"/MealPlans/Details?id={mealPlanId}");
        }

        [Authorize]
        public async Task<IActionResult> DeleteMeal(int mealId, int mealPlanId)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }
            await this.mealPlansService.DeleteMeal(mealId);

            return this.Redirect($"/MealPlans/Details?id={mealPlanId}");
        }

        public PartialViewResult ShowError(string sErrorMessage)
        {
            return this.PartialView("_ErrorPopup");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Restore(int id)
        {
            await this.mealPlansService.RestoreMealPlan(id);
            return this.RedirectToAction("All");
        }
    }
}

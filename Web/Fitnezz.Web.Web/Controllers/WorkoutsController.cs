using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Workouts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutsService workoutsService;
        private readonly IUsersService usersService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public WorkoutsController(IWorkoutsService workoutsService, IUsersService usersService, SignInManager<ApplicationUser> signInManager)
        {
            this.workoutsService = workoutsService;
            this.usersService = usersService;
            this.signInManager = signInManager;
        }

        public IActionResult All( int pageNumber = 1)
        {
            PaginatedList<AllWourkoutsViewModel> viewModel = null;

            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                 viewModel = this.workoutsService.GetAll(pageNumber);
            }
            else if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                viewModel = this.workoutsService.GetAllWithDeletedWorkouts(pageNumber);
            }
            else
            {
                 viewModel = this.workoutsService.GetAllPublic(pageNumber);
            }

            return this.View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string workoutName,string isPublic)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }

            if (workoutName == null || string.IsNullOrWhiteSpace(workoutName.TrimEnd()) || workoutName.TrimEnd().Length < 5 || workoutName.TrimEnd().Length > 30)
            {
                this.TempData["sErrMsg"] = "Workout name cannot be empty and should be between 5 and 30 characters";
                return this.View("All", this.workoutsService.GetAll(1));
            }

            await this.workoutsService.Create(workoutName, isPublic);

            return this.RedirectToAction("All");
        }

        public IActionResult Details(int id)
        {
            var model = this.workoutsService.GetWorkoutDetails(id);
            if (!model.IsPublic)
            {
                if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    if (!this.signInManager.IsSignedIn(this.User))
                    {
                        return this.NotFound();
                    }

                    var userId = this.usersService.GetUserByUserName(this.User.Identity.Name).Id;
                    if (!this.usersService.UserHasWorkout(userId, id))
                    {
                        return this.NotFound();
                    }
                }
            }

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }
            await this.workoutsService.DeleteWorkout(id);

            return this.RedirectToAction("All");
        }

        [Authorize]
        public IActionResult AddExerciseToWorkout(int workoutId)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }

            this.ViewBag.WorkoutName = this.workoutsService.GetWorkoutName(workoutId);
            // maybe a exercise controller
            var input = new AddExerciseToWorkoutInputModel();
            return this.View(input);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddExerciseToWorkout(AddExerciseToWorkoutInputModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.TrainerRoleName) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // maybe a exercise controller
            await this.workoutsService.CreateExercise(input);
            // todo: create a new exercise and add it to the current workoutId
            return this.RedirectToAction("All");
        }

        public PartialViewResult ShowError(string sErrorMessage)
        {
            return this.PartialView("_ErrorPopup");
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.TrainerRoleName)]
        public async Task<IActionResult> AddWorkoutToUser(string username, int workoutId)
        {

            var user = this.usersService.GetUserByUserName(username);
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);

            if (user == null)
            {
                this.TempData["sErrMsg"] = "User not Found";
                return this.View("All",this.workoutsService.GetAll(1));
            }

            if (trainer.Clients.All(x => x.UserName != username))
            {
                this.TempData["sErrMsg"] = "This trainee is not yours ";
                return this.View("All", this.workoutsService.GetAll(1));
            }

            if (this.usersService.UserHasWorkout(user.Id,workoutId))
            {
                this.TempData["sErrMsg"] = "This trainee already has this workout ";
                return this.View("All", this.workoutsService.GetAll(1));
            }

            await this.workoutsService.AddWorkoutToUserAsync(user.Id, workoutId);

            return this.Redirect("/Workouts/All");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> RestoreWorkout(int id)
        {
            await this.workoutsService.RestoreWorkout(id);

            return this.RedirectToAction("All");
        }
    }
}
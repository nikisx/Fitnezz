using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.Workouts;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutsService workoutsService;
        private readonly IUsersService usersService;

        public WorkoutsController(IWorkoutsService workoutsService, IUsersService usersService)
        {
            this.workoutsService = workoutsService;
            this.usersService = usersService;
        }

        public IActionResult All( int pageNumber = 1)
        {

            var viewModel = this.workoutsService.GetAll(pageNumber);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string workoutName)
        {
            if (workoutName == null || string.IsNullOrWhiteSpace(workoutName.TrimEnd()) || workoutName.TrimEnd().Length < 5 || workoutName.TrimEnd().Length > 30)
            {
                this.TempData["sErrMsg"] = "Workout name cannot be empty and should be between 5 and 30 characters";
                return this.View("All", this.workoutsService.GetAll(1));
            }

            await this.workoutsService.Create(workoutName);

            return this.RedirectToAction("All");
        }

        public IActionResult Details(int id)
        {
            var model = this.workoutsService.GetWorkoutDetails(id);

            return this.View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.workoutsService.DeleteWorkout(id);

            return this.RedirectToAction("All");
        }

        public IActionResult AddExerciseToWorkout(int workoutId)
        {
            this.ViewBag.WorkoutName = this.workoutsService.GetWorkoutName(workoutId);
            // maybe a exercise controller
            // todo: returns a workoutName string viewmodel
            var input = new AddExerciseToWorkoutInputModel();
            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> AddExerciseToWorkout(AddExerciseToWorkoutInputModel input)
        {
            if (!ModelState.IsValid)
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
    }
}
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

        public WorkoutsController(IWorkoutsService workoutsService)
        {
            this.workoutsService = workoutsService;
        }
        public IActionResult All()
        {
            var viewModel = this.workoutsService.GetAll();
            return this.View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(string workoutName)
        {
            if (workoutName == null || string.IsNullOrEmpty(workoutName) || workoutName.TrimEnd().Length < 5 || workoutName.TrimEnd().Length > 30)
            {
                return this.RedirectToAction("All");
            }

            await this.workoutsService.Create(workoutName);

            return this.RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult AddWorkoutToUser(string username, int workoutId)
        {

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
    }
}


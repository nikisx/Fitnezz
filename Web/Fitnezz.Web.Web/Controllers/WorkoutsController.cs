using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        public IActionResult All()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(string workoutName)
        {
            if (workoutName == null)
            {
                return this.Redirect("/");
            }

            return this.RedirectToAction("All");
        }

        
        [HttpPost]
        public IActionResult AddWorkoutToUser(string username, int workoutId)
        {
         

            return this.RedirectToAction("All");
        }

        public IActionResult Details(int id)
        {

            this.ViewBag.Id = id;
            return this.View();
        }

        public IActionResult Delete(int id)
        {


            return this.RedirectToAction("All");
        }

        public IActionResult AddExerciseToWorkout(int workoutId)
        {
            // maybe a exercise controller
            // todo: returns a workoutName string viewmodel
            return this.View();
        }
        [HttpPost]
        public IActionResult AddExerciseToWorkout(int workoutId, string exerciseName, string exerciseSets, string exerciseReps, string exerciseDistance, string exerciseTime, string exerciseLink)
        {
            // maybe a exercise controller
            // todo: create a new exercise and add it to the current workoutId
            return this.RedirectToAction("All");
        }
    }
}


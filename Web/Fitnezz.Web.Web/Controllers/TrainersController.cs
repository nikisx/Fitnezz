using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class TrainersController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Hire(string id)
        {
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
            return this.View();
        }

        [HttpPost]
        public IActionResult AddWorkoutToUser(string username, int workoutId)
        {

            return this.RedirectToAction("All");
        }
    }
}

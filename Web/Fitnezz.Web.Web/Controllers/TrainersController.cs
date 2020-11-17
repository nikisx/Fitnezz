using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainersService trainersService;

        public TrainersController(ITrainersService trainersService)
        {
            this.trainersService = trainersService;
        }

        public IActionResult All()
        {
            var viewModel = this.trainersService.GetAll();
            return View(viewModel);
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

            await this.trainersService.Create(input);

            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult AddWorkoutToUser(string username, int workoutId)
        {

            return this.Redirect("/Workouts/All");
        }

    }
}

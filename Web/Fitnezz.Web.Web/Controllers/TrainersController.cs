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
        private readonly IUsersService usersService;

        public TrainersController(ITrainersService trainersService, IUsersService usersService)
        {
            this.trainersService = trainersService;
            this.usersService = usersService;
        }

        public IActionResult All()
        {
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);
            var viewModel = this.trainersService.GetAll();
            return View(viewModel);
        }

        public async Task<IActionResult> Hire(string id)
        {
            var user = this.usersService.GetUser(this.User.Identity.Name);

            await this.trainersService.GetHired(id, user.Id);

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

        public IActionResult Clients()
        {
            var trainer = this.usersService.GetTrainer(this.User.Identity.Name);
            var viewModel = this.trainersService.GetClients(trainer.Id);
            return this.View(viewModel);
        }
    }
}

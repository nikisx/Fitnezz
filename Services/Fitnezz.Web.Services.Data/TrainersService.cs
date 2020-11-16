using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Identity;

namespace Fitnezz.Web.Services.Data
{
    public class TrainersService : ITrainersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> traineRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public TrainersService(IDeletableEntityRepository<ApplicationUser> traineRepository, UserManager<ApplicationUser> userManager)
        {
            this.traineRepository = traineRepository;
            this.userManager = userManager;
        }

        public async Task Create(TrainerCreateInputModel input)
        {
            var trainer = new ApplicationUser()
            {
                Name = input.Name,
                Age = input.Age,
                Specialty = input.Specialty,
                Img = input.Img,
                UserName = input.Name.Replace(" ",string.Empty),
            };

            var res = await this.userManager.CreateAsync(trainer, input.Password);

            if (res.Succeeded)
            {
                await this.userManager.AddToRoleAsync(trainer, "Trainer");
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<AllTrainersViewModel> GetAll()
        {
            return this.traineRepository.All().Where(x => x.Img != null).Select(x => new AllTrainersViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Clients = x.Clients.Count,
                Specialty = x.Specialty,
                Age = x.Age,
                Img = x.Img,
            }).ToList();
        }

        public async Task GetHired(string trainerId, string userId)
        {
            var trainer = this.traineRepository.All().FirstOrDefault(x => x.Id == trainerId);
            var user = this.traineRepository.All().FirstOrDefault(x => x.Id == userId);

            trainer.Clients.Add(user);
            user.TrainerId = trainer.Id;

            await this.traineRepository.SaveChangesAsync();
        }
    }
}
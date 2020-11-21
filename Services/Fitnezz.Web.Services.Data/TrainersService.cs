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
        private readonly IDeletableEntityRepository<TraineesWorkouts> traineesWorkoutsRepository;

        public TrainersService(IDeletableEntityRepository<ApplicationUser> traineRepository, UserManager<ApplicationUser> userManager, IDeletableEntityRepository<TraineesWorkouts> traineesWorkoutsRepository)
        {
            this.traineRepository = traineRepository;
            this.userManager = userManager;
            this.traineesWorkoutsRepository = traineesWorkoutsRepository;
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
            var a = trainer.Clients;
            await this.traineRepository.SaveChangesAsync();
        }

        public IEnumerable<AllClientsViewModel> GetClients(string trainerId)
        {
            var trainer = this.traineRepository.All().Where(x => x.TrainerId == trainerId);

            return trainer.Select(x => new AllClientsViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Age = x.Age,
                Weight = x.Weight,
                Height = x.Height,
                Goal = x.Goal,
                WorkoutsCount = x.Workouts.Count,
                MealPlansCount = x.MealPlans.Count,
            }).ToList();
        }

        public async Task DeleteUsersWorkout(string userId, int workoutId)
        {
            var userWorkout = this.traineesWorkoutsRepository.All()
                .FirstOrDefault(x => x.WorkoutId == workoutId && x.TraineeId == userId);

            this.traineesWorkoutsRepository.HardDelete(userWorkout);
            await this.traineesWorkoutsRepository.SaveChangesAsync();
        }
    }
}
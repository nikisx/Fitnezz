using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Fitnezz.Web.Web.ViewModels.Trainers;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;
        private readonly IDeletableEntityRepository<TraineesWorkouts> userWourkoutsRepository;
        private readonly IDeletableEntityRepository<Food> foodRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository, IDeletableEntityRepository<Workout> workoutsRepository, IDeletableEntityRepository<TraineesWorkouts> userWourkoutsRepository,IDeletableEntityRepository<Food> foodRepository)
        {
            this.userRepository = userRepository;
            this.workoutsRepository = workoutsRepository;
            this.userWourkoutsRepository = userWourkoutsRepository;
            this.foodRepository = foodRepository;
        }

        public ApplicationUser GetUserByUserName(string username)
        {
            return this.userRepository.All().FirstOrDefault(x => x.UserName == username && x.Img == null);
        }

        public ApplicationUser GetTrainer(string username)
        {
            return this.userRepository.All().FirstOrDefault(x => x.UserName == username && x.Img != null);
        }

        public List<List<AllWourkoutsViewModel>> GetAllUsersWorkout(string userId)
        {
            return this.userRepository.All().Where(x => x.Id == userId).Select(x => x.Workouts.Select(w =>
                new AllWourkoutsViewModel()
                {
                    UserId = x.Id,
                    Id = w.Workout.Id,
                    Name = w.Workout.Name,
                    ExercisesCount = w.Workout.Exercises.Count(),
                }).ToList()).ToList();
        }

        public bool UserHasWorkout(string userId, int workoutId)
        {
            return this.userWourkoutsRepository.All()
                .Any(x => x.TraineeId == userId && x.WorkoutId == workoutId);
        }

        public ApplicationUser GetUserById(string id)
        {
            return this.userRepository.All().FirstOrDefault(x=>x.Id == id);
        }

        public List<List<AllMealPLansViewModel>> GetUserMealPlans(string userId)
        {
            return this.userRepository.All().Where(x => x.Id == userId).Select(x => x.MealPlans.Select(w =>
                new AllMealPLansViewModel()
                {
                    Name = w.MealPlan.Name,
                    Img = w.MealPlan.Img,
                    Calories = this.foodRepository.All().Where(f => f.Meal.MealPlanId == w.MealPlanId && f.Meal.IsDeleted == false).Select(c => c.Calories).Sum(),
                    Proteins = this.foodRepository.All().Where(f => f.Meal.MealPlanId == w.MealPlanId && f.Meal.IsDeleted == false).Select(c => c.Proteins).Sum(),
                    Carbs = this.foodRepository.All().Where(f => f.Meal.MealPlanId == w.MealPlanId && f.Meal.IsDeleted == false).Select(c => c.Carbs).Sum(),
                    Fats = this.foodRepository.All().Where(f => f.Meal.MealPlanId == w.MealPlanId && f.Meal.IsDeleted == false).Select(c => c.Fats).Sum(),
                    Id = w.MealPlanId,
                    UserId = userId,

                }).ToList()).ToList();
        }

        public async Task UpdateProfile(ProfileUpdateInputModel input, string userId)
        {
            var user = this.userRepository.All().FirstOrDefault(x => x.Id == userId);

            user.UserName = input.UserName;
            user.Age = input.Age;
            user.Weight = input.Weight;
            user.Height = input.Height;
            user.Goal = input.Goal;

            this.userRepository.Update(user);

            await this.userRepository.SaveChangesAsync();
        }

        public AllTrainersViewModel GetUserTrainer(string trainerId)
        {
            return this.userRepository.All().Select(x => new AllTrainersViewModel()
            {
                Img = x.Img,
                Id = x.Id,
                Name = x.Name,
                Clients = x.Clients.Count(),
                Specialty = x.Specialty,
                Age = x.Age,
            }).FirstOrDefault(x => x.Id == trainerId);
        }

        public double CalculateCalories(Goals goal, double weight)
        {
            double result = 0;

            switch (goal)
            {
                case Goals.GainWeight:
                    result = weight * 36;
                    break;
                case Goals.LoseWeight:
                    result = weight * 28;
                    break;
                case Goals.Maintain:
                    result = weight * 32;
                    break;
                case Goals.MiniCut:
                    result = weight * 24;
                    break;
            }

            return result;
        }
    }
}
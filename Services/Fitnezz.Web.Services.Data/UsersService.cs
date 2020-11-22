using System.Collections.Generic;
using System.Linq;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;
        private readonly IDeletableEntityRepository<TraineesWorkouts> userWourkoutsRepository;
        private readonly IDeletableEntityRepository<Meal> mealRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository, IDeletableEntityRepository<Workout> workoutsRepository, IDeletableEntityRepository<TraineesWorkouts> userWourkoutsRepository,IDeletableEntityRepository<Meal> mealRepository)
        {
            this.userRepository = userRepository;
            this.workoutsRepository = workoutsRepository;
            this.userWourkoutsRepository = userWourkoutsRepository;
            this.mealRepository = mealRepository;
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
                    UserId = x.Id,
                    Id = w.MealPlan.Id,
                    Name = w.MealPlan.Name,
                    Img = w.MealPlan.Img,
                    Calories = this.mealRepository.All().Where(a => a.MealPlanId == w.MealPlan.Id).Select(c => c.Foods.Sum(f => f.Calories)).ToList(),
                    Proteins = this.mealRepository.All().Where(a => a.MealPlanId == w.MealPlan.Id).Select(c => c.Foods.Sum(f => f.Proteins)).ToList(),
                    Carbs = this.mealRepository.All().Where(a => a.MealPlanId == w.MealPlan.Id).Select(c => c.Foods.Sum(f => f.Carbs)).ToList(),
                    Fats = this.mealRepository.All().Where(a => a.MealPlanId == w.MealPlan.Id).Select(c => c.Foods.Sum(f => f.Fats)).ToList(),

                }).ToList()).ToList();
        }
    }
}
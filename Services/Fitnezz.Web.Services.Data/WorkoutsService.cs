﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Workouts;

namespace Fitnezz.Web.Services.Data
{
    public class WorkoutsService : IWorkoutsService
    {
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;
        private readonly IDeletableEntityRepository<TraineesWorkouts> traineeWorkoutsRepository;

        public WorkoutsService(IDeletableEntityRepository<Workout> workoutsRepository, IDeletableEntityRepository<Exercise> exerciseRepository,IDeletableEntityRepository<TraineesWorkouts> traineeWorkoutsRepository)
        {
            this.workoutsRepository = workoutsRepository;
            this.exerciseRepository = exerciseRepository;
            this.traineeWorkoutsRepository = traineeWorkoutsRepository;
        }

        public PaginatedList<AllWourkoutsViewModel> GetAll(int pageNumber)
        {
            var allWorkouts = this.workoutsRepository.All().OrderByDescending(x => x.CreatedOn).Select(x =>
                new AllWourkoutsViewModel
                {
                    Name = x.Name,
                    ExercisesCount = x.Exercises.Count,
                    Id = x.Id,
                });

            var paginatedList = new PaginatedList<AllWourkoutsViewModel>().CreateAsync(allWorkouts, pageNumber, 5).GetAwaiter().GetResult();

            return paginatedList;
        }

        public async Task Create(string name, string isPublic)
        {
            var workout = new Workout()
            {
                Name = name,
                IsPublic = isPublic == "Public" ? true : false,
            };

            await this.workoutsRepository.AddAsync(workout);
            await this.workoutsRepository.SaveChangesAsync();
        }

        public string GetWorkoutName(int id)
        {
            return this.workoutsRepository.All().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
        }

        public DetailsWorkoutsVIewModel GetWorkoutDetails(int id)
        {
            return this.workoutsRepository.All().Where(x => x.Id == id).Select(x => new DetailsWorkoutsVIewModel
            {
                IsPublic = x.IsPublic,
                Name = x.Name,
                Id = x.Id,
                Exercises = x.Exercises,
            }).FirstOrDefault();
        }

        public async Task CreateExercise(AddExerciseToWorkoutInputModel input)
        {
            var exercise = new Exercise()
            {
                Name = input.Name,
                Sets = input.Sets,
                Reps = input.Reps,
                Distance = input.Distance,
                Time = input.Time,
                Link = input.Link,
                WorkoutId = input.WorkoutId,
            };

            await this.exerciseRepository.AddAsync(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }

        public async Task DeleteWorkout(int id)
        {
            var workout = this.workoutsRepository.All().FirstOrDefault(x => x.Id == id);
            var traineesWorkouts = this.traineeWorkoutsRepository.All().Where(x => x.WorkoutId == id).ToList();

            foreach (var traineesWorkout in traineesWorkouts)
            {
                this.traineeWorkoutsRepository.HardDelete(traineesWorkout);
            }

            this.workoutsRepository.Delete(workout);
            await this.workoutsRepository.SaveChangesAsync();
        }

        public async Task AddWorkoutToUserAsync(string userId, int workoutId)
        {
            var trainneWorkout = new TraineesWorkouts()
            {
                TraineeId = userId,
                WorkoutId = workoutId,
            };

            await this.traineeWorkoutsRepository.AddAsync(trainneWorkout);
            await this.traineeWorkoutsRepository.SaveChangesAsync();
        }

        public PaginatedList<AllWourkoutsViewModel> GetAllPublic(int pageNumber)
        {
            var allPublicWorkouts = this.workoutsRepository.All().Where(x=>x.IsPublic == true).OrderByDescending(x => x.CreatedOn).Select(x =>
                new AllWourkoutsViewModel
                {
                    Name = x.Name,
                    ExercisesCount = x.Exercises.Count,
                    Id = x.Id,
                });

            var paginatedList = new PaginatedList<AllWourkoutsViewModel>().CreateAsync(allPublicWorkouts, pageNumber, 5).GetAwaiter().GetResult();

            return paginatedList;
        }

        public PaginatedList<AllWourkoutsViewModel> GetAllWithDeletedWorkouts(int pageNumber)
        {
            var allPublicWorkouts = this.workoutsRepository.AllWithDeleted().OrderByDescending(x => x.CreatedOn).Select(x =>
                new AllWourkoutsViewModel
                {
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                    ExercisesCount = x.Exercises.Count,
                    Id = x.Id,
                });

            var paginatedList = new PaginatedList<AllWourkoutsViewModel>().CreateAsync(allPublicWorkouts, pageNumber, 5).GetAwaiter().GetResult();

            return paginatedList;
        }

        public async Task RestoreWorkout(int id)
        {
            var workout = this.workoutsRepository.AllWithDeleted().FirstOrDefault(x => x.Id == id);

            this.workoutsRepository.Undelete(workout);
            await this.workoutsRepository.SaveChangesAsync();
        }
    }
}
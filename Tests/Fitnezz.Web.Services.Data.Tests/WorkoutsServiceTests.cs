using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Fitnezz.Web.Web.ViewModels.Workouts;
using Moq;
using Xunit;

namespace Fitnezz.Web.Services.Data.Tests
{
    public class WorkoutsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Workout>> workoutsRepository;
        private readonly Mock<IDeletableEntityRepository<Exercise>> exerciseRepository;
        private readonly Mock<IDeletableEntityRepository<TraineesWorkouts>> traineeWorkoutsRepository;
        private readonly List<Workout> dbWorkouts;
        private readonly List<Exercise> dbExercises;
        private readonly List<TraineesWorkouts> dbTraineeWorkouts;

        public WorkoutsServiceTests()
        {
            this.workoutsRepository = new Mock<IDeletableEntityRepository<Workout>>();
            this.exerciseRepository = new Mock<IDeletableEntityRepository<Exercise>>();
            this.traineeWorkoutsRepository = new Mock<IDeletableEntityRepository<TraineesWorkouts>>();
            this.dbWorkouts = new List<Workout>();
            this.dbExercises = new List<Exercise>();
            this.dbTraineeWorkouts = new List<TraineesWorkouts>();
        }

        [Fact]
        public async Task CreateWorkoutTest()
        {
            this.workoutsRepository.Setup(x => x.AddAsync(It.IsAny<Workout>())).Callback((Workout workout) => this.dbWorkouts.Add(workout));
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.Create("Test", "Public");

            Assert.Single(this.dbWorkouts);
        }

        [Fact]
        public async Task GetCorrectWorkoutName()
        {
            this.workoutsRepository.Setup(x => x.AddAsync(It.IsAny<Workout>())).Callback((Workout workout) => this.dbWorkouts.Add(workout));
            this.workoutsRepository.Setup(x => x.All()).Returns(this.dbWorkouts.AsQueryable());
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.Create("Test", "Public");
            var workout = this.dbWorkouts.FirstOrDefault();
            var workoutName = service.GetWorkoutName(workout.Id);

            Assert.Equal(workout.Name, workoutName);
        }

        [Fact]
        public async Task GetCorrectWorkout()
        {
            this.workoutsRepository.Setup(x => x.AddAsync(It.IsAny<Workout>())).Callback((Workout workout) => this.dbWorkouts.Add(workout));
            this.workoutsRepository.Setup(x => x.All()).Returns(this.dbWorkouts.AsQueryable());
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.Create("Test", "Public");
            var expectedWorkout = this.dbWorkouts.FirstOrDefault();
            var actualWorkout = service.GetWorkoutDetails(expectedWorkout.Id);

            Assert.Equal(expectedWorkout.Id, actualWorkout.Id);
        }

        [Fact]
        public async Task CreateExerciseTest()
        {
            this.exerciseRepository.Setup(x => x.AddAsync(It.IsAny<Exercise>())).Callback((Exercise exercise) => this.dbExercises.Add(exercise));
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.CreateExercise(new AddExerciseToWorkoutInputModel());

            Assert.Single(this.dbExercises);
        }

        [Fact]
        public async Task DeleteWorkoutTest()
        {
            this.workoutsRepository.Setup(x => x.AddAsync(It.IsAny<Workout>())).Callback((Workout workout) => this.dbWorkouts.Add(workout));
            this.workoutsRepository.Setup(x => x.All()).Returns(this.dbWorkouts.AsQueryable());
            this.workoutsRepository.Setup(x => x.Delete(It.IsAny<Workout>())).Callback((Workout workout) => this.dbWorkouts.Remove(workout));
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.Create("Test", "Public");
            var workoutId = this.dbWorkouts.FirstOrDefault().Id;
            await service.DeleteWorkout(workoutId);

            Assert.Empty(this.dbWorkouts);
        }

        [Fact]
        public async Task CreateTraineeWorkoutTest()
        {
            this.traineeWorkoutsRepository.Setup(x => x.AddAsync(It.IsAny<TraineesWorkouts>())).Callback((TraineesWorkouts workout) => this.dbTraineeWorkouts.Add(workout));
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.AddWorkoutToUserAsync("Test", 1);

            Assert.Single(this.dbTraineeWorkouts);
        }

        [Fact]
        public async Task RestoreWorkoutTest()
        {
            this.workoutsRepository.Setup(x => x.AddAsync(It.IsAny<Workout>())).Callback((Workout workout) => this.dbWorkouts.Add(workout));
            this.workoutsRepository.Setup(x => x.Delete(It.IsAny<Workout>())).Callback((Workout workout) => workout.IsDeleted = true);
            this.workoutsRepository.Setup(x => x.Undelete(It.IsAny<Workout>())).Callback((Workout workout) => workout.IsDeleted = false);
            this.workoutsRepository.Setup(x => x.All()).Returns(this.dbWorkouts.Where(m => m.IsDeleted == false).AsQueryable());
            this.workoutsRepository.Setup(x => x.AllWithDeleted()).Returns(this.dbWorkouts.AsQueryable());
            var service = new WorkoutsService(this.workoutsRepository.Object, this.exerciseRepository.Object, this.traineeWorkoutsRepository.Object);

            await service.Create("Test", "Public");
            var workoutId = this.dbWorkouts.FirstOrDefault().Id;
            await service.DeleteWorkout(workoutId);
            await service.RestoreWorkout(workoutId);
            var countUnDeleted = this.dbWorkouts.Count(x => x.IsDeleted == false);

            Assert.Equal(1, countUnDeleted);
        }
    }
}
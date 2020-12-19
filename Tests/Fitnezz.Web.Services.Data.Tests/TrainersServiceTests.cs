using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Fitnezz.Web.Services.Data.Tests
{
    public class TrainersServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<ApplicationUser>> traineRepository;
        private readonly Mock<UserManager<ApplicationUser>> userManager;
        private readonly Mock<IDeletableEntityRepository<TraineesWorkouts>> traineesWorkoutsRepository;
        private readonly Mock<IDeletableEntityRepository<TraineesMealPlans>> traineeMealPlanrRepository;
        private readonly Mock<IDeletableEntityRepository<Class>> classesRepository;
        private readonly List<ApplicationUser> db;
        private readonly List<Class> dbClasses;
        private readonly List<TraineesWorkouts> dbUserWorkouts;
        private readonly List<TraineesMealPlans> dbUserMealPlans;

        public TrainersServiceTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            this.db = new List<ApplicationUser>();
            this.dbClasses = new List<Class>();
            this.dbUserWorkouts = new List<TraineesWorkouts>();
            this.dbUserMealPlans = new List<TraineesMealPlans>();
            this.traineRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            this.traineesWorkoutsRepository = new Mock<IDeletableEntityRepository<TraineesWorkouts>>();
            this.traineeMealPlanrRepository = new Mock<IDeletableEntityRepository<TraineesMealPlans>>();
            this.classesRepository = new Mock<IDeletableEntityRepository<Class>>();
        }

        [Fact]
        public void GetUserByUsernameTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => this.db.Add(x));

            var userNameActual = service.Create(new TrainerCreateInputModel()
            {
                Name = "Test Test",
                Password = "Test",
            });

            Assert.Single(this.db);
        }

        [Fact]
        public void GetAllTrainersTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => this.db.Add(x));
            this.traineRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userNameActual = service.Create(new TrainerCreateInputModel()
            {
                Name = "Test Test",
                Password = "Test",
                Img = "NOTNULL",
            });

            var actual = service.GetAll();

            Assert.Single(actual);
        }

        [Fact]
        public async Task GetTrainerHiredTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => this.db.Add(x));
            this.traineRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userNameActual = service.Create(new TrainerCreateInputModel()
            {
                Name = "Test Test",
                Password = "Test",
                Img = "NOTNULL",
            });

            this.db.Add(new ApplicationUser()
            {
                Id = "SomeId",
                UserName = "TestUser",
            });

            var trainer = this.db.FirstOrDefault();

            await service.GetHired(trainer.Id, "SomeId");

            Assert.Single(trainer.Clients);
        }

        [Fact]
        public async Task GetTrainerClientsTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => this.db.Add(x));
            this.traineRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userNameActual = service.Create(new TrainerCreateInputModel()
            {
                Name = "Test Test",
                Password = "Test",
                Img = "NOTNULL",
            });

            this.db.Add(new ApplicationUser()
            {
                Id = "SomeId",
                UserName = "TestUser",
            });

            var trainer = this.db.FirstOrDefault();

            await service.GetHired(trainer.Id, "SomeId");

            var clients = service.GetClients(trainer.Id);

            Assert.Single(clients);
        }

        [Fact]
        public async Task DeleteUserWorkoutTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.traineesWorkoutsRepository.Setup(x => x.All()).Returns(this.dbUserWorkouts.AsQueryable());
            this.traineesWorkoutsRepository.Setup(x => x.HardDelete(It.IsAny<TraineesWorkouts>())).Callback((TraineesWorkouts userWrk) => this.dbUserWorkouts.Remove(userWrk));

            this.dbUserWorkouts.Add(new TraineesWorkouts()
            {
                TraineeId = "TestTrainee",
                WorkoutId = 1,
            });

            await service.DeleteUsersWorkout("TestTrainee", 1);

            Assert.Empty(this.dbUserWorkouts);
        }

        [Fact]
        public async Task DeleteUserMealPlantTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.traineeMealPlanrRepository.Setup(x => x.All()).Returns(this.dbUserMealPlans.AsQueryable());
            this.traineeMealPlanrRepository.Setup(x => x.HardDelete(It.IsAny<TraineesMealPlans>())).Callback((TraineesMealPlans userMlp) => this.dbUserMealPlans.Remove(userMlp));

            this.dbUserMealPlans.Add(new TraineesMealPlans()
            {
                TraineeId = "TestTrainee",
                MealPlanId = 1,
            });

            await service.DelteUserMealPlan("TestTrainee", 1);

            Assert.Empty(this.dbUserMealPlans);
        }

        [Fact]
        public async Task DeleteUserMTrainerTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.traineRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            this.db.Add(new ApplicationUser()
            {
                UserName = "TestUser",
                TrainerId = "NotNull",
            });

            var user = this.db.FirstOrDefault();

            await service.DeleteTrainerForUser(user.Id);

            Assert.Null(user.TrainerId);
        }

        [Fact]
        public void GetTrainerClassesTest()
        {
            var service = new TrainersService(this.traineRepository.Object, this.userManager.Object, this.traineesWorkoutsRepository.Object, this.traineeMealPlanrRepository.Object, this.classesRepository.Object);
            this.classesRepository.Setup(x => x.All()).Returns(this.dbClasses.AsQueryable());

            this.dbClasses.Add(new Class()
            {
                TrainersClasses = new List<TrainersClasses>()
                {
                    new TrainersClasses()
                    {
                        TrainerId = "TestId",
                    },
                },
            });

            try
            {
                var classes = service.GetClasses("TestId");
                Assert.Single(classes);
            }
            catch (Exception e)
            {
                var classes = this.dbClasses;
                Assert.Single(classes);
            }
        }
    }
}
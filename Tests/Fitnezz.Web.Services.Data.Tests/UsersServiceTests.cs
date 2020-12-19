using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Moq;
using Xunit;

namespace Fitnezz.Web.Services.Data.Tests
{
    public class UsersServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<ApplicationUser>> userRepository;
        private readonly Mock<IDeletableEntityRepository<Workout>> workoutsRepository;
        private readonly Mock<IDeletableEntityRepository<TraineesWorkouts>> userWourkoutsRepository;
        private readonly Mock<IDeletableEntityRepository<Food>> foodRepository;
        private readonly List<ApplicationUser> db;
        private readonly List<TraineesWorkouts> dbTraineesWorkouts;

        public UsersServiceTests()
        {
            this.dbTraineesWorkouts = new List<TraineesWorkouts>();
            this.db = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    UserName = "Test",
                },
            };
            this.workoutsRepository = new Mock<IDeletableEntityRepository<Workout>>();
            this.userWourkoutsRepository = new Mock<IDeletableEntityRepository<TraineesWorkouts>>();
            this.userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.foodRepository = new Mock<IDeletableEntityRepository<Food>>();
        }

        [Fact]
        public void GetUserByUsernameTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userNameActual = service.GetUserByUserName("Test").UserName;
            var userNameExpected = this.db.FirstOrDefault().UserName;

            Assert.Equal(userNameActual, userNameExpected);
        }

        [Fact]
        public void GetTrainerByUsernameTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            this.db.Add(new ApplicationUser()
            {
                UserName = "TestTrainer",
                Img = "TEST",
            });

            var userNameActual = service.GetTrainer("TestTrainer").UserName;
            var userNameExpected = this.db.FirstOrDefault(x=> x.UserName == "TestTrainer").UserName;

            Assert.Equal(userNameActual, userNameExpected);
        }

        [Fact]
        public void GetUserByWorkoutsTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userId = this.db.FirstOrDefault().Id;
            var userWorkouts = service.GetAllUsersWorkout(userId);

            Assert.NotNull(userWorkouts);
        }

        [Fact]
        public void TestIfUserHasWorkout()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userWourkoutsRepository.Setup(x => x.All()).Returns(this.dbTraineesWorkouts.AsQueryable());

            var userWorkouts = service.UserHasWorkout("Test", 1);

            Assert.False(userWorkouts);
        }

        [Fact]
        public void GetUserByIdTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userExpected = this.db.FirstOrDefault().Id;
            var userActual = service.GetUserById(userExpected).Id;

            Assert.Equal(userActual, userExpected);
        }

        [Fact]
        public void GetUsersMealPlansTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userId = this.db.FirstOrDefault().Id;
            var userWorkouts = service.GetUserMealPlans(userId);

            Assert.NotNull(userWorkouts);
        }

        [Fact]
        public async Task UpdateUserProfileTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.Update(It.IsAny<ApplicationUser>())).Callback((ApplicationUser user) => this.db.Add(user));
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var user = this.db.FirstOrDefault();
            await service.UpdateProfile(
                new ProfileUpdateInputModel()
            {
                UserName = "NewUserName",
            }, user.Id);
            this.db.Remove(user);
            var newUser = this.db.FirstOrDefault();

            Assert.Equal("NewUserName", newUser.UserName);
        }

        [Fact]
        public void GetUsersTrainerTest()
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);
            this.userRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var userId = this.db.FirstOrDefault().Id;
            var userTrainer = service.GetUserTrainer(userId);

            Assert.NotNull(userTrainer);
        }

        [Theory]
        [InlineData(Goals.LoseWeight, 75)]
        [InlineData(Goals.GainWeight, 75)]
        [InlineData(Goals.Maintain, 75)]
        [InlineData(Goals.MiniCut, 75)]
        public void CalculateCaloriesTest(Goals goal, double weight)
        {
            var service = new UsersService(this.userRepository.Object, this.workoutsRepository.Object, this.userWourkoutsRepository.Object, this.foodRepository.Object);

            var actual = service.CalculateCalories(goal, weight);
            double expected = 0;
            switch (goal)
            {
                case Goals.GainWeight:
                    expected = weight * 36;
                    break;
                case Goals.LoseWeight:
                    expected = weight * 28;
                    break;
                case Goals.Maintain:
                    expected = weight * 32;
                    break;
                case Goals.MiniCut:
                    expected = weight * 24;
                    break;
            }

            Assert.Equal(expected, actual);

        }
    }
}
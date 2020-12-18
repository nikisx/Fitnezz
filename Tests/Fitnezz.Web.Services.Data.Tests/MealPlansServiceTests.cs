using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.Classes;
using Fitnezz.Web.Web.ViewModels.MealPlans;
using Moq;
using Xunit;

namespace Fitnezz.Web.Services.Data.Tests
{
    public class MealPlansServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<MealPlan>> mealPlanRepository;
        private readonly Mock<IDeletableEntityRepository<Meal>> mealRepository;
        private readonly Mock<IDeletableEntityRepository<Food>> foodRepository;
        private readonly Mock<IDeletableEntityRepository<TraineesMealPlans>> traineeMealPlanRepository;
        private readonly List<MealPlan> db;
        private readonly List<Meal> dbMeals;
        private readonly List<Food> dbFood;
        private readonly List<TraineesMealPlans> dbUserMealPlans;

        public MealPlansServiceTests()
        {
            this.dbMeals = new List<Meal>();
            this.dbUserMealPlans = new List<TraineesMealPlans>();
            this.dbFood = new List<Food>();
            this.db = new List<MealPlan>();
            this.mealRepository = new Mock<IDeletableEntityRepository<Meal>>();
            this.mealPlanRepository = new Mock<IDeletableEntityRepository<MealPlan>>();
            this.foodRepository = new Mock<IDeletableEntityRepository<Food>>();
            this.traineeMealPlanRepository = new Mock<IDeletableEntityRepository<TraineesMealPlans>>();
        }

        [Fact]
        public async Task CreateMealPlanTest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Add(mealPlan));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel()
            {
                MealPlanName = "TestName",
            });

            Assert.Single(this.db);
        }

        [Fact]
        public async Task DeleteMealPLansTest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Add(mealPlan));
            this.mealPlanRepository.Setup(x => x.Delete(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Remove(mealPlan));
            this.mealPlanRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel()
            {
                MealPlanName = "TestName",
            });

            var mealPlanId = this.db.FirstOrDefault().Id;

            var mealPlans = service.DeleteMealPLan(mealPlanId);

            Assert.Empty(this.db);
        }

        [Fact]
        public async Task GetCorrectMEalPlanTest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Add(mealPlan));
            this.mealPlanRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel()
            {
                MealPlanName = "TestName",
            });
            var id = this.db.FirstOrDefault().Id;

            var mealPlan = service.GetDetails(id);

            Assert.Equal(id, mealPlan.Id);
        }

        [Fact]
        public async Task MealPlanIdShouldBeDiffTest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Add(mealPlan));
            this.mealPlanRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel()
            {
                MealPlanName = "TestName",
            });
            var id = this.db.FirstOrDefault().Id;

            var mealPlan = service.GetDetails(id);

            Assert.NotEqual(12424, mealPlan.Id);
        }

        [Fact]
        public async Task CreateMealTest()
        {
            this.mealRepository.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Add(meal));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMeal("TestMEal", 1);

            Assert.Single(this.dbMeals);
        }

        [Fact]
        public async Task CreateMealShouldBeToTheCorrectMealPlanTest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Add(mealPlan));
            this.mealRepository.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Add(meal));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel()
            {
                MealPlanName = "TestName",
            });
            var mealPLanId = this.db.FirstOrDefault().Id;

            await service.CreateMeal("TestMEal", mealPLanId);
            var mealMealPlanId = this.dbMeals.FirstOrDefault().MealPlanId;

            Assert.Equal(mealPLanId, mealMealPlanId);
        }

        [Fact]
        public async Task CreateMealShouldBeToTheWrongMealPlanTest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan mealPlan) => this.db.Add(mealPlan));
            this.mealRepository.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Add(meal));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel()
            {
                MealPlanName = "TestName",
            });
            var mealPLanId = this.db.FirstOrDefault().Id;

            await service.CreateMeal("TestMEal", mealPLanId);
            var mealMealPlanId = this.dbMeals.FirstOrDefault().MealPlanId;

            Assert.NotEqual(124, mealMealPlanId);
        }

        [Fact]
        public async Task CreateFoodTest()
        {
            this.foodRepository.Setup(x => x.AddAsync(It.IsAny<Food>())).Callback((Food food) => this.dbFood.Add(food));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateFood(new AddFoodInputModel());

            Assert.Single(this.dbFood);
        }

        [Fact]
        public async Task CreatedFoodShouldBeToTheCorrectMealTest()
        {
            this.foodRepository.Setup(x => x.AddAsync(It.IsAny<Food>())).Callback((Food food) => this.dbFood.Add(food));
            this.mealRepository.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Add(meal));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMeal("Test",1);

            var mealId = this.dbMeals.FirstOrDefault().Id;

            await service.CreateFood(new AddFoodInputModel()
            {
                MealId = mealId,
            });

            var foodMealId = this.dbFood.FirstOrDefault().MealId;

            Assert.Equal(mealId,foodMealId);
        }

        [Fact]
        public async Task DeleteFoodTest()
        {
            this.foodRepository.Setup(x => x.AddAsync(It.IsAny<Food>())).Callback((Food food) => this.dbFood.Add(food));
            this.foodRepository.Setup(x => x.HardDelete(It.IsAny<Food>())).Callback((Food food) => this.dbFood.Remove(food));
            this.foodRepository.Setup(x => x.All()).Returns(this.dbFood.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateFood(new AddFoodInputModel());
            var foodId = this.dbFood.FirstOrDefault().Id;
            await service.DeleteFood(foodId);

            Assert.Empty(this.dbFood);
        }

        [Fact]
        public async Task ReturnTheCorrectMealName()
        {
            this.mealRepository.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Add(meal));
            this.mealRepository.Setup(x => x.All()).Returns(this.dbMeals.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMeal("TestMEal", 1);
            var mealId = this.dbMeals.FirstOrDefault().Id;
            var actual = service.GetMealName(mealId);
            var expected = this.dbMeals.FirstOrDefault().Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AddMealPlanToUserTest()
        {
            this.traineeMealPlanRepository.Setup(x => x.AddAsync(It.IsAny<TraineesMealPlans>())).Callback((TraineesMealPlans mealPLan) => this.dbUserMealPlans.Add(mealPLan));
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.AddMealPlanToUser("Test",1);

            Assert.Single(this.dbUserMealPlans);
        }

        [Fact]
        public async Task UserHasMealPlanTest()
        {
            this.traineeMealPlanRepository.Setup(x => x.AddAsync(It.IsAny<TraineesMealPlans>())).Callback((TraineesMealPlans mealPLan) => this.dbUserMealPlans.Add(mealPLan));
            this.traineeMealPlanRepository.Setup(x => x.All()).Returns(this.dbUserMealPlans.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.AddMealPlanToUser("Test", 1);
            var actual = service.UserHasMealPlan("Test", 1);

            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteMealTest()
        {
            this.mealRepository.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Add(meal));
            this.mealRepository.Setup(x => x.Delete(It.IsAny<Meal>())).Callback((Meal meal) => this.dbMeals.Remove(meal));
            this.mealRepository.Setup(x => x.All()).Returns(this.dbMeals.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMeal("Test",1);
            var mealId = this.dbMeals.FirstOrDefault().Id;
            await service.DeleteMeal(mealId);

            Assert.Empty(this.dbMeals);
        }

        [Fact]
        public async Task RestoreMealPlanest()
        {
            this.mealPlanRepository.Setup(x => x.AddAsync(It.IsAny<MealPlan>())).Callback((MealPlan meal) => this.db.Add(meal));
            this.mealPlanRepository.Setup(x => x.Delete(It.IsAny<MealPlan>())).Callback((MealPlan meal) => meal.IsDeleted = true);
            this.mealPlanRepository.Setup(x => x.Undelete(It.IsAny<MealPlan>())).Callback((MealPlan meal) => meal.IsDeleted = false);
            this.mealPlanRepository.Setup(x => x.All()).Returns(this.db.Where(m=>m.IsDeleted == false).AsQueryable());
            this.mealPlanRepository.Setup(x => x.AllWithDeleted()).Returns(this.db.AsQueryable());
            var service = new MealPlansService(this.mealPlanRepository.Object, this.mealRepository.Object, this.foodRepository.Object, this.traineeMealPlanRepository.Object);

            await service.CreateMealPLan(new AddMealPlanInputModel());
            var mealId = this.db.FirstOrDefault().Id;
            await service.DeleteMealPLan(mealId);
            await service.RestoreMealPlan(mealId);
            var countUnDeleted = this.db.Count(x => x.IsDeleted == false);

            Assert.Equal(1,countUnDeleted);
        }
    }
}
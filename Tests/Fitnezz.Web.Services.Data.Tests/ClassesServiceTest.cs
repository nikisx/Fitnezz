using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Moq;
using Xunit;

namespace Fitnezz.Web.Services.Data.Tests
{
    public class ClassesServiceTest
    {
        private readonly Mock<IDeletableEntityRepository<Class>> classRepository;
        private readonly Mock<IRepository<TrainersClasses>> trainerClassesRepository;
        private readonly Mock<IRepository<CardsClasses>> cardsClassesRepository;
        private readonly Mock<IDeletableEntityRepository<ApplicationUser>> trainerRepository;
        private readonly List<Class> db;
        private readonly List<ApplicationUser> dbTrainers;
        private readonly List<CardsClasses> dbCardsClasses;
        private readonly List<TrainersClasses> dbTrainersClasses;

        public ClassesServiceTest()
        {
            this.db = new List<Class>();
            this.dbTrainers = new List<ApplicationUser>();
            this.dbTrainersClasses = new List<TrainersClasses>();
            this.dbCardsClasses = new List<CardsClasses>();
            this.classRepository = new Mock<IDeletableEntityRepository<Class>>();
            this.trainerRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.cardsClassesRepository = new Mock<IRepository<CardsClasses>>();
            this.trainerClassesRepository = new Mock<IRepository<TrainersClasses>>();
        }

        [Fact]
        public void GetAllClassesTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.classRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());

            var classes = service.GetAll();

            Assert.NotNull(classes);
        }

        [Fact]
        public async Task AddTrainerToClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.trainerClassesRepository.Setup(x => x.AddAsync(It.IsAny<TrainersClasses>())).Callback((TrainersClasses trainerCls) => this.dbTrainersClasses.Add(trainerCls));

            await service.AddTrainerToClass("TestId", 1);

            Assert.Single(this.dbTrainersClasses);
        }

        [Fact]
        public async Task IsTrainerJoinedToClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.trainerClassesRepository.Setup(x => x.AddAsync(It.IsAny<TrainersClasses>())).Callback((TrainersClasses trainerCls) => this.dbTrainersClasses.Add(trainerCls));
            this.trainerClassesRepository.Setup(x => x.All()).Returns(this.dbTrainersClasses.AsQueryable());

            await service.AddTrainerToClass("TestId", 1);

            var actual = service.IsTrainerJoinedAlready("TestId", 1);

            Assert.True(actual);
        }

        [Fact]
        public async Task TrainerToClassCountTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.trainerClassesRepository.Setup(x => x.AddAsync(It.IsAny<TrainersClasses>())).Callback((TrainersClasses trainerCls) => this.dbTrainersClasses.Add(trainerCls));
            this.trainerClassesRepository.Setup(x => x.All()).Returns(this.dbTrainersClasses.AsQueryable());

            await service.AddTrainerToClass("TestId", 1);
            var count = service.GetTrainersCount(1);

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task AddUserToClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.cardsClassesRepository.Setup(x => x.AddAsync(It.IsAny<CardsClasses>())).Callback((CardsClasses user) => this.dbCardsClasses.Add(user));

            await service.AddUserToClass("TestId", 1);

            Assert.Single(this.dbCardsClasses);
        }

        [Fact]
        public async Task IsUserJoinedClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.cardsClassesRepository.Setup(x => x.AddAsync(It.IsAny<CardsClasses>())).Callback((CardsClasses user) => this.dbCardsClasses.Add(user));
            this.cardsClassesRepository.Setup(x => x.All()).Returns(this.dbCardsClasses.AsQueryable());

            await service.AddUserToClass("TestId", 1);
            var actual = service.IsUserJoined("TestId", 1);

            Assert.True(actual);
        }

        [Fact]
        public async Task GeJoinedUserCountTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.cardsClassesRepository.Setup(x => x.AddAsync(It.IsAny<CardsClasses>())).Callback((CardsClasses user) => this.dbCardsClasses.Add(user));
            this.cardsClassesRepository.Setup(x => x.All()).Returns(this.dbCardsClasses.AsQueryable());

            await service.AddUserToClass("TestId", 1);
            var count = service.GetUserClassesCount("TestId");

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task UserLeaveClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.cardsClassesRepository.Setup(x => x.AddAsync(It.IsAny<CardsClasses>())).Callback((CardsClasses user) => this.dbCardsClasses.Add(user));
            this.cardsClassesRepository.Setup(x => x.Delete(It.IsAny<CardsClasses>())).Callback((CardsClasses user) => this.dbCardsClasses.Remove(user));
            this.cardsClassesRepository.Setup(x => x.All()).Returns(this.dbCardsClasses.AsQueryable());

            await service.AddUserToClass("TestId", 1);
            await service.LeaveClass("TestId", 1);

            Assert.Empty(this.dbCardsClasses);
        }

        [Fact]
        public async Task IsTrainerCompetenTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.classRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());
            this.trainerRepository.Setup(x => x.All()).Returns(this.dbTrainers.AsQueryable());

            this.dbTrainers.Add(new ApplicationUser()
            {
                Id = "TestTrainer",
                Specialty = "CardioTest",
            });

            this.db.Add(new Class()
            {
                Id = 1,
                Name = "CardioTest",
            });

            var actual = service.IsTrainerCompetent("TestTrainer", 1);

            Assert.True(actual);
        }

        [Fact]
        public async Task TrainerLeaveClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.trainerClassesRepository.Setup(x => x.AddAsync(It.IsAny<TrainersClasses>())).Callback((TrainersClasses user) => this.dbTrainersClasses.Add(user));
            this.trainerClassesRepository.Setup(x => x.Delete(It.IsAny<TrainersClasses>())).Callback((TrainersClasses user) => this.dbTrainersClasses.Remove(user));
            this.trainerClassesRepository.Setup(x => x.All()).Returns(this.dbTrainersClasses.AsQueryable());

            await service.AddTrainerToClass("TestId", 1);
            await service.LeaveClassAsTrainer("TestId", 1);

            Assert.Empty(this.dbTrainersClasses);
        }

        [Fact]
        public async Task DeleteClassTest()
        {
            var service = new ClassesService(this.classRepository.Object, this.trainerClassesRepository.Object, this.cardsClassesRepository.Object, this.trainerRepository.Object);
            this.classRepository.Setup(x => x.All()).Returns(this.db.AsQueryable());
            this.classRepository.Setup(x => x.Delete(It.IsAny<Class>())).Callback((Class @class) => this.db.Remove(@class));

            this.db.Add(new Class()
            {
                Id = 1,
            });

            await service.DeleteClass(1);

            Assert.Empty(this.db);
        }
    }
}
namespace Fitnezz.Web.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Fitnezz.Web.Data;
    using Fitnezz.Web.Data.Common.Repositories;
    using Fitnezz.Web.Data.Models;
    using Fitnezz.Web.Data.Repositories;
    using Fitnezz.Web.Services.Messaging;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CardServiceTests
    {
        private readonly Mock<IRepository<Card>> cardRepo;
        private readonly Mock<IDeletableEntityRepository<ApplicationUser>> userRepo;
        private readonly Mock<IDeletableEntityRepository<Class>> classesRepo;
        private readonly Mock<IRepository<CardsClasses>> cardsCLassesRepo;
        private readonly Mock<IEmailSender> emailSender;
        private readonly List<Card> db;

        public CardServiceTests()
        {
            this.db = new List<Card>();
            this.cardRepo = new Mock<IRepository<Card>>();
            this.classesRepo = new Mock<IDeletableEntityRepository<Class>>();
            this.cardsCLassesRepo = new Mock<IRepository<CardsClasses>>();
            this.userRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            this.emailSender = new Mock<IEmailSender>();
        }

        [Fact]
        public async Task CreateCardTest()
        {
            this.cardRepo.Setup(x => x.AddAsync(It.IsAny<Card>())).Callback((Card card) => db.Add(card));
            var service = new CardsService(this.userRepo.Object, this.cardRepo.Object, this.classesRepo.Object, this.cardsCLassesRepo.Object, this.emailSender.Object);

            await service.Create("TestId");

            Assert.Single(this.db);
        }

        [Fact]
        public async Task ReturnTheCorrectCardDueDate()
        {
            this.cardRepo.Setup(x => x.AddAsync(It.IsAny<Card>())).Callback((Card card) => db.Add(card));
            this.cardRepo.Setup(x => x.All()).Returns(this.db.AsQueryable());
            var service = new CardsService(this.userRepo.Object, this.cardRepo.Object, this.classesRepo.Object, this.cardsCLassesRepo.Object, this.emailSender.Object);

            await service.Create("TestId");
            var actual = this.db.FirstOrDefault().DueDate.Date;

            Assert.Equal(DateTime.Now.Date.AddMonths(1), actual);
        }

        [Fact]
        public async Task ReturnTheCorrectCardExtendedDueDate()
        {
            this.cardRepo.Setup(x => x.AddAsync(It.IsAny<Card>())).Callback((Card card) => db.Add(card));
            this.cardRepo.Setup(x => x.All()).Returns(db.AsQueryable);
            var service = new CardsService(this.userRepo.Object, this.cardRepo.Object, this.classesRepo.Object, this.cardsCLassesRepo.Object, this.emailSender.Object);

            await service.Create("TestId");
            var carId = this.db.FirstOrDefault().Id;
            await service.ExtendUserCard(carId);
            var actual = this.db.FirstOrDefault().DueDate.Date;

            Assert.Equal(DateTime.Now.Date.AddMonths(2), actual);
        }

        [Fact]
        public async Task DeleteIfACardIsInvalid()
        {
            this.cardRepo.Setup(x => x.AddAsync(It.IsAny<Card>())).Callback((Card card) => db.Add(card));
            this.cardRepo.Setup(x => x.Delete(It.IsAny<Card>())).Callback((Card card) => db.Remove(card));
            this.cardRepo.Setup(x => x.All()).Returns(db.AsQueryable);
            var service = new CardsService(this.userRepo.Object, this.cardRepo.Object, this.classesRepo.Object, this.cardsCLassesRepo.Object, this.emailSender.Object);

            await service.Create("TestId");
            var card = this.db.FirstOrDefault();
            card.DueDate = new DateTime(2020, 4, 15);
            await service.DeleteInvalidCards();
            var actual = this.db.Count;

            Assert.Equal(0, actual);
        }

    }
}
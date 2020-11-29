using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Classes;

namespace Fitnezz.Web.Services.Data
{
    public class CardsService : ICardsService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<Card> cardRepository;
        private readonly IDeletableEntityRepository<Class> classesRepository;
        private readonly IRepository<CardsClasses> cardsClassesRepository;

        public CardsService(IDeletableEntityRepository<ApplicationUser> userRepository, IRepository<Card> cardRepository, IDeletableEntityRepository<Class> classesRepository, IRepository<CardsClasses> cardsClassesRepository)
        {
            this.userRepository = userRepository;
            this.cardRepository = cardRepository;
            this.classesRepository = classesRepository;
            this.cardsClassesRepository = cardsClassesRepository;
        }

        public async Task Create(string userId)
        {
            var card = new Card()
            {
                User = this.userRepository.All().FirstOrDefault(x=> x.Id == userId),
                DueDate = DateTime.Now.AddMonths(1),
            };

            await this.cardRepository.AddAsync(card);
            await this.cardRepository.SaveChangesAsync();
        }

        public GetUserCardViewModel GetCard(string userId)
        {
            return this.cardRepository.All().Where(x => x.User.Id == userId).Select(x => new GetUserCardViewModel()
            {
                FromDate = x.CreatedOn.ToShortDateString(),
                DueDate = x.DueDate.ToShortDateString(),
            }).FirstOrDefault();
        }

        public List<List<AllClassesViewModel>> GetUserClasses(string cardId)
        {
            return this.classesRepository.All().Select(x => x.CardsClasses.Where(a=>a.CardId == cardId).Select(c=> new AllClassesViewModel()
            {
                Name = c.Class.Name,
                Image = c.Class.Image,
                DayOfWeek = c.Class.DayOfWeek,
                StartHour = c.Class.StartingHour,
                EndHour = c.Class.FinishingHour,
                Id = c.Class.Id,
                UsersCount = c.Class.CardsClasses.Count,
                TrainersName = c.Class.TrainersClasses.Select(t=>t.Trainer.Name).ToList(),

            }).ToList()).ToList();
        }

        public async Task ExtendUserCard(string cardId)
        {
            var card = this.cardRepository.All().FirstOrDefault(x => x.Id == cardId);

            card.DueDate = card.DueDate.AddMonths(1);

            await this.cardRepository.SaveChangesAsync();
        }

        public async Task DeleteInvalidCards()
        {
            var invalidCards = this.cardRepository.All().Where(x => x.DueDate < DateTime.Now).ToList();

            foreach (var card in invalidCards)
            {
                var cardsClasses = this.cardsClassesRepository.All().Where(x => x.CardId == card.Id).ToList();

                foreach (var cardClass in cardsClasses)
                {
                    this.cardsClassesRepository.Delete(cardClass);
                }

                var invalidUserCards = this.userRepository.All().Where(x => x.CardId == card.Id).ToList();

                foreach (var invalidUserCard in invalidUserCards)
                {
                    invalidUserCard.CardId = null;
                }

                this.cardRepository.Delete(card);
            }

            await this.cardRepository.SaveChangesAsync();
        }
    }
}
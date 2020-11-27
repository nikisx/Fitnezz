using System;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels;

namespace Fitnezz.Web.Services.Data
{
    public class CardsService : ICardsService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<Card> cardRepository;

        public CardsService(IDeletableEntityRepository<ApplicationUser> userRepository, IRepository<Card> cardRepository)
        {
            this.userRepository = userRepository;
            this.cardRepository = cardRepository;
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
    }
}
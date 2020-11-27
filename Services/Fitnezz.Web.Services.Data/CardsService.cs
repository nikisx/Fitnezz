using System;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;

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
    }
}
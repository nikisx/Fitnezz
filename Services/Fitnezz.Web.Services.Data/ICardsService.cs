using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels;

namespace Fitnezz.Web.Services.Data
{
    public interface ICardsService
    {
        Task Create(string userId);

        GetUserCardViewModel GetCard(string userId);
    }
}
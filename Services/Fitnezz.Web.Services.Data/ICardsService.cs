using System.Threading.Tasks;

namespace Fitnezz.Web.Services.Data
{
    public interface ICardsService
    {
        Task Create(string userId);
    }
}
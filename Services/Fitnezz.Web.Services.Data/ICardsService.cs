using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Classes;

namespace Fitnezz.Web.Services.Data
{
    public interface ICardsService
    {
        Task Create(string userId);

        GetUserCardViewModel GetCard(string userId);

        List<List<AllClassesViewModel>> GetUserClasses(string cardId);

        Task ExtendUserCard(string cardId);
    }
}
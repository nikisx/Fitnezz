using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels;
using Fitnezz.Web.Web.ViewModels.Workouts;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private string SearchWord = "";

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public IActionResult All(string searchWord, int pageNumber = 1)
        {
            PaginatedList<AllWourkoutsViewModel> viewModel;

            if (searchWord != null)
            {
                this.SearchWord = searchWord;
            }

            if (this.User.IsInRole(GlobalConstants.TrainerRoleName) || this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                 viewModel = this.searchService.SearchWorkouts(this.SearchWord, pageNumber);
            }
            else
            {
                viewModel = this.searchService.SearchWorkoutsPublic(this.SearchWord, pageNumber);
            }

            return this.View(viewModel);
        }

    }
}

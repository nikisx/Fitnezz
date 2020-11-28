using System.Collections.Generic;
using Fitnezz.Web.Web.ViewModels.Classes;

namespace Fitnezz.Web.Web.ViewModels
{
    public class ComplexModel
    {
        public GetUserCardViewModel ViewModel { get; set; }

        public ProfileUpdateInputModel InputModel { get; set; }

        public List<List<AllClassesViewModel>> ClassesViewModel { get; set; }
    }
}
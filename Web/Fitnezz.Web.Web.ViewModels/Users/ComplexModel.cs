using System.Collections.Generic;
using Fitnezz.Web.Web.ViewModels.Classes;
using Fitnezz.Web.Web.ViewModels.Trainers;

namespace Fitnezz.Web.Web.ViewModels
{
    public class ComplexModel
    {
        public GetUserCardViewModel ViewModel { get; set; }

        public ProfileUpdateInputModel InputModel { get; set; }

        public List<List<AllClassesViewModel>> ClassesViewModel { get; set; }

        public AllTrainersViewModel Trainer { get; set; }
    }
}
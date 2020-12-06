using System;
using System.Linq;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.ViewComponents
{
    public class TakeRandomClassesViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<Class> classesRepository;

        public TakeRandomClassesViewComponent( IDeletableEntityRepository<Class> classesRepository)
        {
            this.classesRepository = classesRepository;
        }

        public IViewComponentResult Invoke(string title)
        {

            var classViewModel = this.classesRepository.All().Select(x=> new RandomClassViewModel()
            {
                Title = title,
                Name = x.Name,
                UsersCount = x.CardsClasses.Count,
                DayOfWeek = x.DayOfWeek,
                StartHour = x.StartingHour,
                EndHour = x.FinishingHour,
                TrainersName = x.TrainersClasses.Where(t=>t.ClassId == x.Id).Select(a=>a.Trainer.Name).ToList(),
                Image = x.Image,
            }).OrderBy(x=> Guid.NewGuid()).Take(3).ToList();

            return this.View(classViewModel);
        }
    }
}
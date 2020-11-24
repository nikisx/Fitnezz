using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Web.ViewModels.Classes;

namespace Fitnezz.Web.Services.Data
{
    public class ClassesService : IClassesService
    {
        private readonly IDeletableEntityRepository<Class> classRepository;

        public ClassesService(IDeletableEntityRepository<Class> classRepository)
        {
            this.classRepository = classRepository;
        }

        public IEnumerable<AllClassesViewModel> GetAll()
        {
            return this.classRepository.All().Select(x => new AllClassesViewModel()
            {
                Name = x.Name,
                DayOfWeek = x.DayOfWeek,
                Image = x.Image,
                StartHour = x.StartingHour,
                EndHour = x.FinishingHour,
                UsersCount = x.CardsClasses.Count,
                TrainersName = x.TrainersClasses.Select(t => t.Trainer.Name).ToList(),
            }).ToList();
        }

        public async Task Create(ClassCreateInputModel input, string path)
        {
            var @class = new Class()
            {
                Name = input.Name,
                DayOfWeek = input.DayOfWeek,
                StartingHour = input.StartHour,
                FinishingHour = input.EndHour,
            };

            var pathArray = path.Split("\\").ToArray();
            var rootPathArr = pathArray.LastOrDefault().Split("/").ToArray();
            var neededPath = rootPathArr[1];

            var physicalPath = $"{path}{@class.Name}.jpg";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await input.Image.CopyToAsync(fileStream);

            var actualPath = $"/{neededPath}/{@class.Name}.jpg";
            @class.Image = actualPath;

            await this.classRepository.AddAsync(@class);
            await this.classRepository.SaveChangesAsync();
        }
    }
}
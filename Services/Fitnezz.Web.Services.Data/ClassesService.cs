﻿using System;
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
        private readonly IRepository<TrainersClasses> trainerClassesRepository;

        public ClassesService(IDeletableEntityRepository<Class> classRepository, IRepository<TrainersClasses> trainerClassesRepository)
        {
            this.classRepository = classRepository;
            this.trainerClassesRepository = trainerClassesRepository;
        }

        public IEnumerable<AllClassesViewModel> GetAll()
        {
            return this.classRepository.All().Select(x => new AllClassesViewModel()
            {
                Id = x.Id,
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
                DayOfWeek = input.DayOfWeek.ToString(),
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

        public async Task AddTrainerToClass(string trainerId, int classId)
        {
            var trainerClass = new TrainersClasses()
            {
                TrainerId = trainerId,
                ClassId = classId,
            };

            await this.trainerClassesRepository.AddAsync(trainerClass);
            await this.trainerClassesRepository.SaveChangesAsync();
        }

        public bool IsTrainerJoinedAlready(string trainerId, int classId)
        {
          return this.trainerClassesRepository.All().Any(x => x.ClassId == classId && x.TrainerId == trainerId);
        }

        public int GetTrainersCount(int classId)
        {
            return this.classRepository.All().Select(x => x.TrainersClasses).Count();
        }
    }
}
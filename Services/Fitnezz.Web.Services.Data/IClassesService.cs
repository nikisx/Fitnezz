﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.Classes;

namespace Fitnezz.Web.Services.Data
{
    public interface IClassesService
    {
        IEnumerable<AllClassesViewModel> GetAll();

        Task Create(ClassCreateInputModel input, string path);

        Task AddTrainerToClass(string trainerId, int classId);

        bool IsTrainerJoinedAlready(string trainerId, int classId);

        int GetTrainersCount(int classId);
    }
}
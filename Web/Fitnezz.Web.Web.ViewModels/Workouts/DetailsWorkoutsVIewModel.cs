using System.Collections.Generic;
using Fitnezz.Web.Data.Models;

namespace Fitnezz.Web.Web.ViewModels.Workouts
{
    public class DetailsWorkoutsVIewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }
}
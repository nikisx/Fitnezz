using System.Collections.Generic;

namespace Fitnezz.Web.Web.ViewModels.Classes
{
    public class AllClassesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DayOfWeek { get; set; }

        public int StartHour { get; set; }

        public int EndHour { get; set; }

        public string Image { get; set; }

        public ICollection<string> TrainersName { get; set; }

        public int UsersCount { get; set; }
    }
}
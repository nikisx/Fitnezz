using System.Collections.Generic;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class Class : BaseDeletableModel<int>
    {
        public Class()
        {
            this.TrainersClasses = new HashSet<TrainersClasses>();
            this.CardsClasses = new HashSet<CardsClasses>();
        }

        public string Name { get; set; }

        public string Image { get; set; }

        public ICollection<CardsClasses> CardsClasses { get; set; }

        public ICollection<TrainersClasses> TrainersClasses { get; set; }

        public string DayOfWeek { get; set; }

        public int StartingHour { get; set; }

        public int FinishingHour { get; set; }
    }
}
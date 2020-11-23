using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class TrainersClasses : BaseModel<int>
    {
        public string TrainerId { get; set; }

        public ApplicationUser Trainer { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }
    }
}
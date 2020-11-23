using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class CardsClasses : BaseModel<int>
    {
        public string CardId { get; set; }

        public Card Card { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }
    }

}
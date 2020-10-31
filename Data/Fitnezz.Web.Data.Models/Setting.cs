namespace Fitnezz.Web.Data.Models
{
    using Fitnezz.Web.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}

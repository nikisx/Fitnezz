// ReSharper disable VirtualMemberCallInConstructor

using System;
using System.Collections.Generic;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class Card : BaseModel<string>
    {
        public Card()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CardsClasses = new HashSet<CardsClasses>();
        }

        public ApplicationUser User { get; set; }

        public DateTime DueDate { get; set; }

        public ICollection<CardsClasses> CardsClasses { get; set; }
    }

}
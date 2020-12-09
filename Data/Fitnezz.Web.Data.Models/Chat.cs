using System;
using System.Collections.Generic;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{
    public class Chat : BaseDeletableModel<string>
    {
        public Chat()
        {
            this.Messages= new HashSet<Message>();
            this.Users = new HashSet<ApplicationUser>();
        }

        public ICollection<Message> Messages { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
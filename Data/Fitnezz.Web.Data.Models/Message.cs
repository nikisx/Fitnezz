using System;
using System.Collections.Generic;
using Fitnezz.Web.Data.Common.Models;

namespace Fitnezz.Web.Data.Models
{

    public class Message: BaseDeletableModel<int>
    {
        public string UserName { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }

        public string ChatId { get; set; }

        public Chat Chat { get; set; }
    }
}
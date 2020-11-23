// ReSharper disable VirtualMemberCallInConstructor
namespace Fitnezz.Web.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Fitnezz.Web.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Workouts = new HashSet<TraineesWorkouts>();
            this.MealPlans = new HashSet<TraineesMealPlans>();
            this.Clients = new HashSet<ApplicationUser>();
            this.TrainersClasses = new HashSet<TrainersClasses>();
        }

        // Audit info
        public ICollection<TrainersClasses> TrainersClasses { get; set; }

        public string CardId { get; set; }

        public Card Card { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Age { get; set; }

        public double Weight { get; set; }

        public string TrainerId { get; set; }

        public ApplicationUser Trainer { get; set; }

        public string Goal { get; set; }

        public double Height { get; set; }

        public string Name { get; set; }

        public string Specialty { get; set; }

        public ICollection<ApplicationUser> Clients { get; set; }

        public string Img { get; set; }

        public ICollection<TraineesWorkouts> Workouts { get; set; }

        public ICollection<TraineesMealPlans> MealPlans { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}

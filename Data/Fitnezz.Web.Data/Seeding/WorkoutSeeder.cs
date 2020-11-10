namespace Fitnezz.Web.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Fitnezz.Web.Data.Models;
    using Microsoft.EntityFrameworkCore.Internal;

    internal class WorkoutSeeder: ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Workouts.Any())
            {
                return;
            }

            await dbContext.Workouts.AddAsync(new Workout() { Name = "Workout1"});
            await dbContext.SaveChangesAsync();
        }
    }
}
using System;
using Hangfire;
using Hangfire.MemoryStorage;
using Stripe;

namespace Fitnezz.Web.Web
{
    using System.Reflection;

    using Fitnezz.Web.Data;
    using Fitnezz.Web.Data.Common;
    using Fitnezz.Web.Data.Common.Repositories;
    using Fitnezz.Web.Data.Models;
    using Fitnezz.Web.Data.Repositories;
    using Fitnezz.Web.Data.Seeding;
    using Fitnezz.Web.Services.Data;
    using Fitnezz.Web.Services.Mapping;
    using Fitnezz.Web.Services.Messaging;
    using Fitnezz.Web.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170).UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer().UseMemoryStorage());

            services.AddHangfireServer();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = this.configuration.GetSection("Facebook")["AppId"];
                options.AppSecret = this.configuration.GetSection("Facebook")["AppSecret"];
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IWorkoutsService, WorkoutsService>();
            services.AddTransient<IMealPlansService, MealPlansService>();
            services.AddTransient<ITrainersService, TrainersService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IClassesService, ClassesService>();
            services.AddTransient<ICardsService, CardsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            recurringJobManager.AddOrUpdate("Delete invalid cards", () => serviceProvider.GetService<ICardsService>().DeleteInvalidCards(), Cron.Daily);
            app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?statusCode={0}");
            StripeConfiguration.SetApiKey(this.configuration.GetSection("Stripe")["SecretKey"]);
            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}

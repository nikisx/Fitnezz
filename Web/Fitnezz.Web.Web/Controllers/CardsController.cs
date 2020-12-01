using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Fitnezz.Web.Web.Controllers
{
    public class CardsController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ICardsService cardsService;

        public CardsController(IUsersService usersService, ICardsService cardsService)
        {
            this.usersService = usersService;
            this.cardsService = cardsService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                return this.NotFound();
            }

            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            if (user.CardId != null)
            {
                return this.Redirect("/");
            }

            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = await customerService.CreateAsync(new CustomerCreateOptions()
            {
                Email = stripeEmail,
                Source = stripeToken,
            });

            var charge = await chargeService.CreateAsync(new ChargeCreateOptions()
            {
                Amount = 6000,
                Description = "Test Description",
                Currency = "usd",
                Customer = customer.Id,
            });

            await this.cardsService.Create(user.Id);

            return this.Redirect("/Users/Profile");
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                return this.NotFound();
            }

            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            if (user.CardId != null)
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Extend(string stripeEmail, string stripeToken)
        {
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                return this.NotFound();
            }

            var user = this.usersService.GetUserByUserName(this.User.Identity.Name);

            if (user.CardId == null)
            {
                return this.NotFound();
            }

            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = await customerService.CreateAsync(new CustomerCreateOptions()
            {
                Email = stripeEmail,
                Source = stripeToken,
            });

            var charge = await chargeService.CreateAsync(new ChargeCreateOptions()
            {
                Amount = 6000,
                Description = "Test Description",
                Currency = "usd",
                Customer = customer.Id,
            });

            await this.cardsService.ExtendUserCard(user.CardId);

            return this.Redirect("/Users/Profile");
        }
    }
}

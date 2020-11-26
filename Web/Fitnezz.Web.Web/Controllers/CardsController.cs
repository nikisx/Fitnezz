using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Fitnezz.Web.Web.Controllers
{
    public class CardsController : Controller
    {
        [HttpPost]
        public IActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = customerService.Create(new CustomerCreateOptions()
            {
                Email = stripeEmail,
                Source = stripeToken,
            });

            var charge = chargeService.Create(new ChargeCreateOptions()
            {
                Amount = 6000,
                Description = "Test Description",
                Currency = "usd",
                Customer = customer.Id,
            });

            return this.Redirect("/");
        }

        public IActionResult Create()
        {
            return this.View();
        }
    }
}

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.BillingPortal;
using TestingSub.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System;

namespace TestingSub.Controllers
{
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly IConfiguration _configuration;

        public IActionResult Index()
        {
            return View();
        }
        public WebhookController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = "sk_test_51HtmObHQbClkzxezsIKa0GNcvyKMngwztyFvlxirWE6QW4YKGADejt1ay8rTZqY4VvBu8a4aSTgYpNMtpLHJVBpC00P6J1uOX3";

        }
        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                                    Request.Headers["Stripe-Signature"], "whsec_D34Y7Rn7bJ5bb6egTY7KjgH3WYMhUjjh");
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
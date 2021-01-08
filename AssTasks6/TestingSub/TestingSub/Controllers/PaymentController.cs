using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.BillingPortal;
using TestingSub.Models;
using System.Net;using System;
using System.Collections;
using System.Collections.Generic;
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

namespace TestingSub.Controllers
{
    public class PaymentController : Controller
    {
        public static string customer_id { get; set; }
        public static string global_plan_id { get; set; }

        private readonly IConfiguration _configuration;
        public IActionResult Index()
        {
            return View();
        }
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = "{STRIPE SECRET KEY}";

        }

        public IActionResult Subscribe()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(string email, string plan, string stripeToken)
        {
            CustomerModel cm = new CustomerModel();
            var customerOptions = new CustomerCreateOptions
            {
                Email = email,
                Source = stripeToken,
            };

            try
            {

                var customerService = new CustomerService();
                var customer = customerService.Create(customerOptions);
                customer_id = customer.Id;
                // Previous code in action

                var planId = "";
                if (plan == "basicPlan")
                {
                    planId = "price_1Htq5ZHQbClkzxezJlLhffk6";
                    global_plan_id = "price_1Htq5ZHQbClkzxezJlLhffk6";
                }
                else
                {
                    planId = "price_1I2w4MHQbClkzxezSyy1Cd1j";
                    global_plan_id = "price_1I2w4MHQbClkzxezSyy1Cd1j";

                }

                var subscriptionOptions = new SubscriptionCreateOptions
                {
                    Customer = customer.Id,
                    Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Plan = planId
            },
        },
                };

                subscriptionOptions.AddExpand("latest_invoice.payment_intent");

                var subscriptionService = new SubscriptionService();
                var subscription = subscriptionService.Create(subscriptionOptions);



                ViewBag.stripeKey = "{PUBLIC KEY};
                ViewBag.subscription = subscription.ToJson();
                cm.customer_id = subscription.CustomerId;
                cm.email = email.ToString();
                cm.subscription_status = subscription.Status;
                if (planId == "price_1Htq5ZHQbClkzxezJlLhffk6")
                {
                    cm.basic = "True";
                    cm.premium = "False";
                }
                else
                {
                    cm.basic = "False";
                    cm.premium = "True";
                }

                cm.SaveDetails();
                return View("SubscribeResult");
            }
            catch(Exception e)
            {
                cm.customer_id = "not-valid";
                cm.email = email.ToString();
                cm.subscription_status = "not-valid";
                cm.basic = null;
                cm.premium = null;
                cm.SaveDetails();
               
                return View("SubscribeFailed");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SubscriptionWebhook()


        [AllowAnonymous]
        [HttpPost]
        [Route("Payment/webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                string signingSecret = "{WEBHOOK KEY}";

                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], signingSecret);
                CustomerModel cm = new CustomerModel();

                switch (stripeEvent.Type)
                {
                    case "charge.succeeded":
                        cm.RecordChargeStatus("succeed", customer_id);
                        break;
                    case "charge.failed":
                        cm.RecordChargeStatus("failed", customer_id);

                        break;

                    case "customer.subscription.deleted":
                        cm = new CustomerModel();
                        cm.PlanRemove(customer_id);

                        break;

                    case "customer.subscription.updated":
                        var subUp = stripeEvent.Data.Object as Subscription;

                        cm = new CustomerModel();
                        //var result = cm.getDetailsWithId("cus_Ihi5ctmRFKeQ6X");
                        //var result = "True";
                        if (global_plan_id == "price_1Htq5ZHQbClkzxezJlLhffk6")
                        {
                            cm.PlanChange("premium", customer_id);
                            global_plan_id = "price_1I2w4MHQbClkzxezSyy1Cd1j";
                        }
                        else
                        {
                            cm.PlanChange("basic", customer_id);
                            global_plan_id = "price_1Htq5ZHQbClkzxezJlLhffk6";

                        }
                        break;


                }

                return Ok();
            }
            catch (StripeException ex)
            {
                //_logger.LogError(ex, $"StripWebhook: {ex.Message}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"StripWebhook: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost("customer-portal")]
        public async Task<IActionResult> CustomerPortal()
        {
            // Authenticate your user.

            var options = new SessionCreateOptions
            {
                Customer = customer_id,
                ReturnUrl = "https://localhost:44367/payment",
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);

        }

    }
}

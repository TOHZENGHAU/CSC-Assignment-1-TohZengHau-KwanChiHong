using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssTask3.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace AssTask3.Controllers
{
    public class HomeController : Controller
    {
        public class CaptchaResponseViewModel
        {
            public bool Success { get; set; }
            [JsonProperty(PropertyName = "error-codes")]
            public IEnumerable<string> ErrorCodes { get; set; }
            [JsonProperty(PropertyName = "challenge_ts")]
            public DateTime ChallengeTime { get; set; }
            public string HostName { get; set; }
            public double Score { get; set; }
            public string Action { get; set; }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> Index(HomeModels model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var isCaptchaValid = await IsCaptchaValid(model.GoogleCaptchaToken);
        //        if (isCaptchaValid)
        //        {
        //            return RedirectToAction("Success");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("GoogleCaptcha", "The captcha is not valid");
        //        }
        //    }
        //    return View();

        //}

        //private async Task<bool> IsCaptchaValid(string response)
        //{
        //    try
        //    {
        //        var secret = "6Ld3iPMZAAAAAOLa2-VctOqbvQmg_YFR64h5bELh";
        //        using (var client = new HttpClient())
        //        {
        //            var values = new Dictionary<string, string>
        //            {
        //                {"secret", secret },
        //                {"response", response },
        //            };

        //            var content = new FormUrlEncodedContent(values);
        //            var verify = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
        //            var captchaResponseJson = await verify.Content.ReadAsStringAsync();
        //            var captchaResult = JsonConvert.DeserializeObject<CaptchaResponseViewModel>(captchaResponseJson);
        //            return captchaResult.Success
        //                && captchaResult.Action == "register_account"
        //                && captchaResult.Score > 0.5;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}



    }
}

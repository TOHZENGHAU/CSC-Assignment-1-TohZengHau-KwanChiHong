using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestingSub.Models
{
    public class CreateCheckoutSessionRequest
    {
        [JsonProperty("priceId")]
        public string PriceId { get; set; }
    }
}

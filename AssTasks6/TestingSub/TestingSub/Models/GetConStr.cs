using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestingSub.Models
{
    public class GetConStr
    {
        public static string ConString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();
            string constring = config.GetConnectionString("UserProfile");
            return constring;
        }
    }
}

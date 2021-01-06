using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;


namespace AssTask1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //    string url = String.Format("https://api.data.gov.sg/v1/environment/24-hour-weather-forecast");
            //    WebRequest requestObject = WebRequest.Create(url);
            //    requestObject.Method = "GET";
            //    HttpWebResponse responseObject = null;
            //    responseObject = (HttpWebResponse)requestObject.GetResponse();

            //    string resultstr = null;
            //    using (Stream stream = responseObject.GetResponseStream())
            //    {
            //        StreamReader sr = new StreamReader(stream);
            //        resultstr = sr.ReadToEnd();
            //        sr.Close();
            //    }
            Label1.Text = Display();

        }

        public string Display()
        {
            string url = String.Format("https://api.data.gov.sg/v1/environment/24-hour-weather-forecast");
            WebRequest requestObject = WebRequest.Create(url);
            requestObject.Method = "GET";
            HttpWebResponse responseObject = null;
            responseObject = (HttpWebResponse)requestObject.GetResponse();


            string resultstr = null;
            using (Stream stream = responseObject.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                resultstr = sr.ReadToEnd();
                sr.Close();
                return resultstr;
            }

        }
    }

}
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DairtelBot
{
    public class JoloService
    {
        public static async Task<JoloJson> GetPlans(string area, string type)
        {
            string uri = $"https://joloapi.com/api/findplan.php?userid=dkooldk&key=284312851138311&opt=28&cir={area}&typ={type}&amt=&max=100&type=json";
            var planDetail = new JoloJson();
            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                planDetail = JsonConvert.DeserializeObject<JoloJson>(rawData);
                

            }

            return planDetail;
        }
       
    }
}

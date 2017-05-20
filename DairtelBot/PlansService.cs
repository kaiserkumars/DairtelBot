using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DairtelBot
{
    public class PlansService
    {
        public static async Task<string>  GetPlans(string recType)
        {
            string uri = "http://private-a8ad69-dairtelbot.apiary-mock.com/getplans";
            string toReturn="AMOUNT | TALKTIME | VALIDITY\n\n";
            var planDetail = new PlansJson();
            int recTypeInt = Convert.ToInt32(recType);
            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                planDetail = JsonConvert.DeserializeObject<PlansJson>(rawData);
                var selectedPlan = JsonConvert.DeserializeObject<PlansJson>(rawData);
                /*
                //using LINQ// //This is not working fix it!!!//
                JObject dir = JObject.Parse(rawData);
                JArray details = (JArray)dir["getplans"][0]["choices"];
                detailsText = details.Select(c => (string)c).ToList(); //something wrog here*/
                
                List<string> details = new List<string>();
                List<string> amount = new List<string>();
                List<string> validity = new List<string>();

                foreach (var x in selectedPlan.getplans[recTypeInt-1].choices)
                {
                    details.Add(x.Detail);
                    amount.Add(x.Amount);
                    validity.Add(x.Validity);
                }

                var res = amount.Zip(details, (x, y) => x + "  |  " + y).Zip(validity, (x, y) => x + "  |  " + y); //joining three lists

                foreach (var x in res)
                {
                    toReturn = toReturn + x + "\n\n";
                }
            

                            
                    

            }

            return toReturn;
        }
       
    }
}

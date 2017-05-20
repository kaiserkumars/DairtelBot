using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DairtelBot
{
   

        public class LuisService
        {
            public static async Task<LuisJson> PareUserInput(string phrase)
            {
            string returnValue = string.Empty;
            string escapedString = Uri.EscapeDataString(phrase);
            using (var client = new HttpClient())
            {
                string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/b843c373-4535-4b3d-95c4-7713656d67ea?subscription-key=9da31e75e0a0467689673fca5f31f7e8&verbose=true&timezoneOffset=0&q={escapedString}";
                var msg = await client.GetAsync(uri);
                if(msg.IsSuccessStatusCode)
                {
                    var jsonresponse = await msg.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<LuisJson>(jsonresponse);
                    return data;
                }
            }

            return null;

            }
        }

       
    
}

using System;

namespace DairtelBot
{
    
    public class PlansJson
    {
        public Getplan[] getplans { get; set; }
    }

    public class Getplan
    {
        public string rechargeType { get; set; }
        public DateTime published_at { get; set; }
        public Choice[] choices { get; set; }
    }

    public class Choice
    {
        public string Detail { get; set; }
        public string Amount { get; set; }
        public string Validity { get; set; }
    }


}

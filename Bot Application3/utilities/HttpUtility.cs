using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Bot_Application3.utilities
{
    public class HttpUtility
    {
            public const string apiKey = "07db8c70ffb04568b68a46f511b014c6";
        public static dynamic getHttpClientForTextAnalytics()
        {
            return new HttpClient
            {
                DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                    {"Accept", "application/json"}
                                                                  }
            };
        } 
    }
}
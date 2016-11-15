using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bot_Application3.Apicallers;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Controllers
{
    public class MessageHandler
    {
        public  string isQuestion(string text)
        {
            if (text.ToUpper().Contains("HOW")
                    || text.ToUpper().Contains("WHAT")
                    || text.ToUpper().Contains("WHY")
                    || text.ToUpper().Contains("WHERE"))
            {
                //var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(text);

                return "QUESTION";
            }
            return "PROBLEM";
        }
    }
}
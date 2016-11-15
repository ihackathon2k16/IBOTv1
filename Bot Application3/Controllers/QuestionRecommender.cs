using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application3.Controllers
{
    public class QuestionRecommender
    {
        public static String quest(dynamic keyPhrases, dynamic intent, dynamic entityType,dynamic Process)
        {
            if (keyPhrases != null)
            {
                if (keyPhrases.contains("RATES") && Process != null)
                {
                    return null;
                }
                else if (keyPhrases.contains("RATES"))
                {
                    return "Which all Pocess did you run?";
                }
                else
                    return null;
            }
            else if (intent == null)
            {
                return "What you exactly want?";
            }
            else
                return null;
        }
    }
}
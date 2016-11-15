using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class EntityDetector
    {
        string DATA_MODEL = "TABLE";
        string API = "API";
        string RUN_BOOK = "PROCESS";
        Dictionary<string, string> m_EntityMap = new Dictionary<string, string>();
        public EntityDetector()
        {
            m_EntityMap.Add("BL1_RC_RATES", DATA_MODEL);
            m_EntityMap.Add("BL1_TAX", DATA_MODEL);
            m_EntityMap.Add("BL1_CHARGE", DATA_MODEL);
            m_EntityMap.Add("BL1_CHARGE_REQUEST", DATA_MODEL);
            m_EntityMap.Add("BL1_CUSTOMER", DATA_MODEL);
            m_EntityMap.Add("BILL PREPRATION","FLOW" );
            m_EntityMap.Add("BLPREP", RUN_BOOK);
            m_EntityMap.Add("BLPOPX", RUN_BOOK);
            m_EntityMap.Add("CHGCRE", RUN_BOOK);
            m_EntityMap.Add("TXAINV", RUN_BOOK);
            m_EntityMap.Add("BTLSOR", RUN_BOOK);
            m_EntityMap.Add("BTLQUOTE", RUN_BOOK);
            m_EntityMap.Add("GETESTIMATEDBILL", API);
            m_EntityMap.Add("GETQUOTE", API);
            m_EntityMap.Add("GETPRORATEDQUOTE", API);
           

        }
  
        public string identifyType(String i_key)
        {
            string retVal;
            string key = i_key.ToUpper();
            retVal = m_EntityMap[key];
         //   Console.WriteLine("Key =" + i_key + "Value=" + retVal);
            return retVal;

        }
    }
}
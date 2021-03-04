using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Log
    {
        public static void DoLog(COM.Action act, string UIDorTELL, int Result, string Exception)
        {
            try
            {
                COM.Log mLog = new COM.Log()
                {
                    ActionType = (int)act,
                    Time = DateTime.UtcNow,
                    UIDorTELLNUMBER = UIDorTELL,
                    Result = Result,
                    Exception = Exception
                };
                using (HasmaEntities ent = new HasmaEntities())
                {
                    ent.Logs.Add(mLog);
                    ent.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error On Log: " + e.Message);
            }
        }
    }
}

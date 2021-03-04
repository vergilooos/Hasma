using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Bon
    {
        public static int AddBon(COM.Bon bon)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Bons.Add(bon);
                    ent.SaveChanges();

                    return bon.BID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddBonSerial, bon.SerialNumber.ToString(), -100, e.Message);
                return -100;
            }
        }

        public static COM.Bon GetBonBySerial(long SerialNumber)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Bons.Where(z => z.SerialNumber == SerialNumber).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetBonBySerial, SerialNumber.ToString(), -100, e.Message);
                return null;
            }
        }

        public static COM.UserBon GetUserBonByID(int BID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.UserBons.Where(z => z.BID == BID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetUserBonByID, BID.ToString(), -100, e.Message);
                return null;
            }
        }

        public static int AddUserBon(int UID, int BID)
        {
            try
            {
                COM.UserBon userBon = new COM.UserBon() { BID = BID, UID = UID };
                using (var ent = DB.Entity)
                {
                    ent.UserBons.Add(userBon);
                    ent.SaveChanges();

                    return userBon.UBID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddUserBon, UID.ToString(), -100, e.Message);
                return -100;
            }
        }
    }
}

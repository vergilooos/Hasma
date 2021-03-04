using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Payment
    {

        public static int AddPay(COM.Pay mPay)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Pays.Add(mPay);
                    ent.SaveChanges();
                    return mPay.PayID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddPay, mPay.BuyID, -100, e.Message);
                return -100;
            }
        }
        public static COM.Pay GetPayByID(int PayID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Pays.Where(z => z.PayID == PayID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetPayByID, PayID.ToString(), -100, e.Message);
                return null;
            }
        }
        public static COM.Pay GetPayByBuyID(string BuyID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Pays.Where(z => z.BuyID == BuyID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetPayByBuyID, BuyID, -100, e.Message);
                return null;
            }
        }

        public static bool UpdatePayAfterDargah(COM.Pay mPay)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Pays.Attach(mPay);
                    var Entry = ent.Entry(mPay);
                    Entry.Property(ex => ex.DargahState).IsModified = true;
                    Entry.Property(ex => ex.Token).IsModified = true;
                    Entry.Property(ex => ex.TrackingNumber).IsModified = true;
                    Entry.Property(ex => ex.ReferenceNumber).IsModified = true;
                    Entry.Property(ex => ex.EndMoment).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateProduct, mPay.BuyID, -100, e.Message);
                return false;
            }
        }
        public static bool UpdatePayAfterConfirm(COM.Pay mPay)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Pays.Attach(mPay);
                    var Entry = ent.Entry(mPay);
                    Entry.Property(ex => ex.IsReverse).IsModified = true;
                    Entry.Property(ex => ex.FinalState).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdatePayAfterConfirm, mPay.BuyID, -100, e.Message);
                return false;
            }
        }
    }
}

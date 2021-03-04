using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Salement
    {
        public static int AddOrder(COM.Order mOrder)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Orders.Add(mOrder);
                    ent.SaveChanges();
                    return mOrder.OID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddOrder, "AddOrder", -100, e.Message);
                return -100;
            }
        }

        public static List<COM.Order> GetAllOrders()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Orders.ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetAllOrders, "GetAll", -100, e.Message);
                return null;
            }
        }

        public static List<COM.Order> GetOrdersByUID(int UID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Orders.Where(W=> W.UID == UID).ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetOrdersByUID, UID.ToString(), -100, e.Message);
                return null;
            }
        }
        public static COM.Order GetOrdersByOID(int OID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Orders.Where(W => W.OID == OID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetOrdersByOID, OID.ToString(), -100, e.Message);
                return null;
            }
        }
        public static bool UpdateOrderReBuy(COM.Order mOrder)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Orders.Attach(mOrder);
                    var Entry = ent.Entry(mOrder);
                    Entry.Property(ex => ex.BuyID).IsModified = true;
                    Entry.Property(ex => ex.GhestNumber).IsModified = true;
                    Entry.Property(ex => ex.GhestValue).IsModified = true;
                    Entry.Property(ex => ex.PishPardakht).IsModified = true;
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateOrderReBuy, mOrder.OID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateOrderPayStatus(COM.Order mOrder)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Orders.Attach(mOrder);
                    var Entry = ent.Entry(mOrder);
                    Entry.Property(ex => ex.PayStatus).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateOrderPayStatus, mOrder.OID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateOrderDeliverStatus(COM.Order mOrder)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Orders.Attach(mOrder);
                    var Entry = ent.Entry(mOrder);
                    Entry.Property(ex => ex.PayStatus).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateOrderDeliverStatus, mOrder.OID.ToString(), -100, e.Message);
                return false;
            }
        }

    }
}

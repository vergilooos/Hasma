using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public class Product
    {
        public static COM.Ghest GetGhestInfo()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Ghests.Where( GH => GH.ID == 1).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool RemoveProductByID(int PID)
        {
            try
            {
                using (var ent = DB.Entity)
                {

                    COM.Product mProduct = new COM.Product()
                    {
                        PID = PID
                    };
                    ent.Products.Count();

                    ent.Products.Attach(mProduct);
                    ent.Products.Remove(mProduct);
                    ent.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int GetCountOfProduct()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Products.Count();
                }
            }
            catch
            {
                return 0;
            }
        }

        public static List<COM.Category> GetCategories()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Categories.ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetCategories, "GetALL", -100, e.Message);
                return null;
            }
        }
        public static List<COM.Category> GetCategoriesByGroupName(string GroupName)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Categories.Where(G => G.Group == GroupName).ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetCategoriesByGroupName, "GroupName", -100, e.Message);
                return null;
            }
        }
        public static int AddProduct(COM.Product mProduct)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Products.Add(mProduct);
                    ent.SaveChanges();
                    return mProduct.PID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddProduct, "NoName", -100, e.Message);
                return -100;
            }
        }

        public static COM.Product GetProductByID(int PID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Products.Where(z => z.PID == PID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetProductByID, PID.ToString(), -100, e.Message);
                return null;
            }
        }


        public static List<COM.Product> GetALLProduct()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Products.ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetALLProduct, "GetALL", -100, e.Message);
                return null;
            }
        }
        public static List<COM.Product> GetALLProductByCatID(int CatID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Products.Where(W => W.CatID == CatID).ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetALLProductByCatID, "GetALL", -100, e.Message);
                return null;
            }
        }


        public static bool UpdateProduct(COM.Product mProduct)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Products.Attach(mProduct);
                    var Entry = ent.Entry(mProduct);
                    Entry.Property(ex => ex.Description).IsModified = true;
                    Entry.Property(ex => ex.Price).IsModified = true;
                    Entry.Property(ex => ex.PriceOff).IsModified = true;
                    Entry.Property(ex => ex.StockCount).IsModified = true;
                    Entry.Property(ex => ex.specification).IsModified = true;
                    Entry.Property(ex => ex.Name).IsModified = true;
                    Entry.Property(ex => ex.CatID).IsModified = true;
                    Entry.Property(ex => ex.SubCatID).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateProduct, mProduct.PID.ToString(), -100, e.Message);
                return false;
            }
        }

        public static List<COM.MiniProcuct> GetMiniProcuctsByCatIDs(int CatID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Products
                        .Where(W => W.CatID == CatID)
                        .Select(P => new COM.MiniProcuct
                        {
                            Name = P.Name,
                            PID = P.PID,
                            Price = P.Price
                        })
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetMiniProcuctsByCatIDs, CatID.ToString(), -100, e.Message);

                return null;
            }
        }

        public static List<COM.MiniProcuct> GetMiniProcuctsBySubCatIDs(int CatID, int SubCatID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Products
                        .Where(W => W.CatID == CatID && W.SubCatID == SubCatID)
                        .Select(P => new COM.MiniProcuct
                        {
                            Name = P.Name,
                            PID = P.PID,
                            Price = P.Price
                        })
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetMiniProcuctsByCatIDs, CatID.ToString(), -100, e.Message);

                return null;
            }
        }
        //public static List<MiniStory> GetMiniStoryByIDs(List<int> SIDs)
        //{
        //    try
        //    {
        //        using (GoftemanEntities ent = new GoftemanEntities())
        //        {
        //            return ent.Stories
        //                .Where(t => SIDs.Contains(t.SID))
        //                .Select(b => new
        //                {
        //                    b.Author,
        //                    b.CounterView,
        //                    CategoryID = b.Category,
        //                    b.EpisodNumber,
        //                    b.Description,
        //                    b.Img,
        //                    b.Name,
        //                    b.Price,
        //                    b.SID,
        //                    b.GSID,
        //                    b.Type,
        //                    b.WID,
        //                    b.BaseAddress
        //                }).AsEnumerable().Select(c => new MiniStory
        //                {
        //                    Author = c.Author,
        //                    CategoryID = c.CategoryID,
        //                    CategoryName = GetCategoryName(c.CategoryID),
        //                    CounterView = FormatNumber(c.CounterView),
        //                    Description = c.Description,
        //                    EpisodNumber = c.EpisodNumber,
        //                    Img = c.Img,
        //                    Name = c.Name,
        //                    Price = c.Price,
        //                    SID = c.SID,
        //                    GSID = c.GSID,
        //                    Type = c.Type,
        //                    GroupName = GetGroupName(c.GSID),
        //                    WID = c.WID,
        //                    BaseAddress = c.BaseAddress
        //                })
        //                .ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        new System.Threading.Thread(delegate () { Common.DoLog(Common.Action.GetListStory, "User", 0, e.Message); }).Start();
        //        return null;
        //    }
        //}
    }
}

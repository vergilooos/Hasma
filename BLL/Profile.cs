using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Profile
    {
        public static int GetCountOfUser()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Users.Count();
                }
            }
            catch
            {
                return 0;
            }
        }
        public static COM.LoginByActiveCode_Result GetLoginByCode(string Tell, String Code)
        {
            using (var ent = DB.Entity)
            {
                COM.LoginByActiveCode_Result Res = ent.LoginByActiveCode(Tell, Code).SingleOrDefault();
                return Res;
            }
        }



        public static int AddUserMessage(COM.UserMessage mUserMes)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserMessages.Add(mUserMes);
                    ent.SaveChanges();

                    return mUserMes.UMID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddUser, mUserMes.UID.ToString(), -100, e.Message);
                return -100;
            }
        }

        public static bool AddUserMessageToAll(COM.Msg msg)
        {
            try
            {
                using (var ent = DB.Entity)
                {

                    foreach (var usr in ent.Users)
                    {
                        COM.UserMessage mUserMes = new COM.UserMessage()
                        {
                            UID = usr.UID,
                            seen = false,
                            Title = msg.Title,
                            Context = msg.Content,
                        };
                        ent.UserMessages.Add(mUserMes);
                    }
                    ent.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddUserMessageToAll, msg.Title, -100, e.Message);
                Log.DoLog(COM.Action.AddUserMessageToAll, msg.Title, -101, e.InnerException.Message);
                return false;
            }
        }


        public static List<COM.UserMessage> GetUserMessagebyUID(int UID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.UserMessages.Where(z => z.UID == UID).ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetUserMessagebyUID, UID.ToString(), -100, e.Message);
                return null;
            }
        }

      

        public static bool UpdateUserMessageSeeableBYUMID(int UMID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    COM.UserMessage userMessage = new COM.UserMessage() { seen = true, UMID = UMID };
                    ent.UserMessages.Attach(userMessage);
                    var Entry = ent.Entry(userMessage);
                    Entry.Property(ex => ex.seen).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserMessageSeeableBYUMID, UMID.ToString(), -100, e.Message);
                return false;
            }
        }





        public static int AddUser(COM.User mUser)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Users.Add(mUser);
                    ent.SaveChanges();

                    COM.UserDocument userDocument = new COM.UserDocument() { UID = mUser.UID };
                    AddUserDocument(userDocument);

                    COM.UserMessage mUserMes = new COM.UserMessage() { UID = mUser.UID, seen = false, Context = "به هسما خوش آمدید، ما تو هسما قصد داریم که زندگی رو برای همه کارمندها آسونتر کنیم. شما میتونید با نظراتتون ما رو همراهی کنید، هر مشکلی بود با تماس بگیرید.", Title = "خوش آمدید" };
                    AddUserMessage(mUserMes);

                    return mUser.UID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddUser, mUser.TellNumber, -100, e.Message);
                return -100;
            }
        }

        public static int AddUserDocument(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Add(mUserDocument);
                    ent.SaveChanges();

                    return mUserDocument.UID;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.AddUserDocument, mUserDocument.UID.ToString(), -100, e.Message);
                return -100;
            }
        }

        public static COM.User GetUserByTellNumber(string TellNumber)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Users.Where(z => z.TellNumber == TellNumber).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetUserByTellNumber, TellNumber, -100, e.Message);
                return null;
            }
        }

        public static COM.User GetUserByCodeMeli(string CodeMeli)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Users.Where(z => z.CodeMeli == CodeMeli).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetUserByCodeMeli, CodeMeli, -100, e.Message);
                return null;
            }
        }
        public static COM.User GetUserByID(int UID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Users.Where(z => z.UID == UID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetUserByID, UID.ToString(), -100, e.Message);
                return null;
            }
        }

        public static List<COM.MiniUser> GetAllUserByID(List<int> UIDs)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.Users.Where(x => UIDs.Contains(x.UID))
                        .Select(Y => new COM.MiniUser()
                        {
                            UID = Y.UID,
                            CodeMeli = Y.CodeMeli,
                            Name = Y.Name,
                            TellNumber = Y.TellNumber,
                            JoinTime = Y.JoinDate,
                        })
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetUserByID, "UIDs", -100, e.Message);
                return null;
            }
        }


        public static COM.UserDocument GetUserDocumentByID(int UID)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.UserDocuments.Where(z => z.UID == UID).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UserDocument, UID.ToString(), -100, e.Message);
                return null;
            }
        }

        public static List<int> GetAllUserDocumentUploadedID()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.UserDocuments.Where(z => z.FishHoghoogh == 1 || z.JavazKasb == 1 || z.KasrHoghoogh == 1 || z.KartMeli == 1 || z.CheckZemanat == 1).Select(A => A.UID).ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetAllUserDocumentUploadedID, "", -100, e.Message);
                return null;
            }
        }
        public static List<COM.UserDocument> GetAllUserDocumentUploadedI()
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    return ent.UserDocuments.Where(z => z.FishHoghoogh == 1 || z.JavazKasb == 1 || z.KasrHoghoogh == 1 || z.KartMeli == 1).ToList();
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.GetAllUserDocumentUploaded, "", -100, e.Message);
                return null;
            }
        }

        public static bool UpdateUserCheckZemanat(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Attach(mUserDocument);
                    var Entry = ent.Entry(mUserDocument);
                    Entry.Property(ex => ex.CheckZemanat).IsModified = true;
                    Entry.Property(ex => ex.LastUpTime).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserCheckZemanat, mUserDocument.UID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateUserJavazKasb(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Attach(mUserDocument);
                    var Entry = ent.Entry(mUserDocument);
                    Entry.Property(ex => ex.JavazKasb).IsModified = true;
                    Entry.Property(ex => ex.LastUpTime).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserJavazKasb, mUserDocument.UID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateUserUpKart(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Attach(mUserDocument);
                    var Entry = ent.Entry(mUserDocument);
                    Entry.Property(ex => ex.KartMeli).IsModified = true;
                    Entry.Property(ex => ex.LastUpTime).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserUpKart, mUserDocument.UID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateUserUpKasr(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Attach(mUserDocument);
                    var Entry = ent.Entry(mUserDocument);
                    Entry.Property(ex => ex.KasrHoghoogh).IsModified = true;
                    Entry.Property(ex => ex.LastUpTime).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserUpKasr, mUserDocument.UID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateUserFishHoghoogh(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Attach(mUserDocument);
                    var Entry = ent.Entry(mUserDocument);
                    Entry.Property(ex => ex.FishHoghoogh).IsModified = true;
                    Entry.Property(ex => ex.LastUpTime).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserFishHoghoogh, mUserDocument.UID.ToString(), -100, e.Message);
                return false;
            }
        }
        public static bool UpdateUserDocumentStatus(COM.UserDocument mUserDocument)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.UserDocuments.Attach(mUserDocument);
                    var Entry = ent.Entry(mUserDocument);
                    Entry.Property(ex => ex.CheckZemanat).IsModified = true;
                    Entry.Property(ex => ex.FishHoghoogh).IsModified = true;
                    Entry.Property(ex => ex.JavazKasb).IsModified = true;
                    Entry.Property(ex => ex.KartMeli).IsModified = true;
                    Entry.Property(ex => ex.KasrHoghoogh).IsModified = true;
                    Entry.Property(ex => ex.VerifiedTime).IsModified = true;
                    Entry.Property(ex => ex.Verified).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserFishHoghoogh, mUserDocument.UID.ToString(), -100, e.Message);
                return false;
            }
        }
        //public static bool UpdateUserUp(COM.UserDocument mUserDocument)
        //{
        //    try
        //    {
        //        using (var ent = DB.Entity)
        //        {
        //            ent.UserDocuments.Attach(mUserDocument);
        //            var Entry = ent.Entry(mUserDocument);
        //            Entry.Property(ex => ex.VerifiedKart).IsModified = true;
        //            ent.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log.DoLog(COM.Action.UpdateUserUpKasr, mUserDocument.UID.ToString(), -100, e.Message);
        //        return false;
        //    }
        //}
        //public static bool UpdateUserUp(COM.UserDocument mUserDocument)
        //{
        //    try
        //    {
        //        using (var ent = DB.Entity)
        //        {
        //            ent.UserDocuments.Attach(mUserDocument);
        //            var Entry = ent.Entry(mUserDocument);
        //            Entry.Property(ex => ex.VerifiedFish).IsModified = true;
        //            Entry.Property(ex => ex.VerifiedJavaz).IsModified = true;
        //            Entry.Property(ex => ex.VerifiedKart).IsModified = true;
        //            Entry.Property(ex => ex.VerifiedKasr).IsModified = true;
        //            ent.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log.DoLog(COM.Action.UpdateUserUpKasr, mUserDocument.UID.ToString(), -100, e.Message);
        //        return false;
        //    }
        //}
        public static bool UpdateUserActivation(COM.User mUser)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Users.Attach(mUser);
                    var Entry = ent.Entry(mUser);
                    Entry.Property(ex => ex.Active).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserActivation, mUser.UID.ToString(), -100, e.Message);
                return false;
            }
        }

        public static bool UpdateUserProfile(COM.User mUser)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Users.Attach(mUser);
                    var Entry = ent.Entry(mUser);
                    Entry.Property(ex => ex.CodeMeli).IsModified = true;
                    Entry.Property(ex => ex.Name).IsModified = true;
                    Entry.Property(ex => ex.Address).IsModified = true;
                    Entry.Property(ex => ex.City).IsModified = true;
                    Entry.Property(ex => ex.CodePosti).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUser, mUser.UID.ToString(), -100, e.Message);
                Log.DoLog(COM.Action.UpdateUser, mUser.UID.ToString(), -100, e.InnerException.Message);
                return false;
            }
        }


        public static bool UpdateUserActiveCode(COM.User mUser)
        {
            try
            {
                using (var ent = DB.Entity)
                {
                    ent.Users.Attach(mUser);
                    var Entry = ent.Entry(mUser);
                    Entry.Property(ex => ex.ActiveCode).IsModified = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.DoLog(COM.Action.UpdateUserActiveCode, mUser.UID.ToString(), -100, e.Message);
                return false;
            }
        }
    }
}

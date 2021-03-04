using COM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace HasmaWebApp.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        //valid user 
        //send Msg to all user
        //send Msg to specfic user
        //send notif to all User for app

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> SendMsgToAllUser()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Json(JsonConvert.SerializeObject("Exception dade: UnsupportedMediaType"));
                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                Msg msg = new Msg();

                foreach (var itemContent in filesReadToProvider.Contents)
                {
                    if (itemContent.Headers.ContentDisposition.Name == "Object" || itemContent.Headers.ContentDisposition.Name == "\"Object\"")
                    {
                        var jsonString = await itemContent.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        msg = serializer.Deserialize<Msg>(jsonString);

                        bool ResAdd = BLL.Profile.AddUserMessageToAll(msg);
                        if (!ResAdd)
                        {
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe "));
                        }

                        return Ok(JsonConvert.SerializeObject("ferestade shod "));
                    }
                }
                return Json(JsonConvert.SerializeObject("az halghe biroon oomad toosh hich chizi naboode"));

            }
            catch (Exception ee)
            {

                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.SendMsgToAllUser, "SendToAll", -100, ee.Message); }).Start();
                return Json(JsonConvert.SerializeObject("Exception dade: " + ee.Message));

            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> SendMsgToOneUser()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Json(JsonConvert.SerializeObject("Exception dade: UnsupportedMediaType"));
                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                Msg msg = new Msg();

                foreach (var itemContent in filesReadToProvider.Contents)
                {
                    if (itemContent.Headers.ContentDisposition.Name == "Object" || itemContent.Headers.ContentDisposition.Name == "\"Object\"")
                    {
                        var jsonString = await itemContent.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        msg = serializer.Deserialize<Msg>(jsonString);

                        COM.UserMessage mUserMes = new UserMessage()
                        {
                            Context = msg.Content,
                            seen = false,
                            Title = msg.Title,
                            UID = msg.UID,
                        };

                        int ResAdd = BLL.Profile.AddUserMessage(mUserMes);
                        if (ResAdd < 0)
                        {
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe "));
                        }

                        return Ok(JsonConvert.SerializeObject("ferestade shod "));
                    }
                }
                return Json(JsonConvert.SerializeObject("az halghe biroon oomad toosh hich chizi naboode"));

            }
            catch (Exception ee)
            {

                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.SendMsgToOneUser, "SendToOne", -100, ee.Message); }).Start();
                return Json(JsonConvert.SerializeObject("Exception dade: " + ee.Message));

            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public LastStaticInfoResult GetLastStaticInfo(int UID)
        {
            return new LastStaticInfoResult() { GhestpardakhtNashodeNumber = 232, GhestpardakhtShodeNumber = 121, ProductCount = BLL.Product.GetCountOfProduct(), UserCount = BLL.Profile.GetCountOfUser(), ProductInStockCount = 0, TotalIncome = 12300000 };
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public LastUserResult GetLastUserInfo(int UID)
        {
            try
            {
                LastUserResult lastUserResult = new LastUserResult()
                {
                    GhestNum = 0,
                    MessageNum = 1,
                    ProfileNum = 0,
                    ShopListNum = 0
                };
                return lastUserResult;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<MiniUser> GetAllMiniUsersInProgress(int UID)
        {
            var AllUserProg = BLL.Profile.GetAllUserDocumentUploadedID();
            return BLL.Profile.GetAllUserByID(AllUserProg);
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<Order> GetUserProductHistories(int UID)
        {
            return BLL.Salement.GetOrdersByUID(UID);
        }




        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public COM.User GetUserInfo(int UID)
        {
            return BLL.Profile.GetUserByID(UID);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<COM.UserMessage> GetUserMessage(int UID)
        {
            return BLL.Profile.GetUserMessagebyUID(UID);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public bool UpdateUserMessageSeen(int UMID)
        {
            return BLL.Profile.UpdateUserMessageSeeableBYUMID(UMID);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public LogonResult Logon(string Tell)
        {
            LogonResult mLogonResult = new LogonResult();
            try
            {
                User mUser = BLL.Profile.GetUserByTellNumber(Tell);
                if (mUser == null)
                {
                    mUser = new User()
                    {
                        Active = false,
                        ActiveCode = CreateRandomCode(),
                        CodeMeli = "",
                        JoinDate = DateTime.Now,
                        Name = "",
                        TellNumber = Tell,
                        Role = 0,
                    };

                    int UID = BLL.Profile.AddUser(mUser);

                    mLogonResult.IsNew = true;
                    new System.Threading.Thread(delegate () { SendActivationCodeViaSMS(Tell, mUser.ActiveCode); }).Start();
                    new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.Logon, Tell, 1, null); }).Start();
                    mLogonResult.UserID = mUser.UID;
                }
                else
                {
                    string NewActiveCode = CreateRandomCode();
                    if (Tell == "989123456789")//mehr
                    {
                        NewActiveCode = "12345";
                    }
                    mUser.ActiveCode = NewActiveCode;
                    bool Res = BLL.Profile.UpdateUserActiveCode(mUser);
                    mLogonResult.IsNew = false;
                    mLogonResult.NickName = mUser.Name;

                    new System.Threading.Thread(delegate () { SendActivationCodeViaSMS(Tell, NewActiveCode); }).Start();
                    new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.Logon, Tell, 1, null); }).Start();
                    mLogonResult.UserID = mUser.UID;
                }
            }
            catch (Exception e)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.Logon, Tell, 0, e.Message); }).Start();
                mLogonResult.UserID = -200;
                mLogonResult.IsNew = false;
            }
            return mLogonResult;
        }

        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public int AddBon(long SerialNumber)
        {
            COM.Bon bon = new Bon() { SerialNumber = SerialNumber };
            return BLL.Bon.AddBon(bon);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> UpdateUserInformationByAdmin()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Json(JsonConvert.SerializeObject("Exception dade: UnsupportedMediaType"));
                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                HectoUser hectoUser = new HectoUser();

                foreach (var itemContent in filesReadToProvider.Contents)
                {
                    if (itemContent.Headers.ContentDisposition.Name == "Object" || itemContent.Headers.ContentDisposition.Name == "\"Object\"")
                    {
                        var jsonString = await itemContent.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        hectoUser = serializer.Deserialize<HectoUser>(jsonString);

                        COM.User user = new User()
                        {
                            Address = hectoUser.Address,
                            City = hectoUser.City,
                            CodeMeli = hectoUser.CodeMeli,
                            CodePosti = hectoUser.CodePosti,
                            Name = hectoUser.Name,
                            UID = hectoUser.UID,
                        };

                        bool ResAdd = BLL.Profile.UpdateUserProfile(user);
                        if (!ResAdd)
                        {
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe USR"));
                        }

                        COM.UserDocument userDocument = new UserDocument()
                        {
                            CheckZemanat = hectoUser.CheckZemanatStatus,
                            FishHoghoogh = hectoUser.FishHoghooghStatus,
                            JavazKasb = hectoUser.JavazKasbStatus,
                            KartMeli = hectoUser.KartMeliStatus,
                            KasrHoghoogh = hectoUser.KasrHoghooghStatus,
                            UID = hectoUser.UID,
                            VerifiedTime = DateTime.Now,
                            Verified = hectoUser.VerifiedStatus
                        };

                        bool ResUpDoc = BLL.Profile.UpdateUserDocumentStatus(userDocument);
                        if (!ResUpDoc)
                        {
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe DOC"));
                        }


                        return Ok(ResAdd.ToString());
                    }
                }
                return Json(JsonConvert.SerializeObject("az halghe biroon oomad toosh hich chizi naboode"));
            }
            catch (Exception ee)
            {
                BLL.Log.DoLog(COM.Action.UpdateUserInformationByAdmin, "UpdPro", -100, ee.Message);
                return Json(JsonConvert.SerializeObject("Exception dade: " + ee.Message));
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> UpdateUserProfile()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Json(JsonConvert.SerializeObject("Exception dade: UnsupportedMediaType"));
                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                UserInfo mUserInfo = new UserInfo();

                foreach (var itemContent in filesReadToProvider.Contents)
                {
                    if (itemContent.Headers.ContentDisposition.Name == "Object" || itemContent.Headers.ContentDisposition.Name == "\"Object\"")
                    {
                        var jsonString = await itemContent.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        mUserInfo = serializer.Deserialize<UserInfo>(jsonString);

                        COM.User user = new User()
                        {
                            Address = mUserInfo.Address,
                            City = mUserInfo.City,
                            CodeMeli = mUserInfo.CodeMeli,
                            CodePosti = mUserInfo.CodePosti,
                            Name = mUserInfo.Name,
                            UID = mUserInfo.UID,
                            TellNumber = mUserInfo.TellNumber
                        };

                        bool ResAdd = BLL.Profile.UpdateUserProfile(user);
                        if (!ResAdd)
                        {
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe USR"));
                        }
                        BLL.Log.DoLog(COM.Action.UpdateUserProfile, "UpdPro", 0, mUserInfo.Address + mUserInfo.TellNumber + mUserInfo.Name);
                        return Ok(ResAdd.ToString());
                    }
                }
                return Json(JsonConvert.SerializeObject("az halghe biroon oomad toosh hich chizi naboode"));
            }
            catch (Exception ee)
            {
                BLL.Log.DoLog(COM.Action.UpdateUserProfile, "UpdPro", -100, ee.Message);
                return Json(JsonConvert.SerializeObject("Exception dade: " + ee.Message));
            }
        }


        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[AllowAnonymous]
        //[AcceptVerbs("Post")]
        //public bool UpdateUserProfile([FromBody] UserInfo userInfo)
        //{
        //    COM.User user = new User()
        //    {
        //        Address = userInfo.Address,
        //        City = userInfo.City,
        //        CodeMeli = userInfo.CodeMeli,
        //        CodePosti = userInfo.CodePosti,
        //        Name = userInfo.Name,
        //        UID = userInfo.UID,
        //    };

        //    if (userInfo.Bon != null)
        //    {
        //        //check bon 
        //        // add bon
        //    }

        //    return BLL.Profile.UpdateUserProfile(user);
        //}

        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[AllowAnonymous]
        //[AcceptVerbs("Get")]
        //public List<MiniUser> GetAllInProgressUser()
        //{
        //    var allUserDoc = BLL.Profile.GetAllUserDocumentUploaded();
        //    foreach (var usrDoc in allUserDoc)
        //    {
        //        BLL.Profile.GetUserByID(usrDoc.UID);
        //        if (true)
        //        {

        //        }
        //    }
        //}

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public kiloUser GetUserByID(int UID)
        {
            kiloUser kiloUser = new kiloUser() { };
            try
            {
                var usr = BLL.Profile.GetUserByID(UID);
                if (usr == null)
                {
                    kiloUser.HasError = true;
                    kiloUser.DescError = "user peyda nashod.";
                    return kiloUser;
                }
                var usrDoc = BLL.Profile.GetUserDocumentByID(usr.UID);
                if (usrDoc == null)
                {
                    kiloUser.HasError = true;
                    kiloUser.DescError = "با مهرشاذ تماس بگیرید";
                    return kiloUser;
                }
                else
                {
                    kiloUser.UID = usr.UID;
                    kiloUser.HasError = false;
                    kiloUser.DescError = "";
                    kiloUser.Active = usr.Active;
                    kiloUser.Address = usr.Address;
                    kiloUser.City = usr.City;
                    kiloUser.CodeMeli = usr.CodeMeli;
                    kiloUser.CodePosti = usr.CodePosti;
                    kiloUser.FishHoghooghStatus = usrDoc.FishHoghoogh;
                    kiloUser.JavazKasbStatus = usrDoc.JavazKasb;
                    kiloUser.KartMeliStatus = usrDoc.KartMeli;
                    kiloUser.KasrHoghooghStatus = usrDoc.KasrHoghoogh;
                    kiloUser.CheckZemanatStatus = usrDoc.CheckZemanat;
                    kiloUser.Name = usr.Name;
                    kiloUser.TellNumber = usr.TellNumber;
                    kiloUser.UploadTime = usrDoc.LastUpTime;
                    kiloUser.VerifiedTime = usrDoc.VerifiedTime;
                    kiloUser.VerifiedStatus = usrDoc.Verified;
                    return kiloUser;
                }

            }
            catch (Exception e)
            {
                kiloUser.HasError = true;
                kiloUser.DescError = e.Message;
                return kiloUser;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public kiloUser GetUserByTell(string Tell)
        {
            kiloUser kiloUser = new kiloUser() { };
            try
            {
                var usr = BLL.Profile.GetUserByTellNumber(Tell);
                if (usr == null)
                {
                    kiloUser.HasError = true;
                    kiloUser.DescError = "user peyda nashod.";
                    return kiloUser;
                }
                var usrDoc = BLL.Profile.GetUserDocumentByID(usr.UID);
                if (usrDoc == null)
                {
                    kiloUser.HasError = true;
                    kiloUser.DescError = "با مهرشاذ تماس بگیرید";
                    return kiloUser;
                }
                else
                {
                    kiloUser.UID = usr.UID;
                    kiloUser.HasError = false;
                    kiloUser.DescError = "";
                    kiloUser.Active = usr.Active;
                    kiloUser.Address = usr.Address;
                    kiloUser.City = usr.City;
                    kiloUser.CodeMeli = usr.CodeMeli;
                    kiloUser.CodePosti = usr.CodePosti;
                    kiloUser.FishHoghooghStatus = usrDoc.FishHoghoogh;
                    kiloUser.JavazKasbStatus = usrDoc.JavazKasb;
                    kiloUser.KartMeliStatus = usrDoc.KartMeli;
                    kiloUser.KasrHoghooghStatus = usrDoc.KasrHoghoogh;
                    kiloUser.CheckZemanatStatus = usrDoc.CheckZemanat;
                    kiloUser.Name = usr.Name;
                    kiloUser.TellNumber = usr.TellNumber;
                    kiloUser.UploadTime = usrDoc.LastUpTime;
                    kiloUser.VerifiedTime = usrDoc.VerifiedTime;
                    kiloUser.VerifiedStatus = usrDoc.Verified;
                    return kiloUser;
                }

            }
            catch (Exception e)
            {
                kiloUser.HasError = true;
                kiloUser.DescError = e.Message;
                return kiloUser;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public kiloUser GetUserByCodeMeli(string CodeMeli)
        {
            kiloUser kiloUser = new kiloUser() { };
            try
            {
                var usr = BLL.Profile.GetUserByCodeMeli(CodeMeli);
                if (usr == null)
                {
                    kiloUser.HasError = true;
                    kiloUser.DescError = "user peyda nashod.";
                    return kiloUser;
                }
                var usrDoc = BLL.Profile.GetUserDocumentByID(usr.UID);
                if (usrDoc == null)
                {
                    kiloUser.HasError = true;
                    kiloUser.DescError = "با مهرشاذ تماس بگیرید";
                    return kiloUser;
                }
                else
                {
                    kiloUser.UID = usr.UID;
                    kiloUser.HasError = false;
                    kiloUser.DescError = "";
                    kiloUser.Active = usr.Active;
                    kiloUser.Address = usr.Address;
                    kiloUser.City = usr.City;
                    kiloUser.CodeMeli = usr.CodeMeli;
                    kiloUser.CodePosti = usr.CodePosti;
                    kiloUser.FishHoghooghStatus = usrDoc.FishHoghoogh;
                    kiloUser.JavazKasbStatus = usrDoc.JavazKasb;
                    kiloUser.KartMeliStatus = usrDoc.KartMeli;
                    kiloUser.KasrHoghooghStatus = usrDoc.KasrHoghoogh;
                    kiloUser.CheckZemanatStatus = usrDoc.CheckZemanat;
                    kiloUser.Name = usr.Name;
                    kiloUser.TellNumber = usr.TellNumber;
                    kiloUser.UploadTime = usrDoc.LastUpTime;
                    kiloUser.VerifiedTime = usrDoc.VerifiedTime;
                    kiloUser.VerifiedStatus = usrDoc.Verified;
                    return kiloUser;
                }

            }
            catch (Exception e)
            {
                kiloUser.HasError = true;
                kiloUser.DescError = e.Message;
                return kiloUser;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public COM.ResultCheckBon CheckBon(string Tell, int SerialNumber, string Name, string Codemeli)
        {
            try
            {
                var bon = BLL.Bon.GetBonBySerial(SerialNumber);

                if (bon != null)//BID is availbe
                {

                    var userbon = BLL.Bon.GetUserBonByID(bon.BID);

                    if (userbon == null) // male kasi nabood
                    {

                        var user = BLL.Profile.GetUserByTellNumber(Tell);
                        if (user != null)
                        {
                            return new ResultCheckBon() { UID = 0, Error = "این شماره تلفن قبلا ثبت شده است", HasError = true };
                        }

                        COM.User NewUser = new User()
                        {
                            Active = false,
                            ActiveCode = CreateRandomCode(),
                            JoinDate = DateTime.Now,
                            Name = Name,
                            TellNumber = Tell,
                            CodeMeli = Codemeli,
                            Role = 0,
                        };
                        int ResUser = BLL.Profile.AddUser(NewUser);

                        if (ResUser > 0)
                        {
                            //send SMS
                            int ResUserBon = BLL.Bon.AddUserBon(ResUser, bon.BID);
                            new System.Threading.Thread(delegate () { SendActivationCodeViaSMS(Tell, NewUser.ActiveCode); }).Start();
                            return new ResultCheckBon() { UID = ResUser, Error = "", HasError = false };
                        }
                        else
                        {
                            return new ResultCheckBon() { UID = 0, Error = "خطا در ثبت اطلاعات", HasError = true };
                        }
                    }
                    else
                    {
                        //return ghablan sabt shode
                        return new ResultCheckBon() { UID = 0, Error = "سریال وارد شده به اسم شخص دیگری ثبت شده است", HasError = true };
                    }
                }
                else
                {
                    //BID dorost nist
                    return new ResultCheckBon() { UID = 0, Error = "سریال وارد شده صحیح نمی باشد", HasError = true };
                }
            }
            catch (Exception e)
            {
                return new ResultCheckBon() { UID = 0, Error = " خطای ناشناخته" + e.Message, HasError = true };

            }
        }

        [AcceptVerbs("Get")]
        public bool ValidateBon(int UID, string Code)
        {
            try
            {
                if (Code == null)
                {
                    return false;
                }

                var user = BLL.Profile.GetUserByID(UID);
                if (user != null)
                {
                    if (user.ActiveCode == Code)
                    {
                        user.Active = true;

                        if (BLL.Profile.UpdateUserActivation(user))
                        {

                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [AcceptVerbs("Get")]
        public IEnumerable<string> Get()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var Name = ClaimsPrincipal.Current.Identity.Name;
            var Name1 = User.Identity.Name;

            return new string[] { Name, Name1 };
        }

        // GET: api/Profile/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Profile
        public string Post([FromBody] string value)
        {
            return "Value was : " + value;
        }

        // PUT: api/Profile/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Profile/5
        public void Delete(int id)
        {
        }

        //////////////////////////////////////////////////
        /// Help API For Controler       /////////////////
        //////////////////////////////////////////////////

        string CreateRandomCode()
        {
            string[] SplitChars = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            Random random = new Random();
            string RandomPCode = string.Empty;
            for (int i = 0; i <= 4; i++)
            {
                RandomPCode += SplitChars[random.Next(1, SplitChars.Length)];
            }

            return RandomPCode;
        }

        bool SendActivationCodeViaSMS(string TellNumber, string RndNumber)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("6A6B364F786E554E3353725364357359306F4C796362394337355149386F793475756E702B3465386737513D");
                var result = api.VerifyLookup(TellNumber, RndNumber, "ActivationCode");

                return true;
            }
            catch (Kavenegar.Exceptions.ApiException)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.SendSMS, TellNumber, 0, "Return Not 200"); }).Start();
                return false;
            }
            catch (Kavenegar.Exceptions.HttpException)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.SendSMS, TellNumber, 0, "Not Connected"); }).Start();
                return false;
            }
            catch (Exception e)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.SendSMS, TellNumber, 0, e.Message); }).Start();
                return false;
            }
        }
    }


   
}

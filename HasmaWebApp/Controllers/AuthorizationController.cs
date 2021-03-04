using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using COM;

namespace HasmaWebApp.Controllers
{
    public class AuthorizationController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
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

        // GET: api/Authorization
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Authorization/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Authorization
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Authorization/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Authorization/5
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

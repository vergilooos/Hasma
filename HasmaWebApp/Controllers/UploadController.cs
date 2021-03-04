using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HasmaWebApp.Controllers
{
    [Authorize]
    public class UploadController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public HttpResponseMessage UploadKartMeliImage(int UID)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/Users/" + UID + "/");
                            if (!Directory.Exists(folderPath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(folderPath);
                            }

                            postedFile.SaveAs(folderPath + "KarteMeli" + extension);
                        }
                    }

                    COM.UserDocument mUserDocument = new COM.UserDocument() { UID = UID, KartMeli = 1,LastUpTime = DateTime.Now };
                    if (BLL.Profile.UpdateUserUpKart(mUserDocument))
                    {
                        var message1 = string.Format("Image Updated Successfully.");
                        return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                    }
                    else
                    {
                        var message = string.Format("User is not Here :)");
                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.uploadImgKart, UID.ToString(), -200, ex.Message + " Inner: " + ex.InnerException); }).Start();
                dict.Add("error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public HttpResponseMessage UploadKasrAzHoghoogh(int UID)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/Users/" + UID + "/");
                            if (!Directory.Exists(folderPath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(folderPath);
                            }

                            postedFile.SaveAs(folderPath + "KasrAzHoghoogh" + extension);
                        }
                    }
                    COM.UserDocument mUserDocument = new COM.UserDocument() { UID = UID, KasrHoghoogh = 1, LastUpTime = DateTime.Now };
                    if (BLL.Profile.UpdateUserUpKasr(mUserDocument))
                    {
                        var message1 = string.Format("Image Updated Successfully.");
                        return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                    }
                    else
                    {
                        var message = string.Format("User is not Here :)");
                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.uploadKasrAzHoghoogh, UID.ToString(), -200, ex.Message + " Inner: " + ex.InnerException); }).Start();
                dict.Add("error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public HttpResponseMessage UploadJavazKasb(int UID)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/Users/" + UID + "/");
                            if (!Directory.Exists(folderPath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(folderPath);
                            }

                            postedFile.SaveAs(folderPath + "JavazKasb" + extension);
                        }
                    }

                    COM.UserDocument mUserDocument = new COM.UserDocument() { UID = UID, JavazKasb = 1, LastUpTime = DateTime.Now };
                    if (BLL.Profile.UpdateUserJavazKasb(mUserDocument))
                    {
                        var message1 = string.Format("Image Updated Successfully.");
                        return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                    }
                    else
                    {
                        var message = string.Format("user is not herev :)");
                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.UploadJavazKasb, UID.ToString(), -200, ex.Message + " Inner: " + ex.InnerException); }).Start();
                dict.Add("error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public HttpResponseMessage UploadFishHoghooghi(int UID)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/Users/" + UID + "/");
                            if (!Directory.Exists(folderPath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(folderPath);
                            }

                            postedFile.SaveAs(folderPath + "FishHoghoogh" + extension);
                        }
                    }

                    COM.UserDocument mUserDocument = new COM.UserDocument() { UID = UID, FishHoghoogh = 1, LastUpTime = DateTime.Now };
                    if (BLL.Profile.UpdateUserFishHoghoogh(mUserDocument))
                    {
                        var message1 = string.Format("Image Updated Successfully.");
                        return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                    }
                    else
                    {
                        var message = string.Format("user is not here :)");
                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.UploadFishHoghooghi, UID.ToString(), -200, ex.Message + " Inner: " + ex.InnerException); }).Start();
                dict.Add("error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public HttpResponseMessage UploadChekZemanat(int UID)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/Users/" + UID + "/");
                            if (!Directory.Exists(folderPath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(folderPath);
                            }

                            postedFile.SaveAs(folderPath + "ChekZemanat" + extension);
                        }
                    }

                    COM.UserDocument mUserDocument = new COM.UserDocument() { UID = UID, CheckZemanat = 1, LastUpTime = DateTime.Now };
                    if (BLL.Profile.UpdateUserCheckZemanat(mUserDocument))
                    {
                        var message1 = string.Format("Image Updated Successfully.");
                        return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                    }
                    else
                    {
                        var message = string.Format("user is not here :)");
                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.UploadChekZemanat, UID.ToString(), -200, ex.Message + " Inner: " + ex.InnerException); }).Start();
                dict.Add("error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public HttpResponseMessage UploadProductImages(int PID)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/Products/" + PID + "/");
                            if (!Directory.Exists(folderPath))
                                Directory.CreateDirectory(folderPath);

                            string Cur1 = folderPath + "1.jpg";
                            string Cur2 = folderPath + "2.jpg";
                            string Cur3 = folderPath + "3.jpg";
                            string Cur4 = folderPath + "4.jpg";

                            if (!File.Exists(Cur1))
                            {
                                postedFile.SaveAs(Cur1);
                            }
                            else if (!File.Exists(Cur2))
                            {
                                postedFile.SaveAs(Cur2);
                            }
                            else if (!File.Exists(Cur3))
                            {
                                postedFile.SaveAs(Cur3);
                            }
                            else if (!File.Exists(Cur4))
                            {
                                postedFile.SaveAs(Cur4);
                            }
                            else
                            {
                                var message = string.Format("already there is 4 images");
                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.UploadProductImages, PID.ToString(), 0, null); }).Start();
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.UploadProductImages, PID.ToString(), -100, res); }).Start();

                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                new System.Threading.Thread(delegate () { BLL.Log.DoLog(COM.Action.UploadProductImages, PID.ToString(), -200, ex.Message + " Inner: " + ex.InnerException); }).Start();
                dict.Add("error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

    }
}

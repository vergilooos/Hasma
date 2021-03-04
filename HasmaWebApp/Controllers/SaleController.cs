using BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace HasmaWebApp.Controllers
{
    //[Authorize]
    public class SaleController : ApiController
    {

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public int GetPayStatus(int PayID)
        {
            var tmpPay = BLL.Payment.GetPayByID(PayID);
            return tmpPay.FinalState ?? -1;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<COM.Order> GetOrders()
        {
            return BLL.Salement.GetAllOrders();
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public bool SetOrderIsDeliver(int OID)
        {
            COM.Order order = new COM.Order()
            {
                OID = OID,
                IsDelivered = true
            };
            return BLL.Salement.UpdateOrderDeliverStatus(order);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]//Sale/GetStartPaymentInfo?UID=1
        public COM.StartPaymentInfo GetStartPaymentInfo(int UID)
        {
            var ghest = BLL.Product.GetGhestInfo();
            var usr = BLL.Profile.GetUserByID(UID);

            COM.MiliUser miliUser = new COM.MiliUser()
            {
                Address = usr.Address,
                City = usr.City,
                CodeMeli = usr.CodeMeli,
                CodePosti = usr.CodePosti,
                Name = usr.Name,
                TellNumber = usr.TellNumber
            };

            return new COM.StartPaymentInfo() { Ghest = ghest, MiliUser = miliUser };
        }

        [AcceptVerbs("Post")]

        public IHttpActionResult CallBackPayResult(HttpRequestMessage request)
        {
            try
            {
                new System.Threading.Thread(delegate () { Log.DoLog(COM.Action.CallBackPayResult, "", 0, "STAART"); }).Start();

                var form = request.Content.ReadAsFormDataAsync().Result;
                COM.PayEndResult payEndResult = new COM.PayEndResult();

                payEndResult.ErrorCode = form["ErrorCode"];
                payEndResult.BuyID = form["BuyID"];
                payEndResult.ErrorDescription = form["ErrorDescription"];
                payEndResult.ReferenceNumber = form["ReferenceNumber"];
                payEndResult.State = Int32.Parse(form["State"]);
                payEndResult.Token = form["Token"];
                payEndResult.TrackingNumber = form["TrackingNumber"];

                //payEndResult.ErrorCode = JsonConvert.DeserializeObject<string>(form["ErrorCode"]);
                //payEndResult.BuyID = JsonConvert.DeserializeObject<string>(form["BuyID"]);
                //payEndResult.ErrorDescription = JsonConvert.DeserializeObject<string>(form["ErrorDescription"].ToString());
                //payEndResult.ReferenceNumber = JsonConvert.DeserializeObject<string>(form["ReferenceNumber"].ToString());
                //payEndResult.State = JsonConvert.DeserializeObject<int>(form["State"].ToString());
                //payEndResult.Token = JsonConvert.DeserializeObject<string>(form["Token"].ToString());
                //payEndResult.TrackingNumber = JsonConvert.DeserializeObject<string>(form["TrackingNumber"].ToString());

                //COM.PayEndResult payEndResult = new COM.PayEndResult()
                //{
                //    BuyID = BuyID,
                //    ErrorCode = ErrorCode,
                //    ErrorDescription = ErrorDescription,
                //    ReferenceNumber = ReferenceNumber,
                //    State = State,
                //    Token = Token,
                //    TrackingNumber = TrackingNumber
                //};

                COM.PayVerifiedData payVerifiedData = new COM.PayVerifiedData();

                var pay = BLL.Payment.GetPayByBuyID(payEndResult.BuyID);
                if (pay == null)
                {
                    COM.PayVerifiedData mPayVerifiedDataTmp = new COM.PayVerifiedData()
                    {
                        ForceReverse = true,
                        Token = payEndResult.Token
                    };
                    var resTmp = PayConfirm(mPayVerifiedDataTmp);
                    //chio to Db updte konam Akhe !

                    new System.Threading.Thread(delegate () { Log.DoLog(COM.Action.CallBackPayResult, payEndResult.BuyID, 2, "Bad END"); }).Start();
                    return Redirect("https://www.hasma.ir/payment/failed");
                }
                else
                {
                    pay.ReferenceNumber = payEndResult.ReferenceNumber;
                    pay.DargahState = payEndResult.State;
                    pay.Token = payEndResult.Token;
                    pay.TrackingNumber = payEndResult.TrackingNumber;
                    pay.EndMoment = DateTime.Now;

                    BLL.Payment.UpdatePayAfterDargah(pay);
                }

                COM.PayVerifiedData mPayVerifiedData = new COM.PayVerifiedData()
                {
                    ForceReverse = false,
                    Token = pay.Token,
                    TrackingNumber = pay.TrackingNumber,
                    State = pay.DargahState ?? 0,
                    ReferenceNumber = pay.ReferenceNumber,
                    Amount = pay.Amount,
                    BuyID = pay.BuyID
                };
                var resFinal = PayConfirm(mPayVerifiedData);

                if (resFinal.Status == 1)
                {
                    pay.FinalState = resFinal.Status;
                    pay.IsReverse = false;
                    BLL.Payment.UpdatePayAfterConfirm(pay);
                }
                else
                {
                    pay.FinalState = resFinal.Status;
                    pay.IsReverse = true;
                    BLL.Payment.UpdatePayAfterConfirm(pay);
                }

                new System.Threading.Thread(delegate () { Log.DoLog(COM.Action.CallBackPayResult, payEndResult.BuyID, 1, "Happy END"); }).Start();
                return Redirect("https://www.hasma.ir/payment/Success/"+ payEndResult.TrackingNumber);
            }
            catch (Exception e)
            {
                new System.Threading.Thread(delegate () { Log.DoLog(COM.Action.CallBackPayResult, "payEndResult", -100, e.Message); }).Start();
                return Redirect("https://www.hasma.ir/payment/failed");
            }
        }



        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<COM.BuyResult> Buy()
        {
            COM.BuyResult buyResult = new COM.BuyResult();
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    buyResult.HasError = true;
                    buyResult.StateDesc = "Exception dade: UnsupportedMediaType";
                    return buyResult;
                }


                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                if (filesReadToProvider.Contents[0].Headers.ContentDisposition.Name == "Object" || filesReadToProvider.Contents[0].Headers.ContentDisposition.Name == "\"Object\"")
                {
                    var jsonString = await filesReadToProvider.Contents[0].ReadAsStringAsync();
                    var serializer = new JavaScriptSerializer();
                    COM.MiddlePayment mMiddlePayment = serializer.Deserialize<COM.MiddlePayment>(jsonString);


                    double SumPrice = 0;
                    foreach (var itemPro in mMiddlePayment.productInfos)
                    {
                        var pr = BLL.Product.GetProductByID(itemPro.PID);
                        if (pr == null)
                        {
                            buyResult.HasError = true;
                            buyResult.StateDesc = "product nistesh";
                            return buyResult;
                        }
                        SumPrice = SumPrice + (pr.PriceOff * itemPro.Count);
                    }

                    var ghestInfo = BLL.Product.GetGhestInfo();

                    if (mMiddlePayment.GhestCount > ghestInfo.MaxGhestNumber || mMiddlePayment.GhestCount < ghestInfo.MinGhestNumber)
                    {
                        buyResult.HasError = true;
                        buyResult.StateDesc = "tedade ghest mojaz nemibashad";
                        return buyResult;
                    }

                    double MablagheGhabelePardakht = 0;
                    double MablaghHarGhest = 0;
                    if (SumPrice < ghestInfo.MinGhestValue)
                    {
                        // go to dagah pardaj
                        MablagheGhabelePardakht = SumPrice;
                    }
                    else
                    {

                        double A = (SumPrice + (((SumPrice * ghestInfo.Percentage) / 100.0f) * mMiddlePayment.GhestCount) - ghestInfo.MinPishPardakht);
                        if (A < ghestInfo.MaxGhestValue)
                        {
                            MablaghHarGhest = A / mMiddlePayment.GhestCount;
                            MablagheGhabelePardakht = ghestInfo.MinPishPardakht;
                        }
                        else
                        {
                            double C = A - ghestInfo.MaxGhestValue;
                            MablagheGhabelePardakht = ghestInfo.MinPishPardakht + C;
                            MablaghHarGhest = C / mMiddlePayment.GhestCount;
                        }

                    }

                    string BuyID = "70000324" + DateTime.Now.ToString("MMddyyyyhmms");

                    COM.Order order = new COM.Order()
                    {
                        IsDelivered = false,
                        OrderTime = DateTime.Now,
                        PayStatus = false,
                        ProductsInfo = JsonConvert.SerializeObject(mMiddlePayment.productInfos),
                        ReciverInfo = JsonConvert.SerializeObject(mMiddlePayment.MiliUser),
                        UID = mMiddlePayment.MiliUser.UID,
                        BuyID = BuyID,
                        GhestNumber = mMiddlePayment.GhestCount,
                        GhestValue = (long)MablaghHarGhest,
                        MainPrice = (long)SumPrice,
                        TypePay = mMiddlePayment.PayoffType,
                        PishPardakht = (long)MablagheGhabelePardakht
                    };
                    int OIDRes = BLL.Salement.AddOrder(order);
                    if (OIDRes < 0)
                    {
                        buyResult.HasError = true;
                        buyResult.StateDesc = "Etela'at Kharid dar DB Save nashode";
                        return buyResult;
                    }



                    COM.PayResultRequested payResultRequested = PayStart(10000, BuyID);
                    if (payResultRequested.Result > 0)
                    {

                        COM.Pay newPay = new COM.Pay()
                        {
                            Amount = 10000,//MablagheGhabelePardakht
                            BuyID = BuyID,
                            StartMoment = DateTime.Now,
                            UID = mMiddlePayment.MiliUser.UID,
                            OID = OIDRes,
                            
                        };
                        int resDB = BLL.Payment.AddPay(newPay);
                        if (resDB < 0)
                        {
                            buyResult.HasError = true;
                            buyResult.StateDesc = "Etela'at pardakht dar DB Save nashode";
                            return buyResult;
                        }

                        buyResult.PayID = resDB;
                        buyResult.MablagheKoleKharid = (long)SumPrice;
                        buyResult.MablagheHarGhest = (long)MablaghHarGhest;
                        buyResult.MablaghePishpardakht = (long)MablagheGhabelePardakht;
                        buyResult.HasError = false;
                        buyResult.LinkPardakht = "http://www.poolban.ir/V2PayGate/Pool/StartPayRedirectww/" + payResultRequested.Result;
                        buyResult.StateDesc = "OK Successsssss hoooraaa";
                    }
                    else
                    {
                        buyResult.MablagheKoleKharid = (long)SumPrice;
                        buyResult.MablagheHarGhest = (long)MablaghHarGhest;
                        buyResult.MablaghePishpardakht = (long)MablagheGhabelePardakht;
                        buyResult.HasError = true;
                        buyResult.StateDesc = "irad dar dargahe pardakht";
                    }

                    return buyResult;
                }
                else
                {
                    buyResult.HasError = true;
                    buyResult.StateDesc = "eshtbahi dar ersal Post pish amade";
                    return buyResult;
                }
            }
            catch (Exception e)
            {
                new System.Threading.Thread(delegate () { Log.DoLog(COM.Action.Buy, "", -100, e.Message); }).Start();
                buyResult.HasError = true;
                buyResult.StateDesc = "Exception dade: " + e.Message;
                return buyResult;
            }
        }

        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<COM.BuyResult> ReBuy()
        {
            COM.BuyResult buyResult = new COM.BuyResult();
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    buyResult.HasError = true;
                    buyResult.StateDesc = "Exception dade: UnsupportedMediaType";
                    return buyResult;
                }


                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                if (filesReadToProvider.Contents[0].Headers.ContentDisposition.Name == "Object" || filesReadToProvider.Contents[0].Headers.ContentDisposition.Name == "\"Object\"")
                {
                    var jsonString = await filesReadToProvider.Contents[0].ReadAsStringAsync();
                    var serializer = new JavaScriptSerializer();
                    COM.MiddleRePayment mMiddleRePayment = serializer.Deserialize<COM.MiddleRePayment>(jsonString);

                    var PreOrder = BLL.Salement.GetOrdersByOID(mMiddleRePayment.OID);
                    if (PreOrder ==  null)
                    {
                        buyResult.HasError = true;
                        buyResult.StateDesc = "in Order vojood nadarad";
                        return buyResult;
                    }

                    if (PreOrder.PayStatus)
                    {
                        buyResult.HasError = true;
                        buyResult.StateDesc = "in sefaresh ghablan pardakt shode ast";
                        return buyResult;
                    }

                    List<COM.ProductInfo> ListProductInfo  = serializer.Deserialize<List<COM.ProductInfo>>(PreOrder.ProductsInfo);


                    double SumPrice = 0;
                    foreach (var itemPro in ListProductInfo)
                    {
                        var pr = BLL.Product.GetProductByID(itemPro.PID);
                        if (pr == null)
                        {
                            buyResult.HasError = true;
                            buyResult.StateDesc = "product nistesh";
                            return buyResult;
                        }
                        SumPrice = SumPrice + (pr.PriceOff * itemPro.Count);
                    }

                    var ghestInfo = BLL.Product.GetGhestInfo();

                    if (PreOrder.GhestNumber > ghestInfo.MaxGhestNumber || PreOrder.GhestNumber < ghestInfo.MinGhestNumber)
                    {
                        buyResult.HasError = true;
                        buyResult.StateDesc = "tedade ghest mojaz nemibashad";
                        return buyResult;
                    }

                    double MablagheGhabelePardakht = 0;
                    double MablaghHarGhest = 0;
                    if (SumPrice < ghestInfo.MinGhestValue)
                    {
                        // go to dagah pardaj
                        MablagheGhabelePardakht = SumPrice;
                    }
                    else
                    {

                        double A = (SumPrice + (((SumPrice * ghestInfo.Percentage) / 100.0f) * PreOrder.GhestNumber) - ghestInfo.MinPishPardakht);
                        if (A < ghestInfo.MaxGhestValue)
                        {
                            MablaghHarGhest = A / PreOrder.GhestNumber;
                            MablagheGhabelePardakht = ghestInfo.MinPishPardakht;
                        }
                        else
                        {
                            double C = A - ghestInfo.MaxGhestValue;
                            MablagheGhabelePardakht = ghestInfo.MinPishPardakht + C;
                            MablaghHarGhest = C / PreOrder.GhestNumber;
                        }

                    }

                    string BuyID = "70000324" + DateTime.Now.ToString("MMddyyyyhmms");


                    PreOrder.BuyID = BuyID;
                    PreOrder.GhestValue = (long)MablaghHarGhest;
                    PreOrder.MainPrice = (long)SumPrice;
                    PreOrder.PishPardakht = (long)MablagheGhabelePardakht;
                    
                    bool OIDRes = BLL.Salement.UpdateOrderReBuy(PreOrder);
                    if (!OIDRes)
                    {
                        buyResult.HasError = true;
                        buyResult.StateDesc = "Etela'at Kharid dar DB Update nashode";
                        return buyResult;
                    }



                    COM.PayResultRequested payResultRequested = PayStart(10000, BuyID);
                    if (payResultRequested.Result > 0)
                    {

                        COM.Pay newPay = new COM.Pay()
                        {
                            Amount = 10000,//MablagheGhabelePardakht
                            BuyID = BuyID,
                            StartMoment = DateTime.Now,
                            UID = mMiddleRePayment.UID,
                            OID = mMiddleRePayment.OID,

                        };
                        int resDB = BLL.Payment.AddPay(newPay);
                        if (resDB < 0)
                        {
                            buyResult.HasError = true;
                            buyResult.StateDesc = "Etela'at pardakht dar DB Save nashode";
                            return buyResult;
                        }

                        buyResult.PayID = resDB;
                        buyResult.MablagheKoleKharid = (long)SumPrice;
                        buyResult.MablagheHarGhest = (long)MablaghHarGhest;
                        buyResult.MablaghePishpardakht = (long)MablagheGhabelePardakht;
                        buyResult.HasError = false;
                        buyResult.LinkPardakht = "http://www.poolban.ir/V2PayGate/Pool/StartPayRedirectww/" + payResultRequested.Result;
                        buyResult.StateDesc = "OK Successsssss hoooraaa";
                    }
                    else
                    {
                        buyResult.MablagheKoleKharid = (long)SumPrice;
                        buyResult.MablagheHarGhest = (long)MablaghHarGhest;
                        buyResult.MablaghePishpardakht = (long)MablagheGhabelePardakht;
                        buyResult.HasError = true;
                        buyResult.StateDesc = "irad dar dargahe pardakht";
                    }

                    return buyResult;
                }
                else
                {
                    buyResult.HasError = true;
                    buyResult.StateDesc = "eshtbahi dar ersal Post pish amade";
                    return buyResult;
                }
            }
            catch (Exception e)
            {
                new System.Threading.Thread(delegate () { Log.DoLog(COM.Action.Buy, "", -100, e.Message); }).Start();
                buyResult.HasError = true;
                buyResult.StateDesc = "Exception dade: " + e.Message;
                return buyResult;
            }
        }

        public COM.PayResultRequested PayStart(int Amount, string BuyID)
        {

            COM.PayStart mPayStart = new COM.PayStart()
            {
                Amount = Amount,
                BuyID = BuyID,
                CallBackURl = "https://www.hasma.ir/HasmaAPI/Sale/CallBackPayResult",
                Language = "fa",
                TerminalNumber = "70000324"
            };

            var webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=utf-8";
            var rawMessage = JsonConvert.SerializeObject(mPayStart);

            rawMessage = webClient.UploadString("http://www.poolban.ir/payGate/pay/SendCustomerToIPG", rawMessage);

            var Result = JsonConvert.DeserializeObject<COM.PayResultRequested>(rawMessage);

            return Result;
        }

        public COM.PayFinalResult PayConfirm(COM.PayVerifiedData mPayVerifiedData)
        {

            var webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json;charset=utf-8";
            var rawMessage = JsonConvert.SerializeObject(mPayVerifiedData);

            rawMessage = webClient.UploadString("http://www.poolban.ir/PayGate/pay/VerifiedCustomerForIPG", rawMessage);

            var Result = JsonConvert.DeserializeObject<COM.PayFinalResult>(rawMessage);

            return Result;
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public void GetGhestDetail(int UID)
        {
            try
            {

            }
            catch (Exception e)
            {

            }
        }


    }
}

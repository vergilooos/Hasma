using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM
{
    public enum Action
    {
        AddUser,
        AddUserDocument,
        AddUserMessageToAll,
        AddProduct,
        AddPay,
        AddOrder,
        PostAddProduct,
        PostUpdateProduct,
        UpdateUser,
        UpdateUserCode,
        UpdateUserProfile,
        UpdateUserInformationByAdmin,
        UpdatePayAfterConfirm,
        UpdatePayAfterDargah,
        UpdateOrderPayStatus,
        UpdateOrderDeliverStatus,
        UpdateOrderReBuy,
        SignUpUser,
        ActiveUser,
        GetUser,
        GetPayByID,
        GetPayByBuyID,
        GetUserMessagebyUID,
        GetUserByTellNumber,
        GetUserByCodeMeli,
        GetCategoriesByGroupName,
        GetAllUserDocumentUploaded,
        GetAllUserDocumentUploadedID,
        GetAllOrders,
        GetOrdersByOID,
        GetOrdersByUID,
        UserProductAvailable,
        Buy,
        GetUserProductHistorybyUID,
        Logon,
        Login,
        SendSMS,
        GetBonBySerial,
        AddBonSerial,
        AddUserBon,
        GetUserBonByID,
        GetUserByID,
        GetProductByID,
        GetALLProduct,
        GetALLProductByCatID,
        GetAllUserByID,
        GetCategories,
        UserDocument,
        UpdateUserUpFish,
        UpdateUserJavazKasb,
        UpdateUserUpJavaz,
        UpdateUserCheckZemanat,
        UpdateUserFishHoghoogh,
        UpdateUserMessageSeeableBYUMID,
        UpdateUserUpKart,
        UpdateUserUpKasr,
        UpdateUserActivation,
        UpdateUserActiveCode,
        uploadImgKart,
        uploadImgCheks,
        UploadFishHoghooghi,
        uploadKasrAzHoghoogh,
        UploadChekZemanat,
        UploadProductImages,
        UploadJavazKasb,
        UpdateProduct,
        GetMiniProcuctsByCatIDs,
        SendMsgToAllUser,
        SendMsgToOneUser,
        CallBackPayResult
    }


    public class ResultCheckBon
    {
        public int UID { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }


    public class MiniProcuct
    {
        public int PID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
    public class Msg
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int UID { get; set; }

    }



    public class Stuff
    {
        public int PID { get; set; }
        public int Number { get; set; }
    }

    public class Cart
    {
        public List<Stuff> Stuffs { get; set; }
        public int UID { get; set; }
    }


    public class ResBuy
    {
        public string Error { get; set; }
        public long amount { get; set; }

    }
    public class MiniUser
    {
        public int UID { get; set; }
        public string TellNumber { get; set; }
        public string Name { get; set; }
        public string CodeMeli { get; set; }
        public Nullable<System.DateTime> JoinTime { get; set; }
    }

    public class MiliUser
    {
        public int UID { get; set; }
        public string TellNumber { get; set; }
        public string Name { get; set; }
        public string CodeMeli { get; set; }
        public string Address { get; set; }
        public string CodePosti { get; set; }
        public string City { get; set; }
    }


    public class StartPaymentInfo
    {
        public MiliUser MiliUser { get; set; }
        public COM.Ghest Ghest { get; set; }
    }

    public class MiddleRePayment
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public string BuyID { get; set; }
    }
    public class MiddlePayment
    {
        public int PaymentType { get; set; }//1 => naghdi , 2=> ghesti
        public int DeliverType { get; set; }//1 => ersal Peyk , 2=> tahvil Hozoori
        public int PayoffType { get; set; }//1 => Online , 2=> Hozoori
        public int GhestCount { get; set; }
        public List<ProductInfo> productInfos { get; set; }
        public MiliUser MiliUser { get; set; }

    }

    public class ProductInfo
    {
        public int PID { get; set; }
        public int Count { get; set; }
    }


    public class BuyResult
    {
        public long MablaghePishpardakht { get; set; }
        public long MablagheKoleKharid { get; set; }
        public long MablagheHarGhest { get; set; }
        public bool HasError { get; set; }
        public string StateDesc { get; set; }
        public string LinkPardakht { get; set; }
        public int PayID { get; set; }
    }


    public class PayStart
    {
        public int Amount { get; set; }
        public string Language { get; set; }
        public string BuyID { get; set; }
        public string TerminalNumber { get; set; }
        public string CallBackURl { get; set; }

    }
    public class PayResultRequested
    {
        public int Result { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
    }

    [Serializable]
    public class PayEndResult
    {
        public int State { get; set; }
        public string BuyID { get; set; }
        public string Token { get; set; }
        public string TrackingNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
    }

    public class PayFinalResult
    {
        public int Status { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
    }

    public class PayVerifiedData
    {
        public long Amount { get; set; }
        public int State { get; set; }
        public string BuyID { get; set; }
        public string Token { get; set; }
        public string TrackingNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public bool ForceReverse { get; set; }
    }

    public class LogonResult
    {
        public int UserID { get; set; }
        public bool IsNew { get; set; }
        public string NickName { get; set; }
    }

    public class LastUserResult
    {
        public int MessageNum { get; set; }
        public int ShopListNum { get; set; }
        public int GhestNum { get; set; }
        public int ProfileNum { get; set; }
    }

    public class LastStaticInfoResult
    {
        public int UserCount { get; set; }
        public int TotalIncome { get; set; }
        public int GhestpardakhtShodeNumber { get; set; }
        public int GhestpardakhtNashodeNumber { get; set; }
        public int ProductCount { get; set; }
        public int ProductInStockCount { get; set; }
    }
    public class UserInfo
    {
        public int UID { get; set; }
        public string TellNumber { get; set; }
        public string Name { get; set; }
        public string CodeMeli { get; set; }
        public string Address { get; set; }
        public string CodePosti { get; set; }
        public string City { get; set; }
        public string Bon { get; set; }
    }


    public class UpdateUserResult
    {
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
    }
    

    public class Receiver
    {
        public string Name { get; set; }
        public string Tell { get; set; }
        public string Address { get; set; }
        public string TrackingNumber { get; set; }

    }


    public class kiloUser
    {
        public int UID { get; set; }
        public bool HasError { get; set; }
        public string DescError { get; set; }
        public string TellNumber { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string CodeMeli { get; set; }
        public string Address { get; set; }
        public string CodePosti { get; set; }
        public string City { get; set; }
        public int KartMeliStatus { get; set; }
        public int KasrHoghooghStatus { get; set; }
        public int JavazKasbStatus { get; set; }
        public int FishHoghooghStatus { get; set; }
        public int CheckZemanatStatus { get; set; }
        public Nullable<System.DateTime> UploadTime { get; set; }
        public Nullable<System.DateTime> VerifiedTime { get; set; }
        public bool VerifiedStatus { get; set; }
    }

    public class HectoUser
    {
        public int UID { get; set; }
        public string TellNumber { get; set; }
        public string Name { get; set; }
        public string CodeMeli { get; set; }
        public string Address { get; set; }
        public string CodePosti { get; set; }
        public string City { get; set; }
        public int KartMeliStatus { get; set; }
        public int KasrHoghooghStatus { get; set; }
        public int JavazKasbStatus { get; set; }
        public int FishHoghooghStatus { get; set; }
        public int CheckZemanatStatus { get; set; }
        public bool VerifiedStatus { get; set; }
    }
}

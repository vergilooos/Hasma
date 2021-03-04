using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hasma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new HttpClient(); //if used frequently, don't dispose this guy, make him a singleton if you can (see link below)
                for (int i = 210320; i < 230321; i++)
                {
                    //https://localhost:44372/HasmaAPI/Profile/Get

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");

                    var result = await client.GetAsync("https://localhost:44372/HasmaAPI/Values/AddBon?SerialNumber=" + i);
                    Console.WriteLine(result.Content);
                }
            }
            catch(Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {

            string c = "1.jpg." + DateTime.Now.ToString("MMddyyyyhmmss");
            Console.WriteLine(c);
            //List<Spec> specs = new List<Spec>() { new Spec() { label="rang",value="sabz"} };

            //ProductMorphy productMorphy = new ProductMorphy()
            //{
            //    Brand = "Hasma",
            //    CatID = 1,
            //    Description = DateTime.Now.ToString(),
            //    Name = "Test",
            //    Price= 200000,
            //    PriceOff = 1990000,
            //    StockCount = 5,
            //    SubCatID = 1,
            //    specification = specs
            //};


            //string cc = await PostToServer(productMorphy);

            //int MTIDnew = Int32.Parse(cc);
        }

        public async Task<string> PostToServer(ProductMorphy mProductMorphy)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        string txt = JsonConvert.SerializeObject(mProductMorphy);
                        StringContent contentBody = new StringContent(txt);
                        form.Add(contentBody, "Object");
                        string BaseAddress = "https://www.hasma.ir/";
                        HttpResponseMessage response = await httpClient.PostAsync(BaseAddress + "HasmaAPI/Product/PostAddProduct", form);

                        string ResStr = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(response.Content.ReadAsStringAsync());
                        return ResStr;
                    }
                }
            }
            catch (Exception eeee)
            {
                Console.WriteLine(eeee.Message);
                return "err";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MiddlePayment middlePayment = new MiddlePayment();
            middlePayment.DeliverType = 1;
            middlePayment.GhestCount = 4;
            middlePayment.MiliUser = new MiliUser() {Address ="adres", City ="rasht" ,CodeMeli = "21313",CodePosti = "12313" ,Name ="aac" ,TellNumber ="110" ,UID =33  };
            middlePayment.PaymentType = 1;
            middlePayment.PayoffType = 1;
            middlePayment.productInfos = new List<ProductInfo>() { new ProductInfo() { PID = 5, Count = 2 }, new ProductInfo() { Count = 1, PID = 3 } };

            string txt = JsonConvert.SerializeObject(middlePayment);
        }
    }
    public class MiddlePayment
    {
        public int PaymentType { get; set; }//1 => naghdi , 2=> ghesti
        public int DeliverType { get; set; }//1 => ersal Peyk , 2=> tahvil Hozoori
        public int PayoffType { get; set; }//1 => Online , 2=> Hozoori
        public List<ProductInfo> productInfos { get; set; }
        public MiliUser MiliUser { get; set; }
        public int GhestCount { get; set; }

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
    public class ProductInfo
    {
        public int PID { get; set; }
        public int Count { get; set; }
    }
    public class ProductMorphy
    {
        public int PID { get; set; }
        public string Name { get; set; }
        public int CatID { get; set; }
        public int SubCatID { get; set; }
        public int Price { get; set; }
        public int? PriceOff { get; set; }
        public string Description { get; set; }
        public List<Spec> specification { get; set; }
        public string Brand { get; set; }
        public int? StockCount { get; set; }
    }

    public class Spec
    {
        public string value { get; set; }
        public string label { get; set; }
    }
}

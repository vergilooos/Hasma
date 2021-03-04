using BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace HasmaWebApp.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        //getrandomProductByCat
        //getHighRatedBoughtedProduct



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<COM.Product> GetALLProduct()
        {
            return BLL.Product.GetALLProduct();
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public bool RemmoveProduct(int PID)
        {
            return BLL.Product.RemoveProductByID(PID);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public bool UpdateProduct([FromBody] COM.Product NewProduct)
        {
            COM.Product product = new COM.Product()
            {
                Brand = NewProduct.Brand,
                CatID = NewProduct.CatID,
                Description = NewProduct.Description,
                Name = NewProduct.Name,
                Price = NewProduct.Price,
                PriceOff = NewProduct.PriceOff,
                StockCount = NewProduct.StockCount,
                SubCatID = NewProduct.SubCatID,
                PID = NewProduct.PID
            };

            product.specification = JsonConvert.SerializeObject(NewProduct.specification);

            return BLL.Product.UpdateProduct(product);
        }


        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> PostUpdateProduct()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Json(JsonConvert.SerializeObject("Exception dade: UnsupportedMediaType"));
                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                ProductMorphy mProductMorphy = new ProductMorphy();

                foreach (var itemContent in filesReadToProvider.Contents)
                {
                    if (itemContent.Headers.ContentDisposition.Name == "Object" || itemContent.Headers.ContentDisposition.Name == "\"Object\"")
                    {
                        var jsonString = await itemContent.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        mProductMorphy = serializer.Deserialize<ProductMorphy>(jsonString);

                        COM.Product mProduct = new COM.Product()
                        {
                            PID = mProductMorphy.PID,
                            Brand = mProductMorphy.Brand,
                            CatID = mProductMorphy.CatID,
                            Description = mProductMorphy.Description,
                            Name = mProductMorphy.Name,
                            Price = mProductMorphy.Price,
                            PriceOff = mProductMorphy.PriceOff,
                            StockCount = mProductMorphy.StockCount,
                            SubCatID = mProductMorphy.SubCatID,
                        };

                        mProduct.specification = JsonConvert.SerializeObject(mProductMorphy.specification);

                        bool ResAdd = BLL.Product.UpdateProduct(mProduct);
                        if (!ResAdd)
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe "));
                        else
                            return Json(JsonConvert.SerializeObject(ResAdd.ToString()));
                    }
                }
                return Json(JsonConvert.SerializeObject("az halghe biroon oomad toosh hich chizi naboode"));
            }
            catch (Exception ee)
            {
                Log.DoLog(COM.Action.PostUpdateProduct, "UpdatePro", -100, ee.Message);
                return Json(JsonConvert.SerializeObject("Exception dade: " + ee.Message));
            }
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public Menu GetMenu()
        {
            Menu menu = new Menu();
            menu.menuPrimary = new menuPrimary();
            menu.menuPrimary.menu_1 = new List<X1>() {
            new X1(){ text = "هسما",url="/"},
            new X1(){ text = "خانه و آشپزخانه",url="/",extraClass="menu-item-has-children has-mega-menu",subClass="sub-menu",mega="true"},
            new X1(){ text = "کالای دیجیتال",url="/",extraClass="menu-item-has-children has-mega-menu",subClass="sub-menu",mega="true"},
            new X1(){ text = "مد و پوشاک",url="/",extraClass="menu-item-has-children has-mega-menu",subClass="sub-menu",mega="true"},
            new X1(){ text = "خوردنی و آشامیدنی",url="/",extraClass="menu-item-has-children has-mega-menu",subClass="sub-menu",mega="true"},
            new X1(){ text = "خودرو و لوازم صنعتی",url="/",extraClass="menu-item-has-children has-mega-menu",subClass="sub-menu",mega="true"},
            new X1(){ text = "مقررات فروشگاه",url="/page/our-ruls"},
            new X1(){ text = "روند همکاری",url="/page/partnership"},
            new X1(){ text = "ارتباط با ما",url="/page/contact-us"},
            new X1(){ text = "درباره ما",url="/page/about-us"},
            };

            List<string> MenuGroup = new List<string>() { "خانه و آشپزخانه", "کالای دیجیتال", "مد و پوشاک", "خوردنی و آشامیدنی", "خودرو و لوازم صنعتی" };
            int index = 1;
            foreach (var itemMenuGroup in MenuGroup)
            {


                menu.menuPrimary.menu_1[index].megaContent = new List<X2>();
                var allcat = BLL.Product.GetCategoriesByGroupName(itemMenuGroup);
                int maxValueCatID = allcat.Max(x => x.CatID);

                for (int i = 1; i <= maxValueCatID; i++)
                {
                    var allcatByID = allcat.Where(C => C.CatID == i).ToList();
                    if (allcatByID != null && allcatByID.Count != 0)
                    {
                        X2 MiniSubCat = new X2() { heading = allcatByID[0].Name, CatID = i, megaItems = new List<X3>(), ImgName = allcatByID[0].ImgName };
                        foreach (var SubCat in allcatByID)
                        {
                            MiniSubCat.megaItems.Add(new X3() { SubCatID = SubCat.SubCatID, text = SubCat.SubName, url = "/shop/" + i + "/" + SubCat.SubCatID + "/" + SubCat.SubName });//shop/catid/subcatid/subcatname
                        }
                        menu.menuPrimary.menu_1[index].megaContent.Add(MiniSubCat);
                    }
                }

                index++;
            }

            return menu;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<COM.Category> GetCategories()
        {
            return BLL.Product.GetCategories();
        }


        //GetCategories
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public ProductMorphy GetProductByPID(int PID)
        {
            var mProduct = BLL.Product.GetProductByID(PID);

            ProductMorphy productMorphy = new ProductMorphy()
            {
                Brand = mProduct.Brand,
                PID = mProduct.PID,
                specification = JsonConvert.DeserializeObject<List<Spec>>(mProduct.specification),
                SubCatID = mProduct.SubCatID,
                StockCount = mProduct.StockCount,
                CatID = mProduct.CatID,
                Description = mProduct.Description,
                Name = mProduct.Name,
                Price = mProduct.Price,
                PriceOff = mProduct.PriceOff
            };

            return productMorphy;
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<COM.MiniProcuct> GetAllMiniProductBtCategoryID(int CatID)
        {
            return BLL.Product.GetMiniProcuctsByCatIDs(CatID);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("Get")]
        public List<COM.MiniProcuct> GetAllMiniProductBtCategoryID(int CatID, int SubCatID)
        {
            return BLL.Product.GetMiniProcuctsBySubCatIDs(CatID, SubCatID);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        [AcceptVerbs("OPTIONS")]
        public int AddProduct([FromBody] ProductMorphy NewProduct)
        {
            COM.Product product = new COM.Product()
            {
                Brand = NewProduct.Brand,
                CatID = NewProduct.CatID,
                Description = NewProduct.Description,
                Name = NewProduct.Name,
                Price = NewProduct.Price,
                PriceOff = NewProduct.PriceOff,
                StockCount = NewProduct.StockCount,
                SubCatID = NewProduct.SubCatID,
            };

            product.specification = JsonConvert.SerializeObject(NewProduct.specification);

            return BLL.Product.AddProduct(product);
        }

        [AllowAnonymous]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> PostAddProduct()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    //  return StatusCode(HttpStatusCode.UnsupportedMediaType);
                    return Json(JsonConvert.SerializeObject("Exception dade: UnsupportedMediaType"));

                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

                ProductMorphy mProductMorphy = new ProductMorphy();

                foreach (var itemContent in filesReadToProvider.Contents)
                {
                    if (itemContent.Headers.ContentDisposition.Name == "Object" || itemContent.Headers.ContentDisposition.Name == "\"Object\"")
                    {
                        var jsonString = await itemContent.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        mProductMorphy = serializer.Deserialize<ProductMorphy>(jsonString);

                        COM.Product mProduct = new COM.Product()
                        {
                            Brand = mProductMorphy.Brand,
                            CatID = mProductMorphy.CatID,
                            Description = mProductMorphy.Description,
                            Name = mProductMorphy.Name,
                            Price = mProductMorphy.Price,
                            PriceOff = mProductMorphy.PriceOff,
                            StockCount = mProductMorphy.StockCount,
                            SubCatID = mProductMorphy.SubCatID,
                        };

                        mProduct.specification = JsonConvert.SerializeObject(mProductMorphy.specification);

                        int ResAdd = BLL.Product.AddProduct(mProduct);
                        if (ResAdd < 0)
                        {
                            return Json(JsonConvert.SerializeObject("natoooneste to DB save beshe "));
                        }

                        return Ok(ResAdd.ToString());
                    }
                }
                return Json(JsonConvert.SerializeObject("az halghe biroon oomad toosh hich chizi naboode"));

            }
            catch (Exception ee)
            {
                Log.DoLog(COM.Action.PostAddProduct, "addpro", -100, ee.Message);
                return Json(JsonConvert.SerializeObject("Exception dade: " + ee.Message));
            }
        }


        // GET: api/Product
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Product/5
        public string Get(int id)
        {
            return "value";
        }

    }


    public class Menu
    {
        public menuPrimary menuPrimary { get; set; }
    }

    public class menuPrimary
    {
        public List<X1> menu_1 { get; set; }
    }


    public class X1
    {
        public string text { get; set; }
        public string url { get; set; }
        public string extraClass { get; set; }
        public string subClass { get; set; }
        public string mega { get; set; }
        public List<X2> megaContent { get; set; }

    }

    public class X2
    {
        public string heading { get; set; }
        public int CatID { get; set; }
        public string ImgName { get; set; }
        public List<X3> megaItems { get; set; }
    }

    public class X3
    {
        public string text { get; set; }
        public string url { get; set; }
        public int SubCatID { get; set; }

    }


    public class ProductMorphy
    {
        public int PID { get; set; }
        public string Name { get; set; }
        public int CatID { get; set; }
        public int SubCatID { get; set; }
        public int Price { get; set; }
        public int PriceOff { get; set; }
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

using ETicaretSitesi.Filter;
using ETicaretSitesi.Models;
using ETicaretSitesi.Models.CodeFirstVT;
using ETicaretSitesi.Models.EntityFramework;
using ETicaretSitesi.Models.HomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            IQueryable<Product> products = Context.Products.OrderByDescending(x => x.OlusturmaTarihi).Where(x => x.İsDeleted == false || x.İsDeleted == null);
            Category category = null;
            if (id > 0)
            {
                category = Context.Categories.FirstOrDefault(x => x.Id == id);
                var allcategories = GetChildCategories(category);
                allcategories.Add(category);

                var catInList = allcategories.Select(x => x.Id).ToList();
                //Bir int nesnenin içindeki tüm kategorileri listeliyor
                products = products.Where(x => catInList.Contains(x.Category_Id));

            }
            var viewmodel = new Models.HomeModel.HomeModel()
            {
                Product = products.ToList(),
                Category = category
            };
            return View(viewmodel);
        }
        [HttpGet]
        public ActionResult Product(int id = 0)
        {
            var pro = Context.Products.FirstOrDefault(x => x.Id == id);

            if (pro == null) return RedirectToAction("index", "Home");
            ProductModels model = new ProductModels()
            {
                Product = pro,
                Comments = pro.Comments.ToList()
            };
            return View(model);
        }
        [HttpPost]
        [MyAutharization]
        public ActionResult Product(Comments comment)
        {
            try
            {
                comment.Member_Id = base.CurrentUserId();
                comment.OlusturmaTarihi = DateTime.Now;
                Context.Comments.Add(comment);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.MyError = ex.Message;
            }
            return RedirectToAction("Product", "Home");

        }
        [HttpGet]
        public ActionResult AddBasket(int id, bool remove = false)
        {
            List<Models.HomeModel.BasketModels> basket = null;
            if (Session["Basket"] == null)
            {
                basket = new List<Models.HomeModel.BasketModels>();
            }
            else
            {
                basket = (List<Models.HomeModel.BasketModels>)Session["Basket"];
            }

            if (basket.Any(x => x.Product.Id == id))
            {
                var pro = basket.FirstOrDefault(x => x.Product.Id == id);
                if (remove)
                {
                    pro.Count -= 1;
                }
                else
                {
                    if (pro.Product.Stok > pro.Count)
                    {
                        pro.Count += 1;
                    }
                    else
                    {
                        TempData["MyError"] = "Yeterli Stok Yok";
                    }
                }

            }
            else
            {
                var pro = Context.Products.FirstOrDefault(x => x.Id == id);
                if (pro != null && pro.SatıştaMı && pro.Stok > 0)
                {
                    basket.Add(new Models.HomeModel.BasketModels()
                    {
                        Count = 1,
                        Product = pro
                    });
                }
                else if (pro != null && pro.SatıştaMı == false)
                {
                    TempData["MyError"] = "Bu ürünün satışı durduruldu.";
                }
            }
            basket.RemoveAll(x => x.Count < 1);
            Session["Basket"] = basket;
            return RedirectToAction("Basket", "Home");
        }
        [HttpGet]
        public ActionResult Basket()
        {
            List<Models.HomeModel.BasketModels> model = (List<Models.HomeModel.BasketModels>)Session["Basket"];
            if (model == null)
            {
                model = new List<Models.HomeModel.BasketModels>();
            }
            if (IsLogon())
            {
                int currentId = CurrentUserId();
                ViewBag.CurrentAdres = Context.Adres
                    .Where(x => x.Member_Id == currentId)
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Baslik,
                        Value = x.Id.ToString()
                    }).ToList();
            }
            ViewBag.TotalPrice = model.Select(x => x.Product.Fiyat * x.Count).Sum();
            return View(model);
        }
        [HttpGet]
        public ActionResult RemoveBasket(int id)
        {
            List<Models.HomeModel.BasketModels> basket = (List<Models.HomeModel.BasketModels>)Session["Basket"];
            if (basket != null)
            {
                if (id > 0)
                {
                    basket.RemoveAll(x => x.Product.Id == id);
                }
                else if (id == 0)
                {
                    basket.Clear();
                }
                Session["Basket"] = basket;
            }
            return RedirectToAction("Basket", "Home");
        }
        [HttpPost]
        public ActionResult Buy(string Adres)
        {
            if (IsLogon())
            {
                try
                {
                    var basket = (List<Models.HomeModel.BasketModels>)Session["Basket"];
                    var guid = new Guid(Adres);
                    var _address = Context.Adres.FirstOrDefault(x => x.Id == guid);
                    //Sipariş Verildi = SV
                    //Ödeme Bildirimi = OB
                    //Ödeme Onaylandı = OO

                    var order = new Models.CodeFirstVT.Orders()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        Adres = _address.Tanım,
                        Member_Id = CurrentUserId(),
                        Durum = "SV",
                        Id = Guid.NewGuid()
                    };
                    //5
                    //ahmet 5
                    //mehmet 5
                    foreach (Models.HomeModel.BasketModels item in basket)
                    {
                        var oDetail = new Models.CodeFirstVT.OrderDetails();
                        oDetail.OlusturmaTarihi = DateTime.Now;
                        oDetail.Fiyat = item.Product.Fiyat * item.Count;
                        oDetail.Product_Id = item.Product.Id;
                        oDetail.AlımAdet = item.Count;
                        oDetail.Id = Guid.NewGuid();

                        order.OrderDetails.Add(oDetail);

                        var _product = Context.Products.FirstOrDefault(x => x.Id == item.Product.Id);
                        if (_product != null && _product.Stok >= item.Count)
                        {
                            _product.Stok = _product.Stok - item.Count;
                        }
                        else
                        {
                            throw new Exception(string.Format("{0} ürünü için yeterli stok yoktur veya silinmiş bir ürünü almaya çalışıyorsunuz.", item.Product.UrunAdi));
                        }
                    }
                    Context.Orders.Add(order);
                    Context.SaveChanges();
                    Session["Basket"] = null;
                }
                catch (Exception ex)
                {
                    TempData["MyError"] = ex.Message;
                }
                return RedirectToAction("Buy", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Hesap");
            }

        }
        [HttpGet]
        public ActionResult Buy()
        {
            if (IsLogon())
            {
                var currentId = CurrentUserId();
                IQueryable<Models.CodeFirstVT.Orders> orders;
                if (((int)CurrentUser().MemberType) > 8)
                {
                    orders = Context.Orders.Where(x => x.Durum == "OB");
                }
                else
                {
                    orders = Context.Orders.Where(x => x.Member_Id == currentId);
                }

                List<Models.HomeModel.BuyModels> model = new List<BuyModels>();
                foreach (var item in orders)
                {
                    var byModel = new BuyModels();
                    byModel.TotalPrice = item.OrderDetails.Sum(y => y.Fiyat);
                    byModel.OrderName = string.Join(", ", item.OrderDetails.Select(y => y.Product.UrunAdi + "(" + y.AlımAdet + ")"));
                    byModel.OrderStatus = item.Durum;
                    byModel.OrderId = item.Id.ToString();
                    byModel.Member = item.Members;
                    model.Add(byModel);
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Hesap");
            }
        }
        [HttpPost]
        [MyAutharization]
        public JsonResult OrderNotification(OrderNotificationModel model)
        {
            if (string.IsNullOrEmpty(model.OrderId) == false)
            {
                var guid = new Guid(model.OrderId);
                var order = Context.Orders.FirstOrDefault(x => x.Id == guid);
                if (order != null)
                {
                    order.Aciklama = model.OrderDescription;
                    order.Durum = "OB";
                    Context.SaveChanges();
                }
            }
            return Json("");
        }

        [HttpGet]
        //[HttpPost]
        public JsonResult GetProductDes(int id)
        {
            var pro = Context.Products.FirstOrDefault(x => x.Id == id);
            return Json(pro.UrunAcıklama, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetOrder(string id)
        {
            var guid = new Guid(id);
            var order = Context.Orders.FirstOrDefault(x => x.Id == guid);

            return Json(new {
                Acıklama = order.Aciklama,
                Adress=order.Adres
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [MyAutharization]
        public JsonResult OrderCompilete(string id, string text)
        {
            var guid = new Guid(id);
            var order = Context.Orders.FirstOrDefault(x => x.Id == guid);
            order.Aciklama = text;
            order.Durum = "OO";
            Context.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
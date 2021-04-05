using ETicaretSitesi.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    [MyAutharization(_membertype: 8)]
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
        
            var products = Context.Products.Where(x => x.İsDeleted == false || x.İsDeleted == null).ToList();
            return View(products.OrderByDescending(x=>x.OlusturmaTarihi).ToList());
        }
        [MyAutharization(_membertype:8)]
        public ActionResult Edit(int id = 0)
        {
            var product = Context.Products.FirstOrDefault(x => x.Id == id);
            var categories = Context.Categories.Select(x => new SelectListItem()
            {
                Text = x.Adi,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.Categories = categories;
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Models.CodeFirstVT.Product product)
        {
            var productImagePath = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    var folder = Server.MapPath("~/images/uploads/Product");
                    var filename = Guid.NewGuid() + ".jpg";
                    file.SaveAs(Path.Combine(folder, filename));

                    var filePath = "images/uploads/Product/" + filename;
                    productImagePath = filePath;
                }

            }
            if (product.Id > 0)
            {
                var dbProduct = Context.Products.FirstOrDefault(x => x.Id == product.Id);
                dbProduct.Category_Id = product.Category_Id;
                dbProduct.GuncellemeTarihi = DateTime.Now;
                dbProduct.UrunAcıklama = product.UrunAcıklama;
                dbProduct.SatıştaMı = product.SatıştaMı;
                dbProduct.UrunAdi = product.UrunAdi;
                dbProduct.Fiyat = product.Fiyat;
                dbProduct.Stok = product.Stok;
                dbProduct.İsDeleted = false;
                if (string.IsNullOrEmpty(productImagePath) == false)
                {
                    dbProduct.UrunResmi = productImagePath;
                }
            }
            else
            {
                product.OlusturmaTarihi = DateTime.Now;
                product.İsDeleted = false;
                product.UrunResmi = productImagePath;
                Context.Entry(product).State = System.Data.Entity.EntityState.Added;
            }

            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var pro = Context.Products.FirstOrDefault(x => x.Id == id);
            pro.İsDeleted = true;
            Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
using ETicaretSitesi.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    
    public class GaleriController : BaseController
    {
      
        // GET: Galeri
        public ActionResult Index(int id = 0)
        {
            return View(Context.Galeris.ToList());
        }
        public ActionResult Slider()
        {
            return View(Context.Galeris.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Models.CodeFirstVT.Galeri galeri, HttpPostedFileBase resim)
        {
            
            if (ModelState.IsValid)
            {
                if (resim != null)
                {
                    WebImage img = new WebImage(resim.InputStream);
                    FileInfo resiminfo = new FileInfo(resim.FileName);
                    string newresim = Guid.NewGuid().ToString() + resiminfo.Extension;
                    img.Save("~/images/uploads/Galeri/" + newresim);
                    galeri.ResimUrl = "/images/uploads/Galeri/" + newresim;

                }

                Context.Galeris.Add(galeri);//galeri tablosuna gelen galeri modelini ekle
                Context.SaveChanges();
                return RedirectToAction("/Index");//ekleme işlemi başarılı ise mevcut controllerın ındex sayfasına dön
            }
            return View(galeri);
        }
       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var r = Context.Galeris.Find(id);//galeriden seçtiğimiz resmin id sini bul
            if (r == null)
            {
                return HttpNotFound();
            }
            Context.Galeris.Remove(r);//bulduğun id yi kaldır
            Context.SaveChanges();
            return RedirectToAction("/Index");
        }

    }
}
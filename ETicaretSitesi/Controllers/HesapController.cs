using ETicaretSitesi.Filter;
using ETicaretSitesi.Models.CodeFirstVT;
using ETicaretSitesi.Models.EntityFramework;
using ETicaretSitesi.Models.Hesap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    public class HesapController : BaseController
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Models.Hesap.RegisterModels user)
        {
            try
            {
                if (user.RePasword != user.Member.Password)
                {
                    throw new Exception("Şifreler Aynı Değil");
                }
                if (Context.Members.Any(x => x.Email == user.Member.Email))
                {
                    throw new Exception("Bu E-posta Zaten Kayıtlı");
                }
                user.Member.MemberType = Models.MemberTypeEnum.Customer;
                user.Member.OlusturmaTarihi = DateTime.Now;
                Context.Members.Add(user.Member);
                Context.SaveChanges();
                return RedirectToAction("Login", "Hesap");
            }
            catch (Exception ex)
            {
                ViewBag.ReError = ex.Message;
                return View();
            }


        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.Hesap.LoginModel model)
        {
            try
            {
                var user = Context.Members.FirstOrDefault(x => x.Password == model.Member.Password && x.Email == model.Member.Email);
                if (user != null)
                {
                    Session["LogonUser"] = user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ReError = "Kullanıcı Bilgileri Yanlış";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ReError = ex.Message;
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["Logonuser"] = null;
            Session["Basket"] = null;
            return RedirectToAction("Login", "Hesap");
        }
        [HttpGet]
        public ActionResult Profil(int id = 0, string ad = "")
        {
            List<Adres> adresses = null;
            Adres currentAdres = new Adres();
            if (id == 0)
            {
                id = base.CurrentUserId();
                adresses = Context.Adres.Where(x => x.Member.Id == id).ToList();
                if (string.IsNullOrEmpty(ad)==false)
                {
                    var guid = new Guid(ad);
                    currentAdres = Context.Adres.FirstOrDefault(x => x.Id == guid );
                }
                else
                {

                }
            }
            var user = Context.Members.FirstOrDefault(x => x.Id == id);
            if (user == null) return RedirectToAction("Index", "Home");
            ProfilModels model = new ProfilModels()
            {
                Members = user,
                Adres = adresses,
                CurrentAdres = currentAdres
            };
            return View(model);
        }
        [HttpGet]
        [MyAutharization]
        public ActionResult ProfilEdit()
        {
            int id = base.CurrentUserId();
            var user = Context.Members.FirstOrDefault(x => x.Id == id);
            if (user == null) return RedirectToAction("Index", "Home");
            ProfilModels model = new ProfilModels()
            {
                Members = user
            };
            return View(model);
        }
        [HttpPost]
        [MyAutharization]
        public ActionResult ProfilEdit(ProfilModels model)
        {
            try
            {
                int id = CurrentUserId();
                var updateMember = Context.Members.FirstOrDefault(x => x.Id == id);
                updateMember.OlusturmaTarihi = DateTime.Now;
                updateMember.Biografi = model.Members.Biografi;
                updateMember.Adı = model.Members.Adı;
                updateMember.Soyadı = model.Members.Soyadı;
                if (string.IsNullOrEmpty(model.Members.Password) == false)
                {
                    updateMember.Password = model.Members.Password;
                }
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        var folder = Server.MapPath("~/images/uploads/");
                        var filename = Guid.NewGuid() + ".jpg";
                        file.SaveAs(Path.Combine(folder, filename));

                        var filePath = "~/images/uploads/" + filename;
                        updateMember.ProfilResmi = filePath;
                    }

                }
                Context.SaveChanges();

                return RedirectToAction("Profil", "Hesap");
            }
            catch (Exception ex)
            {
                ViewBag.MyError = ex.Message;
                int id = CurrentUserId();
                var viewmodel = new Models.Hesap.ProfilModels()
                {
                    Members = Context.Members.FirstOrDefault(x => x.Id == id)
                };
                return View(viewmodel);
            }

        }
        [HttpPost]
        [MyAutharization]
        public ActionResult Adres(Adres adres)
        {
            Adres _adres = null;
            if (adres.Id == Guid.Empty)
            {
                adres.Id = Guid.NewGuid();
                adres.OlusturmaTarihi = DateTime.Now;
                adres.Member_Id = base.CurrentUserId();
                Context.Adres.Add(adres);
            }
            else
            {
                _adres = Context.Adres.FirstOrDefault(x => x.Id == adres.Id);
                _adres.GuncellemeTarihi = DateTime.Now;
                _adres.Baslik = adres.Baslik;
                _adres.Tanım = adres.Tanım;
            }
            Context.SaveChanges();
            return RedirectToAction("Profil", "Hesap");
        }

        [HttpGet]
        [MyAutharization]
        public ActionResult RemoveAdres(string id)
        {
            var guid = new Guid(id);
            var adres = Context.Adres.FirstOrDefault(x => x.Id == guid);
            Context.Adres.Remove(adres);
            Context.SaveChanges();
            return RedirectToAction("Profil", "Hesap");
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var member = Context.Members.FirstOrDefault(x => x.Email == email);
            if (member==null)
            {
                ViewBag.MyError = "Böyle Bir Hesap Bulunamadı";
                return View();
            }
            else
            {
                var body = "Şifreniz : " + member.Password;
                MyMail mail = new MyMail(member.Email, "Şifremi Unuttum",body);
                mail.SendMail();
                TempData["Info"] = email + "Mail adresinize şifreniz gönderilmiştir.";
                return RedirectToAction("Login");
            }
            
        }
    }
}
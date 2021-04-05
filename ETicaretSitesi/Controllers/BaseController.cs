using ETicaretSitesi.Models.CodeFirstVT;
using ETicaretSitesi.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    public class BaseController : Controller
    {
        protected DataBaseContex Context { get; private set; }
        public BaseController()
        {
            Context = new DataBaseContex();
            ViewBag.MenuCategories = Context.Categories.Where(x => x.ParentId == null).ToList();
        }
        protected Members CurrentUser()
        {
            if (Session["LogonUSer"] == null) return null;
            return (Members)Session["LogonUser"];
        }
        protected int CurrentUserId()
        {
            if (Session["LogonUSer"] == null) return 0;
            return ((Members)Session["LogonUser"]).Id;
        }
        protected bool IsLogon()
        {
            if (Session["LogonUSer"] == null)
                return false;
            else
                return true;

        }
        // Tüm Alt Kategorileri Getirdim 
        protected List<Category> GetChildCategories(Category cat)
        {
            var result = new List<Category>();

            result.AddRange(cat.Categories1);
            foreach (var item in cat.Categories1)//alt kategorilerinde altını almak için dönüyorum.
            {
                var list = GetChildCategories(item);
                result.AddRange(list);
            }
            return result;
        }
    }
}
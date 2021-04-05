using ETicaretSitesi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    [MyAutharization(_membertype: 8)]
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {       
            var categories = Context.Categories.Where(x => x.İsDeleted == false || x.İsDeleted == null).ToList();
            return View(categories.OrderByDescending(x => x.OlusturmaTarihi).ToList());
        }

        public ActionResult Edit(int id = 0)
        {
           
            var cat = Context.Categories.FirstOrDefault(x => x.Id == id);
            var categories = Context.Categories.Select(x => new SelectListItem()
            {
                Text = x.Adi,
                Value = x.Id.ToString()
            }).ToList();
            categories.Add(new SelectListItem()
            {
                Value = "0",
                Text = "Ana Kategori",
                Selected = true
            });
            ViewBag.Categories = categories;
            return View(cat);
        }

        [HttpPost]
        public ActionResult Edit(Models.CodeFirstVT.Category category)
        {
            if (category.Id>0)
            {
                var cat = Context.Categories.FirstOrDefault(x => x.Id == category.Id);
                cat.Aciklama = category.Aciklama;
                cat.Adi = category.Adi;
                cat.GuncellemeTarihi = DateTime.Now;
                cat.İsDeleted = false;
                if (category.ParentId>0)                
                    cat.ParentId = category.Categories2.ParentId;                
                else                
                    cat.ParentId = null;
                
            }
            else
            {
                category.OlusturmaTarihi = DateTime.Now;
                category.İsDeleted = false;
                if (category.ParentId == 0)
                    category.ParentId = null;
                Context.Entry(category).State = System.Data.Entity.EntityState.Added;              
            }
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var cat = Context.Categories.FirstOrDefault(x => x.Id == id);
            cat.İsDeleted = true;
            Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
using ETicaretSitesi.Filter;
using ETicaretSitesi.Models.Mesaj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Controllers
{
    [MyAutharization]
    public class MesajController : BaseController
    {
        // GET: Mesaj
        [HttpGet]
        public ActionResult Index()
        {
            
            var currentID = CurrentUserId();
            Models.Mesaj.IndexModel model = new Models.Mesaj.IndexModel();
            #region -Select List İtem-
            model.User = new List<SelectListItem>();
            var user = Context.Members.Where(x => ((int)x.MemberType) > 0 && x.Id != currentID).ToList();
            model.User = user.Select(x => new SelectListItem()
            {

                Value = x.Id.ToString(),
                Text = string.Format("{0} {1} ({2})", x.Adı, x.Soyadı, x.MemberType.ToString())

            }).ToList();
            #endregion
            #region -Mesaj Listesi-
            var mList = Context.Mesajs.Where(x => x.Member_To_Id == currentID || x.MesajYanıtlarıs.Any(y => y.Member_Id == currentID)).ToList();//Mesaj bana geldiyse benim gönderdiğim mesaj varsa listele.
            model.Messages = mList;
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult SendMesaj(SendMesajModel message)
        {
           

            Models.CodeFirstVT.Message mesaj = new Models.CodeFirstVT.Message()
            {
                Id = Guid.NewGuid(),
                OlusturmaTarihi = DateTime.Now,
                OkunduMu = false,
                Konu = message.Subject,
                Member_To_Id = message.ToUserId
            };
            var mRep = new Models.CodeFirstVT.MessageReplies()
            {
                Id = Guid.NewGuid(),
                OlusturmaTarihi = DateTime.Now,
                Member_Id = CurrentUserId(),
                Text = message.MessageBody
            };
            mesaj.MesajYanıtlarıs.Add(mRep);
            Context.Mesajs.Add(mesaj);
            Context.SaveChanges();
            return RedirectToAction("Index", "Mesaj");
        }
        [HttpGet]
        public ActionResult MessageReplies(string id)
        {
           
            var currentId = CurrentUserId();
            var guid = new Guid(id);
            Models.CodeFirstVT.Message message = Context.Mesajs.FirstOrDefault(x => x.Id == guid);
            if (message.Member_To_Id==currentId)
            {
                message.OkunduMu = true;
                Context.SaveChanges();
            }
            MessageRepliesModel model = new MessageRepliesModel();            
            model.MReplies = Context.MesajYanıtlarıs.Where(x => x.Message_Id == guid).OrderBy(x=>x.OlusturmaTarihi).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult MessageReplies(Models.CodeFirstVT.MessageReplies message)
        {
           

            message.OlusturmaTarihi = DateTime.Now;
            message.Id = Guid.NewGuid();
            message.Member_Id = CurrentUserId();

            Context.MesajYanıtlarıs.Add(message);
            Context.SaveChanges();

            return RedirectToAction("MessageReplies", "Mesaj", new { id = message.Message_Id });
        }
        [HttpGet]
        public ActionResult RenderMessage()
        {
            RenderMessageModel model = new RenderMessageModel();
            var currentID = CurrentUserId();
            var mList = Context.Mesajs.Where(x => x.Member_To_Id == currentID || x.MesajYanıtlarıs.Any(y => y.Member_Id == currentID))
                .OrderByDescending(x=>x.OlusturmaTarihi);
            model.Messages = mList.Take(4).ToList();
            model.Count = mList.Count();
            return PartialView("_Message", model);
        }

        public ActionResult RemoveMessageReplies(string id)
        {
            var guid = new Guid(id);
            //Mesaj Silindi.
            var mReplies = Context.MesajYanıtlarıs.Where(x => x.Message_Id == guid);
            Context.MesajYanıtlarıs.RemoveRange(mReplies);
            //Mesajın Kendisi Silindi
            var message = Context.Mesajs.FirstOrDefault(x => x.Id == guid);
            Context.Mesajs.Remove(message);

            Context.SaveChanges();
            return RedirectToAction("Index", "Mesaj");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.Mesaj
{
    public class IndexModel
    {
        public List<System.Web.Mvc.SelectListItem> User { get; set; }
        public List<ETicaretSitesi.Models.CodeFirstVT.Message> Messages { get; set; }
    }
}
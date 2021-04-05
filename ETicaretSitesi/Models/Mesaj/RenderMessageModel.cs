using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.Mesaj
{
    public class RenderMessageModel
    {
        public List<Models.CodeFirstVT.Message> Messages { get; set; }
        public int Count { get; set; }
    }
}
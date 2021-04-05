using ETicaretSitesi.Models.CodeFirstVT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.HomeModel
{
    public class CommentsModel
    {
        public List<Comments> Comments { get; set; }
        public Product Product { get; set; }
    }
}
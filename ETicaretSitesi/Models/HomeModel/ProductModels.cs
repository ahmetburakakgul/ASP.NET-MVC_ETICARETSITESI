using ETicaretSitesi.Models.CodeFirstVT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.HomeModel
{
    public class ProductModels
    {
        public Product Product { get; set; }
        public List<Comments> Comments { get; set; }
    }
}
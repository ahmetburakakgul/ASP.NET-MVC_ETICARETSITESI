using ETicaretSitesi.Models.CodeFirstVT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.HomeModel
{
    public class HomeModel
    {
        public List<Product> Product { get; set; }
        public Category Category { get; set; }
    }
}
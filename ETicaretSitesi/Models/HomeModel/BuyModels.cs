using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.HomeModel
{
    public class BuyModels
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public CodeFirstVT.Members Member { get; set; }
    }
}
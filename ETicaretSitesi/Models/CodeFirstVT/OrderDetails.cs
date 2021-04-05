using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class OrderDetails
    {
        [Key]
        public Guid Id { get; set; }

        public decimal Fiyat { get; set; }

        public int AlımAdet { get; set; }

        public int? İndirim { get; set; }

        
        public DateTime OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }

        public int Product_Id { get; set; }

        public virtual Product Product { get; set; } 

        public Guid Order_Id { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
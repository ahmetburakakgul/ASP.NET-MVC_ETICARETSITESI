using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Product : BaseEntity
    {
        public Product()
        {

            Comments = new HashSet<Comments>();

            OrderDetails = new HashSet<OrderDetails>();

        }
        [Required,StringLength(30)]
        public string UrunAdi { get; set; }
        [StringLength(300)]
        public string UrunAcıklama { get; set; }
        [DataType(DataType.Currency)]
        public decimal Fiyat { get; set; }
        
        public string UrunResmi { get; set; }

        public bool SatıştaMı { get; set; }

        public int Stok { get; set; }

        public int YıldızVerenKullanıcı { get; set; }

        public int YıldızSayısı { get; set; }

        public int Category_Id { get; set; }

        public bool? İsDeleted { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
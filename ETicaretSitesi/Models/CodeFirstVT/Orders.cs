using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Orders
    {
        public Orders()
        {

            OrderDetails = new HashSet<OrderDetails>();

        }

        [Key]        
        public Guid Id { get; set; }

        [StringLength(12)]
        public string Numara { get; set; }

        [Required,StringLength(50)]
        public string Durum { get; set; }

        [StringLength(250)]
        public string Aciklama { get; set; }

        public int Member_Id { get; set; }
        public virtual Members Members { get; set; }

        [Required]
        public string  Adres { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
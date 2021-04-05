using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Message 
    {
        public Message()
        {

            MesajYanıtlarıs = new HashSet<MessageReplies>();

        }
        [Key]
        public Guid Id { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }

        [Required,StringLength(300)]
        public string Konu { get; set; }

        public bool OkunduMu { get; set; }
       
        public int Member_To_Id { get; set; }
        public virtual Members Members { get; set; }

        public virtual ICollection<MessageReplies> MesajYanıtlarıs { get; set; }
    }
}
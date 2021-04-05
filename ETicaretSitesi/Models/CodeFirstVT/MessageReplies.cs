using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class MessageReplies 
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }

        [Required,StringLength(300)]
        public string Text { get; set; }

        public bool OkunduMu { get; set; }

        public Guid Message_Id { get; set; }
        public virtual Message Message { get; set; }

        public int Member_Id { get; set; }
        public virtual Members Members { get; set; }

        

    }
}
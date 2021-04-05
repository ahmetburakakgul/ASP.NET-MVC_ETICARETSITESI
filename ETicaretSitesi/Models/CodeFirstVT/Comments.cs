using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Comments : BaseEntity
    {
        [Required,StringLength(300)]
        public string Text { get; set; }

        public int Member_Id { get; set; }
        public virtual Members Members { get; set; }

        public int Product_Id { get; set; }
        public virtual Product Product { get; set; }
    }
}
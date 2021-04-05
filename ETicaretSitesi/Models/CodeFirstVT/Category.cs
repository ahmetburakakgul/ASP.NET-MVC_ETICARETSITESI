using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Category : BaseEntity
    {
        [Required,StringLength(50)]
        public string Adi { get; set; }
        [StringLength(250)]
        public string Aciklama { get; set; }
        [ForeignKey("Categories1")]
        public int? ParentId { get; set; }

        public bool? İsDeleted { get; set; }

        public virtual ICollection<Category> Categories1 { get; set; }

        public virtual Category Categories2 { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Categories1 = new HashSet<Category>();
            Products = new List<Product>();
        }
    }
}
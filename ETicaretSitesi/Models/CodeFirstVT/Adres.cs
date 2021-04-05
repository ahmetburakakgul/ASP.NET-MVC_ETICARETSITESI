using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Adres 
    {
        [Key]
        public Guid Id { get; set; }

        [Required,StringLength(50)]
        public string Baslik { get; set; }

        [Required,StringLength(250)]
        public string Tanım { get; set; }

        [Required]
        public DateTime OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }
        
        public int Member_Id { get; set; }



        public virtual Members Member { get; set; }

       


    }
}
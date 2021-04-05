using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    public class Galeri 
    {
        [Key]
        public int ResimId { get; set; }

        public string ResimUrl { get; set; }
    }
}
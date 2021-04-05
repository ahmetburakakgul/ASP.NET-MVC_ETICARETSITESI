using ETicaretSitesi.Models.CodeFirstVT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.Hesap
{
    public class ProfilModels
    {
        public Members Members { get; set; }
        public List<Adres> Adres { get; set; }
        public Adres CurrentAdres { get; set; }
    }
}
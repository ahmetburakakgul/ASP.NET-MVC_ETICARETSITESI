using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.CodeFirstVT
{
    
    public class Members : BaseEntity
    {
        public Members()
        {

            Adres = new HashSet<Adres>();

            Comments = new HashSet<Comments>();

            MesajYanıtlarıs = new HashSet<MessageReplies>();

            Mesajs = new HashSet<Message>();

            Orders = new HashSet<Orders>();

        }


        [StringLength(25)]
        public string Adı { get; set; }
        [StringLength(25)]
        public string Soyadı { get; set; }
        [Required, StringLength(60)]
        public string Email { get; set; }
        [StringLength(300)]
        public string Biografi { get; set; }
        [Required, StringLength(60)]
        public string Password { get; set; }
        [StringLength(150)]
        public string ProfilResmi { get; set; }

        public MemberTypeEnum MemberType { get; set; }


       

        public virtual ICollection<Adres> Adres { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<MessageReplies> MesajYanıtlarıs { get; set; }
        public virtual ICollection<Message> Mesajs { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }

}
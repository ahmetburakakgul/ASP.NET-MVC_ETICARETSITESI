using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.Mesaj
{
    public class SendMesajModel
    {
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public int ToUserId { get; set; }
    }
}
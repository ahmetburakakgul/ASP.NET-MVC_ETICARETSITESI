using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaretSitesi.Filter
{
    public class MyAutharizationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public int ActionMemberType { get; private set; }
        public MyAutharizationAttribute()
        {

        }
        public MyAutharizationAttribute(int _membertype)
        {
            this.ActionMemberType = _membertype;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var member = (Models.CodeFirstVT.Members)HttpContext.Current.Session["LogonUser"];
            if (member == null)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
            }
            else
            {
                var memberType = (int)member.MemberType;
                if (memberType < ActionMemberType)
                {
                    filterContext.Result = new RedirectResult("/Home/Index");
                }
               
            }
        }
    }
}
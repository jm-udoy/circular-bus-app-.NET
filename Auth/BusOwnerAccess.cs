using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Circular_Bus_App.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BusOwnerAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authenticated = base.AuthorizeCore(httpContext);

            if (!authenticated)
            {
                return false;
            }
            if (httpContext.Session["role"].ToString().Equals("BusOwner  "))
            {
                return true;
            }
            return false;
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GastroByte.Utilities
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly int[] allowedRoles;

        public AuthorizeRoleAttribute(params int[] roles)
        {
            this.allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = (int?)filterContext.HttpContext.Session["UserRole"];

            if (userRole == null || !allowedRoles.Contains(userRole.Value))
            {
                filterContext.Result = new RedirectResult("~/Usuario/Login");
            }

            base.OnActionExecuting(filterContext);
        }
    }

}
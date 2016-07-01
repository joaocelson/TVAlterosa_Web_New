using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campeonato.UI.WEB.Security
{
    public class PermissoesFiltro : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext fiterContext)
        {
            base.OnAuthorization(fiterContext);
            if (fiterContext.Result is HttpUnauthorizedResult)
                fiterContext.HttpContext.Response.Redirect("~/Admin/AdminHome/Login");
        }
    }
}
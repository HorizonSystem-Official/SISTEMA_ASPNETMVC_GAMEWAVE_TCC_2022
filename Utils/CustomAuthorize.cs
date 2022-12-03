using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using System.Threading;
using System.Web.Configuration;


namespace TCC_Sistema_Cliente_Jogos_2022.Utils
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorize(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var role in allowedroles)
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                try
                {
                    String userName = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                                      .Select(c => c.Value).SingleOrDefault();

                    if (role == "Cliente")
                    {
                        authorize = new Cliente().isCli(userName);
                    }
                    else if (role == "Funcionario")
                    {
                        authorize = new Funcionario().isFunc(userName);
                    }

                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/_Layout_Acesso_Restrito.cshtml"
            };
        }

    }
}
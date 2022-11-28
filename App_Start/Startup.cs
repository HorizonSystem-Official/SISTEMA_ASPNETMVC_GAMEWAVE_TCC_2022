using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(TCC_Sistema_Cliente_Jogos_2022.App_Start.Startup))]

namespace TCC_Sistema_Cliente_Jogos_2022.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "AppAplicationCookie",

                //Alterado de / Home / Index para / Login / Index

                LoginPath = new PathString("/Login/Login")
            });

            //Utilizado para resolver o antiforgerytoken nos formulários
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "CPF";
        }
    }
}

using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(TCC_Sistema_Cliente_Jogos_2022.App_Start.Startup))]

namespace TCC_Sistema_Cliente_Jogos_2022.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,

                //Alterado de / Home / Index para / Login / Index

                LoginPath = new PathString("/Login/Login"),
                LogoutPath = new PathString("/Login/Logout")
            });

            //Utilizado para resolver o antiforgerytoken nos formulários
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "CPF";
        }
    }
}

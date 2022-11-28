using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TCC_Sistema_Cliente_Jogos_2022
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "RotaComentarios",
            //    url: "Comentario/{action}/{codprod}",
            //    new { controller = "Comentario", action = "MostraComentarios", codprod = UrlParameter.Optional }
            //     );
        }
    }
}

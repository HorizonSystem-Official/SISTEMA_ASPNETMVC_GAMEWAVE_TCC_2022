using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        //INATIVO
        public ActionResult Index()
        {
            return View();
        }

        

    }
}
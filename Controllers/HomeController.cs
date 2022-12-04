using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using TCC_Sistema_Cliente_Jogos_2022.Utils;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    
    public class HomeController : Controller
    {
        
        public ActionResult Index(string spPesquia, string spProdTipo)
        {
            MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
            try
            {
                conexao.Open();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", new { mensagem = e.Message });
            }
            finally
            {
                if (conexao.State == ConnectionState.Open)
                    conexao.Close();
            }

            var produ = new Produto();

            //if (spPesquia == null && spProdTipo == null)
            //{


            //}
            if (spPesquia != null && spProdTipo == null)
            {
                var MostrarProdPeloNome = produ.ListarProdPeloNome(spPesquia);
                TempData["Pesquisa"] = spPesquia;
                return View(MostrarProdPeloNome);
            }
            if (spPesquia == null && spProdTipo != null)
            {
                var MostrarProdPeloTipo = produ.ListarProdPeloTipo(spProdTipo);

                return View(MostrarProdPeloTipo);
            }
            else
            {
                var MostrarProdTodos = produ.ListarProd();
                return View(MostrarProdTodos);
            }

        }

        public ActionResult Error(string mensagem)
        {
            ViewBag.error = "Entre em contato com um profissional TI e passe a seguinte mensagem: {" + mensagem + "}";
            return View();
        }



    }
}
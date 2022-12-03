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
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;
namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    public class ComentarioController : Controller
    {
        // GET: Comentario

        //REALIZA O CADASTRO DIRETO DO CUPOM E REDIRECIONA  PARA A PÁGINA DOS COMENTÁRIOS
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult CadComentario(DetalhesProdutoEComentarios comentar)
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

            var verificaCPF = comentar.Fk_CpfCli;

            if ( verificaCPF == null)
            {
                TempData["MensagemAviso"] = "Realize o login como Cliente para postar o comentário!";
                return RedirectToAction("DetalhesProduto", "Produto", new { codprod = comentar.CodProd});
            }

            var metodoscomentario = new Comentario();

            Comentario comentario = new Comentario
            {
                TxtComentario = comentar.TxtComentario,
                Fk_CodProd = comentar.CodProd,
                Fk_CpfCli = User.Identity.Name
            };

            metodoscomentario.CadastrarComen(comentario);
            //APÓS CADASTRAR O OBJETO, SERÁ EMITIDO UM AVISO NA TELA INFORMANDO DO COMENTÁRIO ADICIONADO
            TempData["MensagemAviso"] = "Seu comentário foi adicionado com sucesso!";
            return RedirectToAction("MostraComentarios", "Comentario", new { codprod = comentario.Fk_CodProd});
        }


        //SELECIONA TODOS OS COMENTÁRIOS POR UM PRODUTO ESPECÍFICO PELO SEU ID
        [HttpGet]
        public ActionResult MostraComentarios(int codprod)
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

            var ListaComentarios = new Comentario().ListarTodosComentarios(codprod);

            return View(ListaComentarios);
        }
    }
}
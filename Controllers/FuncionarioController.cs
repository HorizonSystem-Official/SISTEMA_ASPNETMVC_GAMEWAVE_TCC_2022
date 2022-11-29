using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;
using TCC_Sistema_Cliente_Jogos_2022.Utils;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    //[Authorize]
    public class FuncionarioController : Controller
    {
        

        // GET: Funcionario
        //FORMULÁRIO PARA ADICIONAR O FUNCIONÁRIO
        public ActionResult CadInsert()
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
            return View();
        }

        //MÉTODO PARA ADICIONAR O FUNCIONÁRIO
        [HttpPost]
        public ActionResult CadInsert(FuncionarioViewModel funcio)
        {
            if(!ModelState.IsValid)
            {
                return View(funcio);
            }
            //USA-SE O OBJETO PARA PEGAR TEMPORÁRIAMENTE OS DADOS DA VIEWMODEL
            Funcionario funcionario = new Funcionario
            {
                Nome = funcio.NomeFunc,
                DataNasc = funcio.DataNasc,
                CPF = funcio.CPF,
                Senha = Hash.GerarHash(funcio.Senha),
                Cargo = funcio.Cargo
            };
            funcionario.CriarFuncio(funcionario);

            //TEMPDATA PARA NOTIFICAR O USUÁRIO DO CADASTRO FEITO
            TempData["MensagemAviso"] = "Cadastro do Funcionário feito com sucesso!";

            return RedirectToAction("ConsulFuncio", "Funcionario");
        }

        //REALIZA A BUSCA DE TODOS OS FUNCIONÁRIOS
        public ActionResult ConsulFuncio(string CPFFun)
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
            var funcio = new Funcionario();

            if (CPFFun != null)
            {
                var MostrarFuncPeloCPF = funcio.ListarFuncioPeloCPF(CPFFun);

                return View(MostrarFuncPeloCPF);
            }
            var MostrarFuncio = funcio.ListarFuncio();

            return View(MostrarFuncio);
        }

        //COM O PARÂMETRO ID do funcionário enviado, É APAGADO O FUNCIONÁRIO SELECIONADO
        
        public ActionResult DelFuncio(int IdFunc)
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

            var funcio = new Funcionario();

            

            funcio.DeletFuncio(IdFunc);

            //AVISA O USUÁRIO DO FUNCIONÁRIO DELETADO E RETORNA PARA A CONSULTA DELES
            TempData["MensagemAviso"] = "Funcionário deletado com sucesso!";
            return RedirectToAction("ConsulFuncio");

        }

        //É PEGO O CPF DO FUNCIONÁRIO PARA FAZER AS ALTERAÇÕES
        [HttpGet]
        public ActionResult EdiFuncio(string CPF)
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

            Funcionario funcionario = new Funcionario();
            
            var umfuncionario = funcionario.ListaUMFuncio(CPF);

            if (umfuncionario == null)
                return HttpNotFound();

            return View(umfuncionario);
        }

        //REALIZA AS ALTERAÇÕES DO FUNCIONÁRIO PELO SEU OBJETO
        [HttpPost]
        public ActionResult EdiFuncio(Funcionario funcio)
        {
            if (!ModelState.IsValid)
            {
                
                return View(funcio);
                
            }
            Funcionario funcionario = new Funcionario();
            
            

            funcionario.AlterFuncio(funcio);

            //ENVIA UMA TEMPDATA PARA NOTIFICAR O USUÁRIO DO FUNCIONÁRIO ALTERADO E REDIRECIONA PARA A CONSULTA DELES
            TempData["MensagemAviso"] = "Alteração do Funcionário feito com sucesso!";

            return RedirectToAction("ConsulFuncio");
        }

        //FEITO PELO REMOTE, TEM FUNÇÃO DE NOTIFICAR A EXISTÊNCIA DE UM CPF DE UM FUNCIONÁRIO 
        public ActionResult verificaCPFFuncio(string vCPF)
        {
            //FEITA PELO REMOTE
            bool LoginExiste;
            string login = new Funcionario().verificaFuncioCPF(vCPF);

            if (login.Length == 0)
                LoginExiste = false;
            else
                LoginExiste = true;

            //RETORNA BOOLEANO
            return Json(!LoginExiste, JsonRequestBehavior.AllowGet);
        }

        //UMA VIEW PRESENTE NA SHARED PARA CASO OCORRER UM ERRO NA CONEXÃO COM O BANCO
        public ActionResult Error(string mensagem)
        {
            ViewBag.error = "Entre em contato com um profissional TI e passe a seguinte mensagem: {" + mensagem + "}";
            return View();
        }
    }
}
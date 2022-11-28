using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;
using TCC_Sistema_Cliente_Jogos_2022.Utils;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    //[Authorize]
    public class ClienteController : Controller
    {
        // GET: Cliente
        //REALIZA A CONSULTA DOS CLIENTES SEM FILTRO
        public ActionResult ConsulCli()
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
            var clien = new Cliente();

            var MostrarClien = clien.ListarCli();

            return View(MostrarClien);
        }

        //FORMULÁRIO PARA O CADASTRO DO CLIENTE
        public ActionResult CadCliente()
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

        //MÉTODO PARA CADASTRAR A MODEL POR UMA VIEWMODEL
        [HttpPost]
        public ActionResult CadCliente(ClienteViewModel clienviewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(clienviewmodel);
            }
            Cliente cliente = new Cliente
            {
                CPF = clienviewmodel.CPF,
                Nome = clienviewmodel.NomeCliente,
                DataNasc = clienviewmodel.DataNasc,
                Senha = Hash.GerarHash(clienviewmodel.Senha),
                EmailCli = clienviewmodel.EmailCli,
                TelCli = clienviewmodel.TelCli
            };

            cliente.CriarCliente(cliente);

            //EMITE UMA MENSAGEM PELO TEMPDATA DE SUCESSO E REDIRECIONA PARA A CONSULTA DELES
            TempData["MensagemAviso"] = "Cadastro do cliente feito com sucesso!";
            return RedirectToAction("ConsulCli", "Cliente");
        }


        //OCORRE O APAGAMENTO DO CLIENTE PELO SEU CPF
        public ActionResult DelCliente(string CPF)
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
            var cli = new Cliente();

            cli.CPF = CPF;

            //RECEBE PUXANDO UM OBJETO SEU PARA SEU ATRIBUTO CPF, REALIZANDO O DELETE
            cli.DeletCli(cli);


            //EMITE UMA TEMP PARA O USUÁRIO INFORMANDO DA AÇÃO FEITA E REDIRECIONA PARA A CONSULTA DOS CLIENTES
            TempData["MensagemAviso"] = "Cliente deletado com sucesso!";
            return RedirectToAction("ConsulCli", "Cliente");

        }


        //FORMULÁRIO PARA ALTERAR O CLIENTE ESPECÍFICO POR UM PARÂMETRO
        [HttpGet]
        public ActionResult EdiCliente(string CPF)
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

            Cliente cli = new Cliente();
            
            var umfuncionario = cli.ListaUMCliente(CPF);

            if (umfuncionario == null)
                return HttpNotFound();

            return View(umfuncionario);
        }

        //MÉTODO PARA REALIZAR A ALTERAÇÃO DO CUPOM
        [HttpPost]
        public ActionResult EdiCliente(Cliente cli)
        {
            if (!ModelState.IsValid)
            {
                return View(cli);
            }
            Cliente cliente = new Cliente();

            cliente.AlterCli(cli);

            //É ENVIADO UM OBJETO COM OS DADOS ALTERADOS E REDIRECIONA O USUÁRIO PARA A TABELA DE CONSULTA COM UMA NOTIFICAÇÃO DA TEMPDATA
            TempData["MensagemAviso"] = "Alteração do cliente feito com sucesso!";
            return RedirectToAction("ConsulCli", "Cliente");
        }


        //MÉTODO CRIADO PARA VERIFICAR A EXISTÊNCIA DE UM CPF DA TABELA CLIENTE
        public ActionResult verificaCPFCliente(string CPF)
        {
            //FEITA PELO REMOTE
            bool LoginExiste;
            string login = new Cliente().verificaClienCPF(CPF);

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
            ViewBag.error = "Entre em contato com um profissional TI e passe a seguinte mensagem: {"+mensagem+"}";
            return View();
        }
    }
}
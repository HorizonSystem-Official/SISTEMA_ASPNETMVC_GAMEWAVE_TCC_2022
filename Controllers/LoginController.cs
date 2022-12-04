using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;
using TCC_Sistema_Cliente_Jogos_2022.Utils;
using System.Security.Claims;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Security.Principal;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        //PÁGINA "HOME" QUE VAI APRESENTAR TODOS OS PRODUTOS SEM FILTRO PRIMEIRAMENTE
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
            } if (spPesquia == null && spProdTipo != null)
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

        //FORMULÁRIO PARA AUTENTICAÇÃO DO FUNCIONÁRIO
        public ActionResult Login(string RetornaURL)
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

            var identidade = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var CPFName = identidade.Claims.Where(c => c.Type == ClaimTypes.Name)
                                                      .Select(c => c.Value).SingleOrDefault();

            //UTILIZADO COMO GUIA DA URL ENQUANTO O USUÁRIO ESTIVER LOGADO
            var viewmodel = new LoginViewModel
            {
                UrlRetorno = RetornaURL
            };
            return View(viewmodel);
            
        }


        
        [HttpPost]
        public ActionResult Login(LoginViewModel loginviewmodel)
        {
            if (!ModelState.IsValid)
                return View(loginviewmodel);

            
            
            Funcionario funcionario = new Funcionario();

            Cliente cliente = new Cliente();

            int idFunc = funcionario.VerificaFuncioIdExiste(loginviewmodel.CPFLogin, loginviewmodel.SenhaLogin);
            if (idFunc > 0)
            {
                var leituraFunc = new Funcionario().ListaUMFuncioLOGIN(loginviewmodel.CPFLogin);
                this.EntraUser(leituraFunc.CPF, false);
                TempData["MensagemAviso"] = "Login realizado com sucesso! Bem-vindo:" + leituraFunc.Nome;

                if (Url.IsLocalUrl(loginviewmodel.UrlRetorno))
                    return Redirect(loginviewmodel.UrlRetorno);
                else
                    return RedirectToAction("Index");
            }
            else
            {
                bool existeCliente = cliente.VerificaClienteExiste(loginviewmodel.CPFLogin, loginviewmodel.SenhaLogin);

                if (existeCliente == true)
                {
                    var tempFunc = new Cliente().ListaUMClienteLOGIN(loginviewmodel.CPFLogin);
                    this.EntraUser(tempFunc.CPF, false);
                    TempData["MensagemAviso"] = "Login realizado com sucesso! Bem-vindo:" + tempFunc.Nome;
                    if (Url.IsLocalUrl(loginviewmodel.UrlRetorno))
                        return Redirect(loginviewmodel.UrlRetorno);
                    else
                        return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("SenhaLogin", "CPF ou Senha inválidos!");
                }

            }
            //É ENVIADO O PARÂMETRO ÚNICO PARA BUSCAR O REGISTRO NO BD
            //funcionario = funcionario.ListaUMFuncioLOGIN(loginviewmodel.CPFLogin);

            //SE CASO NÃO ACHAR O REGISTRO PELO CPF, RETORNA UM MODELERROR
            //if (funcionario == null | funcionario.CPF != loginviewmodel.CPFLogin)
            //{
            //    ModelState.AddModelError("CPFLogin", "CPF Incorreto");
            //    return View(loginviewmodel);
            //}
            //SE ACHAR O REGISTRO, MAS A SENHA TIRAR DIFERENTE, RETORNA UM MODELERROR
            //if (funcionario.Senha != Hash.GerarHash(loginviewmodel.SenhaLogin))
            //{
            //    ModelState.AddModelError("SenhaLogin", "Senha incorreta");
            //    return View(loginviewmodel);
            //}
            //CRIA A IDENTIFICACAO DO USUÁRIO PELO CLAIMSIDENTITY USANDO O CPF ÚNICO
            //var identificacao = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.Name, funcionario.CPF),
            //    new Claim("CPF", funcionario.CPF)
            //}, "AppAplicationCookie");

            //FAZ A AUTENTICAÇÃO PELO OWIN
            //Request.GetOwinContext().Authentication.SignIn(identificacao);
            //TempData["MensagemAviso"] = "Funcionário autenticado com sucesso!";
            //CASO A URL ESTIVER INATIVA, RETORNA PARA ONDE ELA ESTÁ
            //if (!String.IsNullOrWhiteSpace(loginviewmodel.UrlRetorno) || Url.IsLocalUrl(loginviewmodel.UrlRetorno))
            //    return Redirect(loginviewmodel.UrlRetorno);
            //else
            //    return RedirectToAction("Index", "Login");

            return View(loginviewmodel);

        }

        private void EntraUser(string CPF, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, CPF));
                claims.Add(new Claim("CPF", CPF));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //MÉTODO PARA DAR LOGOUT DIRETO
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TempData["MensagemAviso"] = "Logout feito com sucesso!";
            return RedirectToAction("Login", "Login");
        }

        //UMA VIEW PRESENTE NA SHARED PARA CASO OCORRER UM ERRO NA CONEXÃO COM O BANCO
        public ActionResult Error(string mensagem)
        {
            ViewBag.error = "Entre em contato com um profissional TI e passe a seguinte mensagem: {" + mensagem + "}";
            return View();
        }
    }
}
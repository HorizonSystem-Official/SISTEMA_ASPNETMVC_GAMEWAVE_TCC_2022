using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    public class CarrinhoController : Controller
    {
        // GET: Carrinho
        //VIEW DE TESTE
        //public ActionResult FazerCarrinho()
        //{
        //    return View();
        //}

        //VIEW DE TESTE
        //public ActionResult ColocarItemCarrinho()
        //{
        //    return View();
        //}

        //É SELECIONADO UM CARRINHO DO CLIENTE CADASTRADO E TENHA FEITO O LOGIN COM SEU CPF
        public ActionResult VerCarrinhoIndex(string CPF)
        {
            //UTILIZADO O OWIN PARA PEGAR O CPF LOGADO NO MOMENTO
            CPF = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return View(CPF);
            }
            var carr = new ItemCarrinho();

            //RETORNA O CARRINHO DOS ITENS COLOCADOS
            var carrinho = carr.ListarItemCarr(CPF);

            return View(carrinho);
        }

        //MÉTODO PARA ADICIONAR UM ITEM AO CARRINHO
        [HttpGet]
        public ActionResult AdicionarAOCarrinho(int codprod)
        {
            var CPFCliente = User.Identity.Name;
            var itemcar = new ItemCarrinho();
            
            //COM A UTILIZAÇÃO DO OWIN PARA PEGAR O CPF ATUAL, PEGA O ID DE PRODUTO PARA INSERIR NO CARRINHO E REDIRECIONA PARA A INDEX
            itemcar.ColocarItemCarrinho(codprod, CPFCliente);

            TempData["MensagemAviso"] = "Produto colocado com sucesso no carrinho! Acesse o link 'Ver Carrinho' para visualizar o carrinho";
           return RedirectToAction("Index", "Login");

        }

        //FORMULÁRIO PARA REALIZAR A VENDA 
        public ActionResult FazerVenda()
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

        //UTILIZA O OBJETO DA VENDA DO FORMULÁRIO PARA ENVIAR EM UM MÉTODO DA MODEL
        [HttpPost]
        public ActionResult FazerVenda(Venda ven)
        {
            ven.fk_Clinte_CPF = User.Identity.Name;

            //if (!ModelState.IsValid)
            //{
            //    return View(ven);
            //}
            Venda venda = new Venda
            {
                FormaPag = ven.FormaPag,
                Parcela = ven.Parcela,
                fk_Clinte_CPF = ven.fk_Clinte_CPF
            };

            venda.FazerVenda(venda);

            //FAZ O TEMPDATA NOTIFICAR O USUÁRIO PARA A VENDA FEITA E REDIRECIONAR ELE PARA A CONSULTA DAS VENDAS
            TempData["MensagemAviso"] = "Cadastro da venda feito com sucesso!";
            return RedirectToAction("ConsulVenda", "Carrinho");
        }

        //MÉTODO COM VIEW PARA VISUALIZAR A CONSULTA DE VENDAS SEM FILTRO
        public ActionResult ConsulVenda(string CPFCli)
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
            var ven = new Venda();

            if (CPFCli != null)
            {
                var MostrarVendaPeloCPF = ven.ListarVendasPeloCPFCliente(CPFCli);

                return View(MostrarVendaPeloCPF);
            }

            var MostrarVenda = ven.ListarTodasVendas();

            return View(MostrarVenda);
        }

    }
}
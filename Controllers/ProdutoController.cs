using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using Image = System.Drawing.Image;
using System.IO;
using System.Web.UI.WebControls;
using TCC_Sistema_Cliente_Jogos_2022.Utils;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    //[Authorize]
    public class ProdutoController : Controller
    {
        // GET: Produto
        //REALIZA A BUSCA DE TODOS OS PRODUTOS
        public ActionResult ConsulProduto()
        {
            //Try e Catch utilizado para tratar erros relacionados a conexão do bd
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

            //UTILIZA UM MÉTODO GERAL PARA RETORNAR UMA LISTA GERAL
            var MostrarProd = produ.ListarProd();

            return View(MostrarProd);
        }


        //TELA PARA REALIZAR O CADASTRO DO PRODUTO
        public ActionResult CadProduto()
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

        //Processo para o cadastro do produto
        [HttpPost]
        public ActionResult CadProduto(Produto prod, HttpPostedFileBase imgcapa)
        {
            if(ModelState.IsValid)
            {
                //ADAPTAÇÃO DA IMAGEM PARA O BANCO DE DADOS E ARMAZENAMENTO NA PASTA IMAGENSGAMES
                if (imgcapa != null && imgcapa.ContentLength > 0)
                {
                    string extensao = Path.GetExtension(imgcapa.FileName).ToLower();

                    if (extensao.Equals(".jpg") || extensao.Equals(".png") || extensao.Equals(".jpeg"))
                    {
                        string nomeArquivo = Hash.GenerateMD5(
                                string.Format("{0:HH:mm:ss tt}", DateTime.Now) + imgcapa.FileName
                            ) + extensao; // Criptografar o nome do arquivo com a data no arquivo utils no método MD5 para torna-lo único

                        string imgPath = Path.Combine(Server.MapPath("/ImagensGames/Capa/"), nomeArquivo);

                        bool imgSaved = new ImageCrop().SaveCroppedImage(Image.FromStream(imgcapa.InputStream), 256, 256, imgPath);

                        if (imgSaved)
                        {
                            prod.ImgCapa = "/ImagensGames/Capa/" + nomeArquivo;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("imgCapa", "A imagem deve ser do tipo .jpg/.png/.jpeg");
                        return View(prod);
                    }
                }
                else
                {
                    prod.ImgCapa = "/ImagensGames/Capa/capatemplate.png";
                }

            }
            //INSERE O FORMULÁRIO EM UM OBJETO PARA DEPOIS SER ADICIONADO POR UM MÉTODO DA CLASSE PRODUTO
            Produto produto = new Produto
            {
                ProdNome = prod.ProdNome,
                ProdTipo = prod.ProdTipo,
                ProdQtnEstoque = prod.ProdQtnEstoque,
                ProdDesc = prod.ProdDesc,
                ProdAnoLanc = prod.ProdAnoLanc,
                ProdFaixaEtaria = prod.ProdFaixaEtaria,
                ProdValor = prod.ProdValor,
                ImgCapa = prod.ImgCapa,
                FK_Funcionario_IdFunc = prod.FK_Funcionario_IdFunc
            };

            prod.CadastrarProd(produto);

            //EMITE UM AVISO AO USUÁRIO POR UMA TEMPDATA
            TempData["MensagemAviso"] = "Cadastro do produto feito com sucesso!";
            return RedirectToAction("ConsulProduto", "Produto");
        }

        //É PEGO O CÓDIGO DO PRODUTO PARA FAZER AS ALTERAÇÕES
        [HttpGet]
        public ActionResult EdiProduto(int codprod)
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

            Produto produto = new Produto();

            var umproduto = produto.ListaUMProdutoID(codprod);

            if (umproduto == null)
                return HttpNotFound();

            return View(umproduto);
        }

        //MÉTODO PARA ALTERAÇÃO DO PRODUTO
        [HttpPost]
        public ActionResult EdiProduto(Produto prod, HttpPostedFileBase imgcapa)
        {
            //ADAPTAÇÃO DA IMAGEM PARA ALTERAR NO PRODUTO
            if (ModelState.IsValid)
            {
                if (imgcapa != null && imgcapa.ContentLength > 0)
                {
                    string extensao = Path.GetExtension(imgcapa.FileName).ToLower();

                    if (extensao.Equals(".jpg") || extensao.Equals(".png") || extensao.Equals(".jpeg"))
                    {
                        string nomeArquivo = Hash.GenerateMD5(
                                string.Format("{0:HH:mm:ss tt}", DateTime.Now) + imgcapa.FileName
                            ) + extensao; // Criptografar o nome do arquivo com a data no arquivo utils no método MD5 para torna-lo único

                        string imgPath = Path.Combine(Server.MapPath("/ImagensGames/Capa/"), nomeArquivo);

                        bool imgSaved = new ImageCrop().SaveCroppedImage(Image.FromStream(imgcapa.InputStream), 256, 256, imgPath);

                        if (imgSaved)
                        {
                            //if (prod.ImgCapa != "/ImagensGames/Capa/capatemplate.png")
                            //    System.IO.File.Delete(Server.MapPath(prod.ImgCapa));

                            prod.ImgCapa = "/ImagensGames/Capa/" + nomeArquivo;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("imgCapa", "A imagem deve ser do tipo .jpg/.png/.jpeg");
                        return View(prod);
                    }
                }
                /*
                else
                {
                    prod.ImgCapa = "/ImagensGames/Capa/capatemplate.png";
                }
                */

            }
            Produto produto = new Produto();
            //{
            //    ProdNome = prod.ProdNome,
            //    ProdTipo = prod.ProdTipo,
            //    ProdQtnEstoque = prod.ProdQtnEstoque,
            //    ProdDesc = prod.ProdDesc,
            //    ProdAnoLanc = prod.ProdAnoLanc,
            //    ProdFaixaEtaria = prod.ProdFaixaEtaria,
            //    ProdValor = prod.ProdValor,
            //    ImgCapa = prod.ImgCapa
            //};

            //EMITE UM AVISO AO USUÁRIO POR UMA TEMPDATA
            TempData["MensagemAviso"] = "Alteração do produto feito com sucesso!";

            produto.AlterProduto(prod);

            return RedirectToAction("ConsulProduto");
        }

     
        //MÉTODO DE APAGAR O PRODUTO, É UTILIZADO O CPF PARA BUSCAR REGISTROS EM OUTRAS TABELAS COMO FK
        public ActionResult DelProduto(int codprod, string CPFCliente)
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

            var produto = new Produto();

            produto.DelProduto(codprod, CPFCliente);
            /*
            FuncionarioViewModel funcionario = new FuncionarioViewModel();
            funcionario.CPF = CPF;
            funcionario.DeletFuncio = funcionario.CPF;
            */

            //EMITE UM AVISO AO USUÁRIO POR UMA TEMPDATA
            TempData["MensagemAviso"] = "Produto deletado com sucesso!";
            return RedirectToAction("ConsulProduto");

        }

        //SEMELHANTE AO MÉTODO DE BUSCA DO PRODUTO, MAS SELECIONA UM ÚNICO PRODUTO PARA VER MAIS DETALHES SOBRE
        public ActionResult DetalhesProduto(int codprod)
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

            var metodoproduto = new Produto();

            //MÉTODO VAI RECEBER O PARÂMETRO ID DO PRODUTO
            var produto = metodoproduto.ListaDetalheProdutoID(codprod);

            if( produto == null)
            {
                return HttpNotFound();
            }

            return View(produto);
        }

        
        
    }
}
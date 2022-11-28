using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class ItemCarrinho : Produto
    {
        [Display(Name = "Quantidade do produto")]
        [Required]
        public int QtnProd { get; set; }
        [Display(Name = "Valor Unitário")]
        [Required]
        public decimal ValorUnit { get; set; }
        [Display(Name = "Valor Total")]
        [Required]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Código do Produto")]
        [Required]
        public int fk_Produto_CodProd { get; set; }
        [Display(Name = "Código do Carrinho")]
        [Required]
        public int fk_Carrinho_CodCarrinho { get; set; }

        public string CPFCliente { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();

        //MÉTODO SEM RETORNO PARA INSERIR O ITEM AO CARRINHO COM DOIS PARÂMETROS
        public void ColocarItemCarrinho (int codprod, string CPFCliente)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELA URL
            cmd.CommandText = "call spInsertItemCarrinho(@spQtnProd,@spfk_Produto_CodProd, @spfkCliCpf);";
            cmd.Parameters.Add("@spQtnProd", MySqlDbType.Int16).Value = 1;
            cmd.Parameters.Add("@spfk_Produto_CodProd", MySqlDbType.Int64).Value = codprod ;
            cmd.Parameters.Add("@spfkCliCpf", MySqlDbType.VarChar).Value = CPFCliente;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }


        //LISTAGEM DE TODOS OS ITENS NO CARRINHO
        public List<ItemCarrinho> ListarItemCarr(string CPF)
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "call spMostraItens2(@spCPF);";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = CPF;
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var read = cmd.ExecuteReader();
            List<ItemCarrinho> tempItemCarLista = new List<ItemCarrinho>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (read.Read())
            {
                var tempItemCarr = new ItemCarrinho();

                tempItemCarr.fk_Produto_CodProd = Int16.Parse(read["CodProd"].ToString());
                    tempItemCarr.ValorUnit = Decimal.Parse(read["ValorUnit"].ToString());
                    tempItemCarr.QtnProd = Int16.Parse(read["QtnProd"].ToString());
                    tempItemCarr.ValorTotal = Decimal.Parse(read["ValorTotal"].ToString());
                    tempItemCarr.ProdNome = read["ProdNome"].ToString();
                    tempItemCarr.CodProd = Int16.Parse(read["CodProd"].ToString());
                    tempItemCarr.ImgCapa = read["ImgCapa"].ToString();
                tempItemCarr.fk_Carrinho_CodCarrinho = Int16.Parse(read["fk_Carrinho_CodCarrinho"].ToString());

                tempItemCarLista.Add(tempItemCarr);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            read.Close();
            conexao.Close();

            return tempItemCarLista;
        }


        //public ItemCarrinho VerItensCarrinhoIndividual(string CPF)
        //{
        //    conexao.Open();
        //    cmd.CommandText = "call spMostraItens(@spCPF);";
        //    cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = CPF;
        //    cmd.Connection = conexao;
        //    var leituraItemCarrinhoID = cmd.ExecuteReader();
        //    var TempItemCarrinho = new ItemCarrinho();

        //    if (leituraItemCarrinhoID.Read())
        //    {
        //        TempItemCarrinho.fk_Produto_CodProd = Int16.Parse(leituraItemCarrinhoID["CodProd"].ToString());
        //        TempItemCarrinho.ValorUnit = Decimal.Parse(leituraItemCarrinhoID["ValorUnit"].ToString());
        //        TempItemCarrinho.QtnProd = Int16.Parse(leituraItemCarrinhoID["QtnProd"].ToString());
        //        TempItemCarrinho.ValorTotal = Decimal.Parse(leituraItemCarrinhoID["ValorTotal"].ToString());
        //        TempItemCarrinho.ProdNome = leituraItemCarrinhoID["ProdNome"].ToString();
        //        TempItemCarrinho.CodProd = Int16.Parse(leituraItemCarrinhoID["CodProd"].ToString());
        //        TempItemCarrinho.ImgCapa = leituraItemCarrinhoID["ImgCapa"].ToString();
        //    }
        //    leituraItemCarrinhoID.Close();
        //    conexao.Close();
        //    return TempItemCarrinho;

        //}

        
    }
}
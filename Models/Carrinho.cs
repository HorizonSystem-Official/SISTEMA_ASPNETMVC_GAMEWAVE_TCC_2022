using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Carrinho
    {

        public int qtdProdCart { get; set; }

        [Display(Name = "Código do carrinho")]
        [Required]
        public int CodCarrinho { get; set; }

        [Display(Name = "Valor total")]
        [Required]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Cupom do carrinho")]
        [Required]
        public string Cupom { get; set; }

        [Display(Name = "Código do cupom")]
        [Required]
        public int fk_Cupons_CodCupom { get; set; }

        [Display(Name = "Código do carrinho")]
        [Required]
        public string fk_Cliente_CPF { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();


        //MÉTODO PARA INSERIR UM CUPOM À UM CARRINHO PELO CPF
        public void ColocarCarrinho (Carrinho carrinho)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertCarrinho(@spCupom, @spfkCliCpf);";
            cmd.Parameters.Add("@spCupom", MySqlDbType.VarChar).Value = carrinho.Cupom;
            cmd.Parameters.Add("@spfkCliCpf", MySqlDbType.VarChar).Value = carrinho.fk_Cliente_CPF;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        

        
    }
}
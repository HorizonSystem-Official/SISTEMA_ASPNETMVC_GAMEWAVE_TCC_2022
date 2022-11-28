using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Venda
    {
        //ATRIBUTOS
        [Display(Name = "Código da venda")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int CodVenda { get; set; }

        [Display(Name = "Forma de Pagamento")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string FormaPag { get; set; }

        [Display(Name = "Quantidade de parcelas")]
        [Range(1, 6, ErrorMessage = "A quantidade do valor deve ser no mínimo 1 a 6")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Parcela { get; set; }

        [Display(Name = "Valor total da venda")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public decimal Total { get; set; }

        [Display(Name = "Código do carrinho")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int fk_Carrinho_CodCarrinho { get; set; }

        [Display(Name = "Código via CPF do Cliente")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string fk_Clinte_CPF { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();

        //MÉTODO PARA ADD A VENDA
        public void FazerVenda(Venda venda)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertVenda(@VFormaPag,@VParcela, @vfk_Clinte_CPF);";
            cmd.Parameters.Add("@VFormaPag", MySqlDbType.VarChar).Value = venda.FormaPag;
            cmd.Parameters.Add("@VParcela", MySqlDbType.VarChar).Value = venda.Parcela;
            cmd.Parameters.Add("@vfk_Clinte_CPF", MySqlDbType.VarChar).Value = venda.fk_Clinte_CPF;
            cmd.Connection = conexao;

            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //LISTAGEM DE TODOS AS VENDAS
        public List<Venda> ListarTodasVendas()
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "select * from tbvenda";
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var readVend = cmd.ExecuteReader();
            List<Venda> tempVendLista = new List<Venda>();

            //ENQUANTO TIVER REGISTRO NA readVend, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (readVend.Read())
            {
                var tempVend = new Venda();
                tempVend.CodVenda = Int16.Parse(readVend["CodVenda"].ToString());
                tempVend.FormaPag = readVend["FormaPag"].ToString();
                tempVend.Parcela = Int16.Parse(readVend["Parcela"].ToString());
                tempVend.Total = Decimal.Parse(readVend["Total"].ToString());
                tempVend.fk_Clinte_CPF = readVend["fk_Clinte_CPF"].ToString();
                tempVend.fk_Carrinho_CodCarrinho = Int16.Parse(readVend["fk_Carrinho_CodCarrinho"].ToString());

                tempVendLista.Add(tempVend);
            }

            //É FECHADO A LEITURA DA VARIÁVEL readVend E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            readVend.Close();
            conexao.Close();

            return tempVendLista;
        }
    }
}
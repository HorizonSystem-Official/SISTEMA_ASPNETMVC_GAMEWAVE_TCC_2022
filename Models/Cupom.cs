using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Cupom
    {
        public Cupom()
        {

        }

        
        [Display(Name = "Código do Cupom")]
        public int CodCupom { get; set; }

        [Display(Name = "Nome do Cupom")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "O campo deve conter o código de desconto!")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string CupomTxt { get; set; }
        [Display(Name = "Cupom de Desconto")]
        [Range(1, 100, ErrorMessage = "A quantidade do valor deve ser no mínimo entre 1 a 100")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public int ValorCupom { get; set; }

        [Display(Name = "Número de limite para compras")]
        [Range(1, 1000, ErrorMessage = "A quantidade do número deve ser no mínimo entre 1 a 1000")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int NumLimiteCompras { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();

        //MÉTODO PARA ADICIONAR UM CUPOM
        public void CadastrarCup(Cupom cupom)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertCupom(@spCupomTxt, @spValorCupom, @spNumLimiteCompras);";
            cmd.Parameters.Add("@spCupomTxt", MySqlDbType.VarChar).Value = cupom.CupomTxt;
            cmd.Parameters.Add("@spValorCupom", MySqlDbType.Int32).Value = cupom.ValorCupom;
            cmd.Parameters.Add("@spNumLimiteCompras", MySqlDbType.Int16).Value = cupom.NumLimiteCompras;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //MÉTODOS DE LISTAGEM DE TODOS OS CUPONS
        public List<Cupom> ListarCup()
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "select * from tbcupons;";
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var read = cmd.ExecuteReader();
            List<Cupom> tempCupLista = new List<Cupom>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (read.Read())
            {
                var tempCup = new Cupom();

                tempCup.CodCupom = Int16.Parse(read["CodCupom"].ToString());
                tempCup.CupomTxt = read["CupomTxt"].ToString();
                tempCup.ValorCupom = Int16.Parse(read["ValorCupom"].ToString());
                tempCup.NumLimiteCompras = Int16.Parse(read["NumLimiteCompras"].ToString());

                tempCupLista.Add(tempCup);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            read.Close();
            conexao.Close();

            return tempCupLista;
        }

        //SEMELHANTE AO ListarCup, mas apenas irá selecionar um cupom pelo parâmetro codcupom
        public Cupom ListaUMCupom(int codcupom)
        {
            conexao.Open();
            cmd.CommandText = "select * from tbcupons where CodCupom = @codcupom";
            cmd.Parameters.Add("@codcupom", MySqlDbType.Int16).Value = codcupom;
            cmd.Connection = conexao;
            var leituracupomCodigo = cmd.ExecuteReader();
            var TempCupom = new Cupom();

            if (leituracupomCodigo.Read())
            {
                TempCupom.CodCupom = Int16.Parse(leituracupomCodigo["CodCupom"].ToString());
                TempCupom.CupomTxt = leituracupomCodigo["CupomTxt"].ToString();
                TempCupom.ValorCupom = Int16.Parse(leituracupomCodigo["ValorCupom"].ToString());
                TempCupom.NumLimiteCompras = Int16.Parse(leituracupomCodigo["NumLimiteCompras"].ToString());
            }
            leituracupomCodigo.Close();
            conexao.Close();
            return TempCupom;
        }
        

        //MÉTODO PARA ALTERAR CUPOM, CONTENDO A ESTRUTURA SEMELHANTE AO CADASTRO DO CUPOM
        public void AlterarCup(Cupom cupom)
        {
            conexao.Open();
            cmd.CommandText = "call spUpdateCupom(@spIDCup,@sptxt, @spvalor, @splimite);";
            cmd.Parameters.Add("spIDCup", MySqlDbType.Int16).Value = cupom.CodCupom;
            cmd.Parameters.Add("@sptxt", MySqlDbType.VarChar).Value = cupom.CupomTxt;
            cmd.Parameters.Add("@spvalor", MySqlDbType.Int16).Value = cupom.ValorCupom;
            cmd.Parameters.Add("@splimite", MySqlDbType.VarChar).Value = cupom.NumLimiteCompras;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }
        //FIM DO MÉTODO ALTERAR

        //MÉTODO PARA DELETAR O CUPOM, SEMELHANTE AO CADASTRO DO CUPOM MAS APENAS IRÁ ENVIAR UM PARÂMETRO DA URL
        public void DeletarCup (int codcupom)
        {
            conexao.Open();
            cmd.CommandText = "call spDeleteCupom(@spid);";
            cmd.Parameters.Add("@spid", MySqlDbType.Int16).Value = codcupom;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }
        //FIM DO MÉTODO PARA DELETAR


    }
}
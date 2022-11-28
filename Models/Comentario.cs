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
    public class Comentario
    {
        public Comentario()
        {

        }

        

        [Display(Name = "Comentário")]
        [Required(ErrorMessage = "É preciso escrever algo neste campo para ser postado")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "O campo deve conter entre 3 a 300 caracteres")]
        public string TxtComentario { get; set; }

        [Display(Name = "Código do produto")]
        public int Fk_CodProd { get; set; }

        [Display(Name = "Código via CPF do cliente")]
        public string Fk_CpfCli { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();


        //MÉTODO PARA ADICIONAR UM COMENTÁRIO E DIRECIONA PARA UMA LISTA
        public void CadastrarComen(Comentario comentario)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertComentarios(@spTxt, @spfk_codprod, @spfk_cpfcli);";
            cmd.Parameters.Add("@spTxt", MySqlDbType.VarChar).Value = comentario.TxtComentario;
            cmd.Parameters.Add("@spfk_codprod", MySqlDbType.Int32).Value = comentario.Fk_CodProd;
            cmd.Parameters.Add("@spfk_cpfcli", MySqlDbType.VarChar).Value = comentario.Fk_CpfCli;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();

            //IRÁ PEGAR O CÓDIGO DO PRODUTO PARA FAZER A REFERÊNCIA À UMA LISTA DE COMENTÁRIOS
            int codprod = comentario.Fk_CodProd;

            var metodocomentarios = new Comentario();

            metodocomentarios.ListarTodosComentarios(codprod);
        }

        //MÉTODOS DE LISTAGEM DE TODOS OS COMENTÁRIOS
        public List<DetalhesProdutoEComentarios> ListarTodosComentarios(int codprod)
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "call spVerComentarios(@spCodProd)";
            cmd.Parameters.Add("@spCodProd", MySqlDbType.Int32).Value = codprod;
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var readComent = cmd.ExecuteReader();
            List<DetalhesProdutoEComentarios> tempComentLista = new List<DetalhesProdutoEComentarios>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (readComent.Read())
            {
                var tempComent = new DetalhesProdutoEComentarios();

                tempComent.Nome = readComent["NomeCliente"].ToString();
                tempComent.TxtComentario = readComent["TxtComentario"].ToString();

                tempComentLista.Add(tempComent);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            readComent.Close();
            conexao.Close();

            return tempComentLista;
        }

        
    }
}
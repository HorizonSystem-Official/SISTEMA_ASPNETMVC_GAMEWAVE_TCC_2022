
using Antlr.Runtime.Misc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;


namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Produto
    {
        public Produto()
        {

        }

        
        [Display(Name = "Código do Produto")]
        public int CodProd { get; set; }

        [Display(Name = "Nome do Produto")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O campo deve conter o mínimo entre 2 a 50 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string ProdNome { get; set; }

        [Display(Name = "Tipo do Produto")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "O campo deve conter o mínimo entre 2 a 20 caracteres")]
        public string ProdTipo { get; set; }

        [Display(Name = "Quantidade em estoque")]
        [Range(1, 9999, ErrorMessage = "A quantidade de estoque deve ter no mínimo entre 1 a 9999 produtos")]
        public int ProdQtnEstoque { get; set; }

        [Display(Name = "Descrição do produto")]
        [MaxLength(1000, ErrorMessage = "O campo contém o máximo de 1000 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string ProdDesc { get; set; }

        [Display(Name = "Ano de Lançamento")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "O campo deve conter 4 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string ProdAnoLanc { get; set; }

        [Display(Name = "Faixa etária do Produto")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "O campo contém apenas 3 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string ProdFaixaEtaria { get; set; }

        [Display(Name = "Valor Unitário do Produto")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public decimal ProdValor { get; set; }

        [Display(Name = "Capa do produto")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string ImgCapa { get; set; }

        [Display(Name = "Código do Funcionário")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int FK_Funcionario_IdFunc { get; set; }

        public bool tipopesquisa { get; set; }

        public string PesquisaGeral { get; set; }

        public string PesquisaCat { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();


        //MÉTODO PARA ADD O PRODUTO
        public void CadastrarProd (Produto prod)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertProduto(@spProdNome, @spTipoProd, @spQtnEstoqueProd, @spDescProd, @spAnoLancProd, @spFaixaEtaraia, @spProdValor, @spImgCapa, @spFkIdFunc);";
            cmd.Parameters.Add("@spProdNome", MySqlDbType.VarChar).Value = prod.ProdNome;
            cmd.Parameters.Add("@spTipoProd", MySqlDbType.VarChar).Value = prod.ProdTipo;
            cmd.Parameters.Add("@spQtnEstoqueProd", MySqlDbType.Int32).Value = prod.ProdQtnEstoque;
            cmd.Parameters.Add("@spDescProd", MySqlDbType.VarChar).Value = prod.ProdDesc;
            cmd.Parameters.Add("@spAnoLancProd", MySqlDbType.VarChar).Value = prod.ProdAnoLanc;
            cmd.Parameters.Add("@spFaixaEtaraia", MySqlDbType.VarChar).Value = prod.ProdFaixaEtaria;
            cmd.Parameters.Add("@spProdValor", MySqlDbType.Decimal).Value = prod.ProdValor;
            cmd.Parameters.Add("@spImgCapa", MySqlDbType.VarChar).Value = prod.ImgCapa;
            cmd.Parameters.Add("@spFkIdFunc", MySqlDbType.Int32).Value = prod.FK_Funcionario_IdFunc;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //MÉTODO DE LISTAGEM DE TODOS OS PRODUTOS

        public List<Produto> ListarProd()
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "select * from tbproduto;";
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var readProd = cmd.ExecuteReader();
            List<Produto> tempProdLista = new List<Produto>();

            //ENQUANTO TIVER REGISTRO NA readVend, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (readProd.Read())
            {
                var tempProd = new Produto();

                tempProd.CodProd = Int32.Parse(readProd["CodProd"].ToString());
                tempProd.ProdNome = readProd["ProdNome"].ToString();
                tempProd.ProdTipo = readProd["ProdTipo"].ToString();
                tempProd.ProdQtnEstoque = Int32.Parse(readProd["ProdQtnEstoque"].ToString());
                tempProd.ProdDesc = readProd["ProdDesc"].ToString();
                tempProd.ProdAnoLanc = readProd["ProdAnoLanc"].ToString();
                tempProd.ProdFaixaEtaria = readProd["ProdFaixaEtaria"].ToString();
                tempProd.ProdValor = Decimal.Parse(readProd["ProdValor"].ToString());
                tempProd.ImgCapa = readProd["ImgCapa"].ToString();
                tempProd.FK_Funcionario_IdFunc = Int32.Parse(readProd["FK_Funcionario_IdFunc"].ToString());

                tempProdLista.Add(tempProd);
            }
            //É FECHADO A LEITURA DA VARIÁVEL readVend E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            readProd.Close();
            conexao.Close();

            return tempProdLista;
        }

        //SEMELHANTE AO MÉTODO DE LISTAGEM ACIMA, PORÉM IRÁ APENAS RETORAR UM PRODUTO PELO SEU PARÂMETRO ID
        public Produto ListaUMProdutoID(int codprod)
        {
            conexao.Open();
            cmd.CommandText = "call spMostraProdDetalhado2(@spCodProd);";
            cmd.Parameters.Add("@spCodProd", MySqlDbType.Int16).Value = codprod;
            cmd.Connection = conexao;
            var leituraProdID = cmd.ExecuteReader();
            var TempProduto = new Produto();

            if (leituraProdID.Read())
            {
                TempProduto.CodProd = Int32.Parse(leituraProdID["CodProd"].ToString());
                TempProduto.ProdNome = leituraProdID["ProdNome"].ToString();
                TempProduto.ProdTipo = leituraProdID["ProdTipo"].ToString();
                TempProduto.ProdQtnEstoque = Int16.Parse(leituraProdID["ProdQtnEstoque"].ToString());
                TempProduto.ProdDesc = leituraProdID["ProdDesc"].ToString();
                TempProduto.ProdAnoLanc = leituraProdID["ProdAnoLanc"].ToString();
                TempProduto.ProdFaixaEtaria = leituraProdID["ProdFaixaEtaria"].ToString();
                TempProduto.ProdValor = Decimal.Parse(leituraProdID["ProdValor"].ToString());
                TempProduto.ImgCapa = leituraProdID["ImgCapa"].ToString();
                TempProduto.FK_Funcionario_IdFunc = Int16.Parse(leituraProdID["fk_Funcionario_IdFunc"].ToString());                
            }
            leituraProdID.Close();
            conexao.Close();
            return TempProduto;
        }

        //SEMELHANTE A LISTAGEM GERAL, PORÉM IRÁ SELECIONAR UM PRODUTO EM ESPECÍFICO POR UM PARÂMETRO DA URL
        public DetalhesProdutoEComentarios ListaDetalheProdutoID(int codprod)
        {
            conexao.Open();
            cmd.CommandText = "call spMostraProdSimples2(@spCodProd);";
            cmd.Parameters.Add("@spCodProd", MySqlDbType.Int16).Value = codprod;
            cmd.Connection = conexao;
            var leituraProdID = cmd.ExecuteReader();
            var TempProduto = new DetalhesProdutoEComentarios();

            if (leituraProdID.Read())
            {
                TempProduto.CodProd = Int32.Parse(leituraProdID["CodProd"].ToString());
                TempProduto.ProdNome = leituraProdID["ProdNome"].ToString();
                TempProduto.ProdTipo = leituraProdID["ProdTipo"].ToString();
                TempProduto.ProdValor = Decimal.Parse(leituraProdID["ProdValor"].ToString());
                TempProduto.ProdDesc = leituraProdID["ProdDesc"].ToString();
                TempProduto.ProdAnoLanc = leituraProdID["ProdAnoLanc"].ToString();
                TempProduto.ProdFaixaEtaria = leituraProdID["ProdFaixaEtaria"].ToString();
                TempProduto.ImgCapa = leituraProdID["ImgCapa"].ToString();

            }
            leituraProdID.Close();
            conexao.Close();
            return TempProduto;
        }
        //FIM LISTAGEM
        
        //MÉTODO PARA ALTERAR O PRODUTO, O PROCESSO É SEMELHANTE AO CAD VOID
        public void AlterProduto(Produto prod)
        {
            conexao.Open();
            cmd.CommandText = "call spUpdateProd2(@spCodProd,@spnome, @sptipo, @spquantidade, @spdesc, @spano, @spfaixa, @spvalor, @spimagem );";
            cmd.Parameters.Add("@spCodProd", MySqlDbType.Int16).Value = prod.CodProd;
            cmd.Parameters.Add("@spnome", MySqlDbType.VarChar).Value = prod.ProdNome;
            cmd.Parameters.Add("@sptipo", MySqlDbType.VarChar).Value = prod.ProdTipo;
            cmd.Parameters.Add("@spquantidade", MySqlDbType.Int16).Value = prod.ProdQtnEstoque;
            cmd.Parameters.Add("@spdesc", MySqlDbType.VarChar).Value = prod.ProdDesc;
            cmd.Parameters.Add("@spano", MySqlDbType.VarChar).Value = prod.ProdAnoLanc;
            cmd.Parameters.Add("@spfaixa", MySqlDbType.VarChar).Value = prod.ProdFaixaEtaria;
            cmd.Parameters.Add("@spvalor", MySqlDbType.Decimal).Value = prod.ProdValor;
            cmd.Parameters.Add("@spimagem", MySqlDbType.VarChar).Value = prod.ImgCapa;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //MÉTODO PARA APAGAR O PRODUTO, O PROCESSO É SEMELHANTE AO CAD VOID, PORÉM HÁ PARÂMETRO VINDO PELA URL
        public void DelProduto(int codprod, string CPFCliente)
        {
            conexao.Open();
            cmd.CommandText = "call spDeleteProd2(@spCodProd, @spCPF);";
            cmd.Parameters.Add("@spCodProd", MySqlDbType.Int16).Value = codprod;
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = CPFCliente;

            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }
    }

}

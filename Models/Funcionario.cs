using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using TCC_Sistema_Cliente_Jogos_2022.ViewModels;
using BCrypt;
using TCC_Sistema_Cliente_Jogos_2022.Utils;
using Hash = TCC_Sistema_Cliente_Jogos_2022.Utils.Hash;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Funcionario : Usuario
    {

        //public Funcionario(string nome, DateTime dataNasc, string cpf, string senha, bool cargo)
        //{

        //    Nome = nome;
        //    DataNasc = dataNasc;
        //    CPF = cpf;
        //    Senha = senha;
        //    Cargo = cargo;
        //}

        [Display(Name = "Código do Funcionário")]

        public int IdFunc { get; set; }

        public string Cargo { get; set; }


        MySqlConnection conexao = new MySqlConnection (ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();


        //MÉTODO PARA ADD O FUNCIONÁRIO 
        public void CriarFuncio (Funcionario funcionario)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertFuncionario(@spNome, @spData, @spCPF, @spSenha, @spCargo);";
            cmd.Parameters.Add("@spNome", MySqlDbType.VarChar).Value = funcionario.Nome;
            cmd.Parameters.Add("@spData", MySqlDbType.DateTime).Value = funcionario.DataNasc;
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = funcionario.CPF;
            cmd.Parameters.Add("@spSenha", MySqlDbType.VarChar).Value = funcionario.Senha;
            cmd.Parameters.Add("@spCargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //MÉTODO DE LISTAGEM

        public List<Funcionario> ListarFuncio()
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "select * from tbfuncionario;";
            
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var read = cmd.ExecuteReader();
            List<Funcionario> tempFuncLista = new List<Funcionario>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (read.Read())
            {
                var tempFunc = new Funcionario();

                    tempFunc.IdFunc = Int16.Parse(read["IdFunc"].ToString());
                    tempFunc.Nome = read["NomeFunc"].ToString();
                    tempFunc.DataNasc = DateTime.Parse(read["DataNasc"].ToString());
                    tempFunc.CPF = read["CPF"].ToString();
                    tempFunc.Cargo = read["Cargo"].ToString();

                tempFuncLista.Add(tempFunc);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            read.Close();
            conexao.Close();

            return tempFuncLista;
        }

        public List<Funcionario> ListarFuncioPeloCPF(string CPFFun)
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "call spPesquisaCPFFuncionario(@spCPF)";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = CPFFun;

            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var read = cmd.ExecuteReader();
            List<Funcionario> tempFuncLista = new List<Funcionario>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (read.Read())
            {
                var tempFunc = new Funcionario();

                tempFunc.IdFunc = Int16.Parse(read["IdFunc"].ToString());
                tempFunc.Nome = read["NomeFunc"].ToString();
                tempFunc.DataNasc = DateTime.Parse(read["DataNasc"].ToString());
                tempFunc.CPF = read["CPF"].ToString();
                tempFunc.Cargo = read["Cargo"].ToString();

                tempFuncLista.Add(tempFunc);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            read.Close();
            conexao.Close();

            return tempFuncLista;
        }

        //MÉTODO PARA SELECIONAR UM FUNCIONÁRIO ÚNICO PELO CPF NA URL, UTILIZANDO A MESMA ESTRUTURA DO QUE DO LISTARFUNCIO()
        public Funcionario ListaUMFuncio(string CPF)
        {
            conexao.Open();
            cmd.CommandText = "call spDadosFunc2(@spCPFFunc);";
            cmd.Parameters.Add("@spCPFFunc", MySqlDbType.VarChar).Value = CPF;
            cmd.Connection = conexao;
            var leituraFuncioCPF = cmd.ExecuteReader();
            var TempFuncionario = new Funcionario();

            //A DIFERENÇA É QUE VAI VERIFICAR SE IRÁ HAVER O REGISTRO ÚNICO OU NÃO
            if(leituraFuncioCPF.Read())
            {
                TempFuncionario.IdFunc = Int32.Parse(leituraFuncioCPF["IdFunc"].ToString());
                TempFuncionario.Nome = leituraFuncioCPF["NomeFunc"].ToString();
                TempFuncionario.DataNasc = DateTime.Parse(leituraFuncioCPF["DataNasc"].ToString());
                TempFuncionario.CPF = leituraFuncioCPF["CPF"].ToString();
                TempFuncionario.Cargo = leituraFuncioCPF["Cargo"].ToString();
            }
            leituraFuncioCPF.Close();
            conexao.Close();
            return TempFuncionario;
        }

        //MÉTODO PARA VERIFICAR A AUTENTICAÇÃO, APRESENTANDO A MESMA ESTRUTURA DO ListaUMFuncio 
        public Funcionario ListaUMFuncioLOGIN(string CPF)
        {
            conexao.Open();
            cmd.CommandText = "call spDadosFuncLOGIN(@SpCPFFunc);";
            cmd.Parameters.Add("@SpCPFFunc", MySqlDbType.VarChar).Value = CPF;
            cmd.Connection = conexao;
            var leituraFuncioCPF = cmd.ExecuteReader();
            var TempFuncionario = new Funcionario();

            if (leituraFuncioCPF.Read())
            {
                TempFuncionario.CPF = leituraFuncioCPF["CPF"].ToString();
                TempFuncionario.Nome = leituraFuncioCPF["NomeFunc"].ToString();
                TempFuncionario.Senha = leituraFuncioCPF["Senha"].ToString();
            }
            leituraFuncioCPF.Close();
            conexao.Close();
            return TempFuncionario;
        }

        //VERIFICA SE JÁ EXISTE O CPF DO FUNCIONÁRIO DURANTE O FORMULÁRIO DE CADASTRO
        public string verificaFuncioCPF(string vCPF)
        {
            conexao.Open();
            cmd.CommandText = "call spDadosFunc2(@spCPFFunc);";
            cmd.Parameters.Add("@spCPFFunc", MySqlDbType.VarChar).Value = vCPF;
            cmd.Connection = conexao;
            string CPF = (string)cmd.ExecuteScalar();
            conexao.Close();
            if (CPF == null)
                CPF = "";
            return CPF;
        }

        public int VerificaFuncioIdExiste(string CPF, string senha)
        {
            conexao.Open();
            cmd.CommandText = "select IdFunc, CPF, Senha from tbfuncionario where CPF = @CPF ";
            cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = CPF;
            cmd.Connection = conexao;

            var leituraFuncCPF = cmd.ExecuteReader();
            var TempFunc = new Funcionario();

            if (leituraFuncCPF.Read())
            {
                TempFunc.IdFunc = int.Parse(leituraFuncCPF["IdFunc"].ToString());
                TempFunc.Senha = leituraFuncCPF["Senha"].ToString();
            }
            leituraFuncCPF.Close();

            conexao.Close();

            if (Hash.CompareBCrypt(senha, TempFunc.Senha))
                return TempFunc.IdFunc;
            else
                return 0;
            
        }

        public bool isFunc(string CPF)
        {
            conexao.Open();
            cmd.CommandText = "SELECT * FROM tbfuncionario WHERE CPF = @CPF;";
            cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = CPF;
            cmd.Connection = conexao;

            var readFunc = cmd.ExecuteReader();

            if (readFunc.Read())
            {
                readFunc.Close();
                conexao.Close();
                return true;
            }
            else
            {
                readFunc.Close();
                conexao.Close();
                return false;
            }
        }

        //FIM MÉTODO DE LISTAGEM

        //MÉTODO PARA APAGAR O FUNCIONÁRIO, UTILIZANDO A MESMA ESTRUTURA DO CADASTRO SEM RETORNO
        public void DeletFuncio (int IdFunc)
        {
            conexao.Open();
            cmd.CommandText = "call spDeleteFunc(@spIdProd);";
            cmd.Parameters.Add("@spIdProd", MySqlDbType.Int16).Value = IdFunc;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }
        //MÉTODO PARA ALTERAR O FUNCIONÁRIO, UTILIZANDO A MESMA ESTRUTURA DO CADASTRI SEM RETORNO
        public void AlterFuncio (Funcionario funcio)
        {
            conexao.Open();
            cmd.CommandText = "call spUpdateFunc2(@spnome, @spdata, @spCPF, @spcargo);";
            cmd.Parameters.Add("@spnome", MySqlDbType.VarChar).Value = funcio.Nome;
            cmd.Parameters.Add("@spdata", MySqlDbType.DateTime).Value = funcio.DataNasc;
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = funcio.CPF;
            cmd.Parameters.Add("@spcargo", MySqlDbType.VarChar).Value = funcio.Cargo;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        
    }
}
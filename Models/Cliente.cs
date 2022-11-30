using Antlr.Runtime.Misc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Cliente : Usuario
    {
        
        public string EmailCli { get; set; }

        public string TelCli { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexaobd"].ConnectionString);
        MySqlCommand cmd = new MySqlCommand();

        //MÉTODO PARA ADICIONAR O CLIENTE
        public void CriarCliente(Cliente cliente)
        {
            //ABRE A CONEXÃO
            conexao.Open();
            //ABAIXO IRÁ PEGAR A SP DO BANCO E INSERIR OS PARÂMETROS ENVIADOS PELO FORMULÁRIO
            cmd.CommandText = "call spInsertCliente(@spCPF, @spNome, @spData, @spSenha, @spEmail, @sptelCli);";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = cliente.CPF;
            cmd.Parameters.Add("@spNome", MySqlDbType.VarChar).Value = cliente.Nome;
            cmd.Parameters.Add("@spData", MySqlDbType.DateTime).Value =cliente.DataNasc;
            cmd.Parameters.Add("@spSenha", MySqlDbType.VarChar).Value = cliente.Senha;
            cmd.Parameters.Add("@spEmail", MySqlDbType.VarChar).Value = cliente.EmailCli;
            cmd.Parameters.Add("@sptelCli", MySqlDbType.VarChar).Value = cliente.TelCli;
            cmd.Connection = conexao;
            //APÓS REALIZAR A CONEXÃO, REALIZA A FUNÇÃO DE ADIÇÃO E FECHA A CONEXÃO DEPOIS
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //Método de LISTAGEM

        
        //MÉTODOS DE LISTAGEM DE TODOS OS CUPONS
        public List<Cliente> ListarCli()
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "select * from tbcliente;";
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var read = cmd.ExecuteReader();
            List<Cliente> tempCliLista = new List<Cliente>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (read.Read())
            {
                var tempCli = new Cliente();

                tempCli.CPF = read["CPF"].ToString();
                tempCli.Nome = read["NomeCliente"].ToString();
                tempCli.DataNasc = DateTime.Parse(read["DataNasc"].ToString());
                tempCli.EmailCli = read["EmailCli"].ToString();
                tempCli.TelCli = read["TelCli"].ToString();

                tempCliLista.Add(tempCli);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            read.Close();
            conexao.Close();

            return tempCliLista;
        }

        public List<Cliente> ListarCliPeloCPF(string CPFCli)
        {
            //IRÁ ABRIR A CONEXÃO E UTILIZAR TODA A TABELA PARA REGISTROS E É AUTENTICADO A CONEXÃO COM O CONNECTION
            conexao.Open();
            cmd.CommandText = "call spPesquisaCPFCliente(@spCPF)";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = CPFCli;
            cmd.Connection = conexao;

            //É INSERIDO OS REGISTROS DO BANCO PELO ExecuteReader()
            var read = cmd.ExecuteReader();
            List<Cliente> tempCliLista = new List<Cliente>();

            //ENQUANTO TIVER REGISTRO NA read, irá adicionar os itens em uma lista com as variáveis abaixo do banco.
            while (read.Read())
            {
                var tempCli = new Cliente();

                tempCli.CPF = read["CPF"].ToString();
                tempCli.Nome = read["NomeCliente"].ToString();
                tempCli.DataNasc = DateTime.Parse(read["DataNasc"].ToString());
                tempCli.EmailCli = read["EmailCli"].ToString();
                tempCli.TelCli = read["TelCli"].ToString();

                tempCliLista.Add(tempCli);
            }
            //É FECHADO A LEITURA DA VARIÁVEL read E TAMBÉM DA CONEXÃO DO BANCO, RETORNANDO A LISTA DE DIVERSOS REGISTROS
            read.Close();
            conexao.Close();

            return tempCliLista;
        }

        //SEMELHANTE AO ListarCli, PORÉM APENAS UM CLIENTE PELO SEU PARÂMETRO
        public Cliente ListaUMCliente(string CPF)
        {
            conexao.Open();
            cmd.CommandText = "call spDadosCliente(@spCPF);";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = CPF;
            cmd.Connection = conexao;
            var leituraClienCPF = cmd.ExecuteReader();
            var TempCliente = new Cliente();

            if (leituraClienCPF.Read())
            {
                TempCliente.CPF = leituraClienCPF["CPF"].ToString();
                TempCliente.Nome = leituraClienCPF["NomeCliente"].ToString();
                TempCliente.DataNasc = DateTime.Parse(leituraClienCPF["DataNasc"].ToString());
                TempCliente.EmailCli = leituraClienCPF["EmailCli"].ToString();
                TempCliente.TelCli = leituraClienCPF["TelCli"].ToString();
            }
            leituraClienCPF.Close();
            conexao.Close();
            return TempCliente;
        }

        //MÉTODO PARA VERIFICAR A EXISTÊNCIA DO CPF FORMULÁRIO POR UMA PROCEDURE
        public string verificaClienCPF(string vCPF)
        {
            conexao.Open();
            cmd.CommandText = "call spDadosCliente(@spCPF);";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = vCPF;
            cmd.Connection = conexao;
            string CPF = (string)cmd.ExecuteScalar();
            conexao.Close();
            if (CPF == null)
                CPF = "";
            return CPF;
        }

        //FIM DOS MÉTODOS LISTAGEM

        //ALTERAÇÃO DO CLIENTE, SEMELHANTE AO CADASTRO DO CLIENTE

        public void AlterCli(Cliente cli)
        {
            conexao.Open();
            cmd.CommandText = "call spUpdateCli(@spCPF, @spnome, @spdata, @spemail, @sptel)";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = cli.CPF;
            cmd.Parameters.Add("@spnome", MySqlDbType.VarChar).Value = cli.Nome;
            cmd.Parameters.Add("@spdata", MySqlDbType.DateTime).Value = cli.DataNasc;
            cmd.Parameters.Add("@spemail", MySqlDbType.VarChar).Value = cli.EmailCli;
            cmd.Parameters.Add("@sptel", MySqlDbType.VarChar).Value = cli.TelCli;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //FIM DA ALTERAÇÃO

        //APAGAR O CLIENTE, SEMELHANTE AO CADASTRO DO CLIENTE

        public void DeletCli(Cliente cli)
        {
            conexao.Open();
            cmd.CommandText = "call spDeleteCli(@spCPF);";
            cmd.Parameters.Add("@spCPF", MySqlDbType.VarChar).Value = cli.CPF;
            cmd.Connection = conexao;
            cmd.ExecuteNonQuery();
            conexao.Close();
        }

        //FIM DO MÉTODO DE DELETAR




    }
}
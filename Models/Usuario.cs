using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    //SERVE APENAS COMO UMA CLASSE MÃE PARA CLIENTE E FUNCIONÁRIO
    public abstract class Usuario
    {
        //public Usuario()
        //{

        //}


        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNasc { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        
        
    }
}
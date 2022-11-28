using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCC_Sistema_Cliente_Jogos_2022.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace TCC_Sistema_Cliente_Jogos_2022.ViewModels
{
    //Utilizada essa ViewModel para adaptar campos que não estão no banco de dados
    public class ClienteViewModel
    {
        [Display(Name = "CPF do Cliente")]
        [RegularExpression(@"^(\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}|\d{3}\.?\d{3}\.?\d{3}-?\d{2})$", ErrorMessage = "O formato do CPF está incorreto")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O campo deve conter 14 caracteres")]
        [Remote("verificaCPFCliente", "Cliente", ErrorMessage = "O CPF JÁ EXISTE!")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string CPF { get; set; }

        [Display(Name = "Nome do Cliente")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O campo deve conter o mínimo entre 2 a 50 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/YYYY}", ApplyFormatInEditMode = true)]
        public DateTime DataNasc { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [MaxLength(100, ErrorMessage = "a senha deve conter no máximo 100 caracteres")]
        [MinLength(5, ErrorMessage = "a senha deve conter no mínimo 5 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        //ATRIBUTO QUE NÃO IRÁ SER ENVIADO AO BANCO, SERVINDO COMO UMA CAMADA EXTRA DE SEGURANÇA
        [Display(Name = "Confirmar a Senha")]
        [Required(ErrorMessage = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare(nameof(Senha), ErrorMessage = "Senhas estão diferente! ")]
        public string ConfirmSenha { get; set; }

        [Display(Name = "Email do Cliente")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "O formato do email está incorreto")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string EmailCli { get; set; }

        [Display(Name = "Celular do Cliente")]
        //[RegularExpression(@"^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$", ErrorMessage = "O formato do número está incorreto")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string TelCli  { get; set; }

        
    }
}
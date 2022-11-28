using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace TCC_Sistema_Cliente_Jogos_2022.ViewModels
{
    public class FuncionarioViewModel
    {

        [Display(Name = "Nome do Funcionário")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O campo deve conter o mínimo entre 2 a 50 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string NomeFunc { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/YYYY}", ApplyFormatInEditMode = true)]
        public DateTime DataNasc { get; set; }

        [Display(Name = "CPF do Funcionário")]
        [RegularExpression(@"^(\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}|\d{3}\.?\d{3}\.?\d{3}-?\d{2})$", ErrorMessage = "O formato do CPF está incorreto")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O campo deve conter 14 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Remote("verificaCPFFuncio", "Funcionario", ErrorMessage = "O CPF JÁ EXISTE!")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [MaxLength(100, ErrorMessage = "a senha deve conter no máximo 100 caracteres")]
        [MinLength(5, ErrorMessage = "a senha deve conter no mínimo 5 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        //ATRIBUTO NÃO IRÁ SER ENVIADO AO BANCO, APENAS COMO UMA CAMADA EXTRA DE SEGURANÇA
        [Display(Name = "Confirmar a Senha")]
        [Required(ErrorMessage = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare(nameof(Senha), ErrorMessage = "Senhas estão diferente!")]
        public string ConfirmSenha { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Cargo do Funcionário")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O campo deve conter o mínimo entre 2 a 50 caracteres")]
        public string Cargo { get; set; }

        

    }
}
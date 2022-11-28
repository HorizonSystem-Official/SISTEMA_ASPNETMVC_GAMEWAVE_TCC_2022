using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCC_Sistema_Cliente_Jogos_2022.ViewModels
{
    public class LoginViewModel
    {
        //ATRIBUTO PARA NÍVEIS DE ACESSO
        public string ControleAcesso { get; set; }

        //ATRIBUTO PARA CONTROLE DA URL ENQUANTO AUTENTICADO
        public string UrlRetorno { get; set; }
        
        [Required(ErrorMessage = "Informe o CPF!")]
        [RegularExpression(@"^(\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}|\d{3}\.?\d{3}\.?\d{3}-?\d{2})$", ErrorMessage = "O formato do CPF está incorreto")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O campo deve conter 14 caracteres")]
        [Display(Name = "CPF para login")]
        public string CPFLogin { get; set; }

        [Display(Name = "Senha para login")]
        [Required(ErrorMessage = "Informe a Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [MaxLength(100, ErrorMessage = "a senha deve conter no máximo 100 caracteres")]
        [DataType(DataType.Password)]
        public string SenhaLogin { get; set; }
    }
}
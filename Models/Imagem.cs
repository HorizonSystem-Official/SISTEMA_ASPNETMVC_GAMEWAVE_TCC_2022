using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCC_Sistema_Cliente_Jogos_2022.Models
{
    public class Imagem
    {
        public Imagem()
        {

        }


        [Display(Name = "Imagem para o Produto")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string LinkImg { get; set; }

        [Display(Name = "Categoria da Imagem")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo contém entre 3 a 50 caracteres")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string CatImg { get; set; }

        [Display(Name = "Código do Produto")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Fk_tbProduto_CodProd { get; set; }

    }
}
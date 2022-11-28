using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TCC_Sistema_Cliente_Jogos_2022.ViewModels
{
    public class DetalhesProdutoEComentarios
    {

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

        //PARTE DA MODEL DO COMENTÁRIO

        [Display(Name = "Comentário")]
        [Required(ErrorMessage = "É preciso escrever algo neste campo para ser postado")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "O campo deve conter entre 3 a 300 caracteres")]
        public string TxtComentario { get; set; }

        [Display(Name = "Código do produto")]
        public int Fk_CodProd { get; set; }

        [Display(Name = "Código via CPF do cliente")]
        public string Fk_CpfCli { get; set; }

        //Parte da MODEL Cliente

        public string Nome { get; set; }


    }
}
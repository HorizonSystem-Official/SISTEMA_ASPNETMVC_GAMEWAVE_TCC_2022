using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_Sistema_Cliente_Jogos_2022.Models;

namespace TCC_Sistema_Cliente_Jogos_2022.Controllers
{
    public class CupomController : Controller
    {
        // GET: Cupom

        //FAZ A CONSULTA DE CUPONS SEM FILTRO
        public ActionResult ConsulCupom()
        {
            var cup = new Cupom();

            var MostrarCupom = cup.ListarCup();

            return View(MostrarCupom);
        }


        //FORMULÁRIO PARA ADICIONAR O CUPOM
        public ActionResult CadCupom()
        {
            return View();
        }

        //REALIZA O CADASTRO DO CUPOM
        [HttpPost]
        public ActionResult CadCupom(Cupom cup)
        {
            
            if (!ModelState.IsValid)
            {
                return View(cup);
            }
            Cupom cupom = new Cupom
            {
                CupomTxt = cup.CupomTxt,
                ValorCupom = cup.ValorCupom,
                NumLimiteCompras = cup.NumLimiteCompras,
            };
            //É ENVIADO UM OBJETO COM OS DADOS DO PARÂMETRO E REDIRECIONA O USUÁRIO PARA A TABELA DE CONSULTA COM UMA NOTIFICAÇÃO DA TEMPDATA
            cupom.CadastrarCup(cup);

            TempData["MensagemAviso"] = "Cadastro do cupom feito com sucesso!";
            return RedirectToAction("ConsulCupom");
        }

        //FORMULÁRIO PARA ALTERAR O CUPOM ESPECÍFICO PELO PARÂMETRO
        [HttpGet]
        public ActionResult EdiCupom(int codcupom)
        {
            Cupom cup = new Cupom();

            var umcupom = cup.ListaUMCupom(codcupom);

            if (umcupom == null)
                return HttpNotFound();

            return View(umcupom);
        }


        //MÉTODO PARA REALIZAR A ALTERAÇÃO DO CUPOM
        [HttpPost]
        public ActionResult EdiCupom(Cupom cupom)
        {
            if (!ModelState.IsValid)
            {
                return View(cupom);
            }
            Cupom cup = new Cupom();

            cup.AlterarCup(cupom);

            //É ENVIADO UM OBJETO COM OS DADOS ALTERADOS E REDIRECIONA O USUÁRIO PARA A TABELA DE CONSULTA COM UMA NOTIFICAÇÃO DA TEMPDATA
            TempData["MensagemAviso"] = "Alteração do Cupom feito com sucesso!";
            return RedirectToAction("ConsulCupom");
        }

        //FAZ APAGAR O CUPOM
        
        public ActionResult DelCupom(int codcupom)
        {
            var cup = new Cupom();

            cup.DeletarCup(codcupom);


            //EMITE UM AVO AO USUÁRIO SOBRE O CUPOM APAGADO E REDIRECIONA PARA A CONSULTA DELES
            TempData["MensagemAviso"] = "Cupom deletado com sucesso!";
            return RedirectToAction("ConsulCupom");
        }

    }
}
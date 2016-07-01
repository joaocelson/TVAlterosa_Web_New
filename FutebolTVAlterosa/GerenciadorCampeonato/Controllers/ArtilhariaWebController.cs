using GerenciadorCampeonato.Controllers.Abstratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Campeonato.Business.Logics;
using Campeonato.Dominio;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections;
using GerenciadorCampeonato.Controllers.Abstratos;

namespace GerenciadorCampeonato.Controllers
{
    public class ArtilhariaWebController : BaseController
    {
        ArtilhariaBusiness artilhariaBusiness = new ArtilhariaBusiness();
        JogadorBusiness jogadorBusiness = new JogadorBusiness();
        TimeBusiness timeBusiness = new TimeBusiness();
        CampeonatoBusiness campeonatoBusiness = new CampeonatoBusiness();


        public ArtilhariaWebController()
        {
            List<Jogador_Web> listaJogador = jogadorBusiness.ObterJogador();
            Jogador_Web valorPrincipalJogador = new Jogador_Web { Id = 0, Nome = "Selecione um valor" };
            listaJogador.Add(valorPrincipalJogador);
            var jogadores = listaJogador.Select(i => new { id = i.Id, descricao = i.Nome });
            ViewData["JogadorControle_Data"] = new SelectList(jogadores, "id", "descricao");

            List<Time_Web> listaTime = timeBusiness.ObterTime();
            Time_Web valorPrincipalTime = new Time_Web { Id = 0, Nome = "Selecione um valor" };
            listaTime.Add(valorPrincipalTime);
            var times = listaTime.Select(i => new { id = i.Id, descricao = i.Nome });
            ViewData["TimeControle_Data"] = new SelectList(times, "id", "descricao");

            List<Campeonato_Web> listaCampeonato = campeonatoBusiness.ObterCampeonato();
            Campeonato_Web valorPrincipal = new Campeonato_Web { Id = 0, Nome = "Selecione um valor" };
            listaCampeonato.Add(valorPrincipal);
            var campeonatos = listaCampeonato.Select(i => new { id = i.Id, descricao = i.Nome });
            ViewData["CampeonatoControle_Data"] = new SelectList(campeonatos, "id", "descricao");
        }

        public ActionResult Consultar()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                return View("~/Views/Artilharia/Index.cshtml");
            else
                return View("~/Views/AdminHome/Login.cshtml");
            
        }

        public ActionResult Ler([DataSourceRequest] DataSourceRequest request)
        {
            List<Artilharia_Web> artilharia_Web = new List<Artilharia_Web>();

            try
            {
                artilharia_Web = artilhariaBusiness.ObterArtilharia();

                return Json(artilharia_Web.ToList().ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(artilharia_Web.ToList().ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Inserir([DataSourceRequest] DataSourceRequest request, Artilharia_Web item)
        {
            try
            {
                artilhariaBusiness.InserirArtilharia(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Atualizar([DataSourceRequest] DataSourceRequest request, Artilharia_Web item)
        {
            try
            {
                artilhariaBusiness.AtualizaArtilharia(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }

        }

        public ActionResult Apagar([DataSourceRequest] DataSourceRequest request, Artilharia_Web item)
        {
            try
            {
                artilhariaBusiness.RemoveArtilharia(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
        }

        //public FileResult Exportar([DataSourceRequest] DataSourceRequest request, string arquivo, string colunas)
        //{
        //    var lista = jogadorBusiness.ObterEquipe();
        //    return base.Exportar(request, lista != null ? (IEnumerable)lista : null, arquivo, colunas);
        //}

    }
}
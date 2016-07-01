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
    public class PartidaWebController : BaseController
    {

        PartidaBusiness partidaBusiness = new PartidaBusiness();
        TimeBusiness timeBusiness = new TimeBusiness();        
        CampeonatoBusiness campeonatoBusiness = new CampeonatoBusiness();


        public PartidaWebController()
        {
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
                return View("~/Views/Partidas/Index.cshtml");
            else
                return View("~/Views/AdminHome/Login.cshtml");               
        }

        public ActionResult ObterTimes()
        {
            List<Time_Web> listaTime = timeBusiness.ObterTime();
            Time_Web valorPrincipalTime = new Time_Web { Id = 0, Nome = "Selecione um valor" };
            listaTime.Add(valorPrincipalTime);
            var times = listaTime.Select(i => new { id = i.Id, descricao = i.Nome });

            return Json(times, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterCampeonato()
        {
            List<Campeonato_Web> listaCampeonato = campeonatoBusiness.ObterCampeonato();
            var times = listaCampeonato.Select(i => new { id = i.Id, descricao = i.Nome });

            return Json(times, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ler([DataSourceRequest] DataSourceRequest request)
        {
            var campeonato = (Request.Params["campeonato"]);
            var dataInicio = (Request.Params["dataInicio"]);
            var dataFim = (Request.Params["dataFim"]);            

            List<Partida_Web> partida_Web = new List<Partida_Web>();
            DataSourceResult dataSource = new DataSourceResult();

            try
            {
                if (dataInicio == "" || dataInicio == null)
                {
                    ModelState.AddModelError(String.Empty, "O data inicial do período de saída deve ser informada");
                    dataSource = partida_Web.ToDataSourceResult(request, ModelState);
                    return Json(dataSource, JsonRequestBehavior.AllowGet);
                }

                if (dataFim == "" || dataFim == null)
                {
                    ModelState.AddModelError(String.Empty, "O data final do período de saída deve ser informada");
                    dataSource = partida_Web.ToDataSourceResult(request, ModelState);
                    return Json(dataSource, JsonRequestBehavior.AllowGet);
                }

                if (DateTime.Parse(dataInicio) > DateTime.Parse(dataFim))
                {
                    ModelState.AddModelError(String.Empty, "O data inicial do período de saída deve menor que a data final");
                    dataSource = partida_Web.ToDataSourceResult(request, ModelState);
                    return Json(dataSource, JsonRequestBehavior.AllowGet);
                }

                partida_Web = partidaBusiness.ObterPartida(campeonato, dataInicio, dataFim);

                return Json(partida_Web.ToList().ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(partida_Web.ToList().ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Inserir([DataSourceRequest] DataSourceRequest request, Partida_Web item)
        {
            try
            {
                partidaBusiness.InserirPartida(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Atualizar([DataSourceRequest] DataSourceRequest request, Partida_Web item)
        {
            try
            {
                partidaBusiness.AtualizaPartida(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }

        }

        public ActionResult Apagar([DataSourceRequest] DataSourceRequest request, Partida_Web item)
        {
            try
            {
                partidaBusiness.RemovePartida(item);

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
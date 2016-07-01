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
using Campeonato.Aplicacao;

namespace GerenciadorCampeonato.Controllers
{
    public class ResultadoPartidaWebController : BaseController
    {

        PartidaBusiness partidaBusiness = new PartidaBusiness();
        TimeBusiness timeBusiness = new TimeBusiness();
        CampeonatoBusiness campeonatoBusiness = new CampeonatoBusiness();
        private readonly PartidaAplicacao appPartida;
        
        public ResultadoPartidaWebController()
        {
            appPartida = PartidaAplicacaoConstrutor.PartidaAplicacaoADO();

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
                return View("~/Views/ResultadoPartidas/Index.cshtml");
            else
                return View("~/Views/AdminHome/Login.cshtml");
        }

        public ActionResult Ler([DataSourceRequest] DataSourceRequest request)
        {
            var campeonato = (Request.Params["campeonato"]);
            var dataInicio = (Request.Params["dataInicio"]);
            var dataFim = (Request.Params["dataFim"]);

            List<ResultadoPartida_Web> resultadoPartida_Web = new List<ResultadoPartida_Web>();
            DataSourceResult dataSource = new DataSourceResult();

            try
            {
                if (dataInicio == "" || dataInicio == null)
                {
                    ModelState.AddModelError(String.Empty, "O data inicial do período de saída deve ser informada");
                    dataSource = resultadoPartida_Web.ToDataSourceResult(request, ModelState);
                    return Json(dataSource, JsonRequestBehavior.AllowGet);
                }

                if (dataFim == "" || dataFim == null)
                {
                    ModelState.AddModelError(String.Empty, "O data final do período de saída deve ser informada");
                    dataSource = resultadoPartida_Web.ToDataSourceResult(request, ModelState);
                    return Json(dataSource, JsonRequestBehavior.AllowGet);
                }

                if (DateTime.Parse(dataInicio) > DateTime.Parse(dataFim))
                {
                    ModelState.AddModelError(String.Empty, "O data inicial do período de saída deve menor que a data final");
                    dataSource = resultadoPartida_Web.ToDataSourceResult(request, ModelState);
                    return Json(dataSource, JsonRequestBehavior.AllowGet);
                }

                resultadoPartida_Web = partidaBusiness.ObterPartidaResultado(campeonato, dataInicio, dataFim);

                return Json(resultadoPartida_Web.ToList().ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(resultadoPartida_Web.ToList().ToDataSourceResult(request, ModelState));
            }
        }        

        public ActionResult Atualizar([DataSourceRequest] DataSourceRequest request, ResultadoPartida_Web item)
        {
            Partida_Web obj = new Partida_Web();
            obj = partidaBusiness.ObterPartidaId(item.Id);
            Partida partidaBancoDados = appPartida.ListarPorId(item.Id.ToString());
            try
            {
                obj.Id = item.Id;
                obj.IdTimeMandante = item.IdTimeMandante;
                obj.IdTimeVisitante = item.IdTimeVisitante;
                obj.Id_Campeonato = item.Id_Campeonato;
                obj.GolMandante = item.GolMandante;
                obj.GolVisitante = item.GolVisitante;
                obj.DataPartida = item.DataPartida;
                                                
                partidaBancoDados.GolMandante = item.GolMandante;
                partidaBancoDados.GolVisitante = item.GolVisitante;
                partidaBancoDados.DataPartida = item.DataPartida;

                appPartida.Resultado(partidaBancoDados);

                partidaBusiness.AtualizaPartida(obj);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }

        }        

    }
}
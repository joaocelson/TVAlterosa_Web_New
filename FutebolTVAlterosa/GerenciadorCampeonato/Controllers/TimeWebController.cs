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
using System.IO;
using System.Drawing;

namespace GerenciadorCampeonato.Controllers
{
    public class TimeWebController : BaseController
    {

        TimeBusiness timeBusiness = new TimeBusiness();

        public ActionResult Consultar()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                return View("~/Views/Time/Index.cshtml");
            else
                return View("~/Views/AdminHome/Login.cshtml");              
        }

        public ActionResult AnexarEscudo(string id)
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                return View("~/Views/Time/AnexarEscudo.cshtml");
            else
                return View("~/Views/AdminHome/Login.cshtml");
        }

        public ActionResult Ler([DataSourceRequest] DataSourceRequest request)
        {
            List<Time_Web> time_Web = new List<Time_Web>();

            try
            {
                time_Web = timeBusiness.ObterTime();

                return Json(time_Web.ToList().ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(time_Web.ToList().ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Inserir([DataSourceRequest] DataSourceRequest request, Time_Web item)
        {
            try
            {
                timeBusiness.InserirTime(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Atualizar([DataSourceRequest] DataSourceRequest request, Time_Web item)
        {
            try
            {
                timeBusiness.AtualizaTime(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }

        }

        public ActionResult Apagar([DataSourceRequest] DataSourceRequest request, Time_Web item)
        {
            try
            {
                timeBusiness.RemoveTime(item);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return Json(new[] { item }.ToDataSourceResult(request, ModelState));
            }
        }

        public ActionResult Submit(string files)
        {
            byte[] byteArrayIn = System.Text.Encoding.ASCII.GetBytes(files);
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/imagens"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/imagens");
            }
            returnImage.Save(AppDomain.CurrentDomain.BaseDirectory + "/imagens/" + files);
            return null;
        }

        //public FileResult Exportar([DataSourceRequest] DataSourceRequest request, string arquivo, string colunas)
        //{
        //    var lista = equipeBusiness.ObterEquipe();
        //    return base.Exportar(request, lista != null ? (IEnumerable)lista : null, arquivo, colunas);
        //}

    }
}
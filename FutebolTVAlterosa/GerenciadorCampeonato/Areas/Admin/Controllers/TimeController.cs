using Campeonato.Aplicacao;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campeonato.UI.WEB.Areas.Admin
{
    public class TimeController : Controller
    {
        private readonly TimeAplicacao appTime;
        private readonly FotoVideoAplicacao appFotosVideos;

        public TimeController()
        {
            appTime = TimeAplicacaoConstrutor.TimeAplicacaoADO();
            appFotosVideos = FotoVideoAplicacaoConstrutor.FotoVideoADO();
        }

        public ActionResult Index()
        {
            var listaDeTimes = appTime.ListarTodos();
            return View(listaDeTimes);
        }

        public ActionResult ListarTimesCampeonato(String idCampeonato)
        {
            var listaDeTimes = appTime.ListarTimesCampeonato(idCampeonato);
            return View(listaDeTimes);
        }

        public JsonResult ListarTimesCampeonatoJSON(String idCampeonato)
        {
            var listaDeTimes = appTime.ListarTimesCampeonato(idCampeonato);
            return Json(listaDeTimes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Time Time)
        {
            AtualizarImagem(Time);
            return RedirectToAction("Index");
        }

        private void AtualizarImagem(Time Time)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase arquivo = Request.Files[i];
                //Suas validações ...... //Salva o arquivo 
                if (arquivo.ContentLength > 0)
                {
                    var uploadPath = Server.MapPath("~/Content/images/escudo/");
                    string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivo.FileName));
                    arquivo.SaveAs(caminhoArquivo);
                    Time.EscudoPequeno = arquivo.FileName;
                }
            }

            appTime.Salvar(Time);
        }

        public ActionResult Editar(string id)
        {
            var Time = appTime.ListarPorId(id);

            if (Time == null)
                return HttpNotFound();

            return View(Time);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Time Time)
        {
            AtualizarImagem(Time);
            return RedirectToAction("Index");
        }

        public ActionResult Detalhes(string id)
        {
            var Time = appTime.ListarPorId(id);
            Time.ListaFotosVideos = appFotosVideos.ListarTime(id);
            if (Time == null)
                return HttpNotFound();

            return View(Time);
        }

        public ActionResult Excluir(string id)
        {
            var Time = appTime.ListarPorId(id);

            if (Time == null)
                return HttpNotFound();

            return View(Time);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirmado(string id)
        {
            var Time = appTime.ListarPorId(id);
            appTime.Excluir(Time);
            return RedirectToAction("Index");
        }

        //GETIMAGENS
        public ActionResult Image(string nomeImagem)
        {
            var dir = Server.MapPath("/Content/images/escudo");
            var path = Path.Combine(dir, nomeImagem);
            return base.File(path, "image/jpg");
        }
    }

}

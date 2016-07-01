using Campeonato.Aplicacao;
using Campeonato.Dominio;
using Campeonato.UI.WEB.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Campeonato.UI.WEB.Areas.Admin
{
    public class GaleriaController : Controller
    {

        private readonly FotoVideoAplicacao appFotoVideo;
        private readonly TimeAplicacao appTime;

        //
        // GET: /Galeria/
        public GaleriaController()
        {
            appTime = TimeAplicacaoConstrutor.TimeAplicacaoADO();
            appFotoVideo = FotoVideoAplicacaoConstrutor.FotoVideoADO();
        }

        public ActionResult Index()
        {
            var listaFotosPorData = appFotoVideo.ListarDataFotoVideo();
            return View(listaFotosPorData);
        }

        public ActionResult PaginaInicial()
        {
            return View();
        }

        public ActionResult Galeria(string data)
        {
            var listaFotosPorData = appFotoVideo.ListarFotoVideoPorData(data);
            return View(listaFotosPorData);
        }

        public ActionResult GaleriaBase()
        {
            return View();
        }

        public ActionResult GaleriaVeteranos()
        {
            return View();
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult FotosVideos(Partida partida)
        {
            //Trannsformo o objeto de partida em foto para salvar no banco de dados.
            FotosVideos fotosVideos = new FotosVideos();
            fotosVideos.IdCampeonato = partida.IdCampeonato;
            fotosVideos.IdPartida = partida.Id.ToString();
            fotosVideos.Rodada = partida.Rodada;
            fotosVideos.Caminho = partida.EscudoPequenoMandante;
            fotosVideos.Video = partida.GolMandante;
            fotosVideos.Descricao = partida.NomeCampeonato;
            if (partida.IdTimeMandante != null)
            {
                fotosVideos.IdTime = partida.IdTimeMandante;
            }
            else
            {
                fotosVideos.IdTime = partida.IdTimeVisitante;
            }

            if (appFotoVideo.Inserir(fotosVideos))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("FotosVideos", new RouteValueDictionary(
  new { controller = "Partida", action = "FotosVideos", id = partida.Id }));
            }
        }


        public ActionResult ListarFotosVideosParaEditar()
        {
            var listaFotosPorData = appFotoVideo.ListarFotosVideosParaEditar();
            return View(listaFotosPorData);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar()
        {
            var listaTimes = appTime.ListarTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar(FotosVideos fotos)
        {
            return View();
        }

    }
}
using Campeonato.Aplicacao;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campeonato.UI.WEB.Controllers
{
    public class PaginaController : Controller
    {
        //
        // GET: /Home/

        private readonly ClassificacaoAplicacao appClassificacao;
        private readonly PartidaAplicacao appPartida;
        private readonly AcessoAplicacao appAcesso;
        private readonly CampeonatoAplicacao appCampeonato;
        private readonly FotoVideoAplicacao appFotoVideo;


        public PaginaController()
        {
            appClassificacao = ClassificacaoAplicacaoConstrutor.ClassificacaoAplicacaoADO();
            appPartida = PartidaAplicacaoConstrutor.PartidaAplicacaoADO();
            appAcesso = AcessoAplicacaoConstrutor.AcessoAplicacaoADO();
            appCampeonato = CampeonatoAplicacaoConstrutor.CampeonatoAplicacaoADO();
            appFotoVideo = FotoVideoAplicacaoConstrutor.FotoVideoADO();
        }

        public ActionResult Index()
        {
            appAcesso.AtualizarNumeroAcesso();
            //Listar Chamado Atual
            //var listaProximaRodada = appPartida.ListarUltimaRodada();

            //Listar próxima rodada
            var listaProximaRodada = appPartida.ListarUltimaRodada();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                listaProximaRodada = appPartida.ListarUltimaRodada();
            }
            else
            {
                listaProximaRodada = appPartida.ListarProximaRodada();
            }


            return View(listaProximaRodada);
        }

        public ActionResult Tabela()
        {
            var tabela = appPartida.ListarTodos();
            return View(tabela);
        }

        public ActionResult Galeria()
        {
            var listaFotosVideos = 0;
            return View(listaFotosVideos);
        }

        public ActionResult Contato()
        {

            return View();
        }

        public ActionResult About()
        {

            return View();
        }
       
    }
}

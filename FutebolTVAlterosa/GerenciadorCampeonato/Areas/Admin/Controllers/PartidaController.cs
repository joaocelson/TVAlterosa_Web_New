using Campeonato.Aplicacao;
using Campeonato.Dominio;
using Campeonato.Dominio.util;
using Campeonato.UI.WEB.Security;
using GerenciadorCampeonato.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace Campeonato.UI.WEB.Areas.Admin
{
    public class PartidaController : Controller
    {
        private readonly PartidaAplicacao appPartida;
        private readonly CampeonatoAplicacao appCampeonato;
        private readonly TimeAplicacao appTime;
        private readonly JogadorAplicacao appJogador;
        public PartidaController()
        {
            appPartida = PartidaAplicacaoConstrutor.PartidaAplicacaoADO();
            appCampeonato = CampeonatoAplicacaoConstrutor.CampeonatoAplicacaoADO();
            appTime = TimeAplicacaoConstrutor.TimeAplicacaoADO();
            appJogador = JogadorAplicacaoConstrutor.JogadorAplicacaoADO();
        }


        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Index()
        {
            var listaDePartida = appPartida.ListarTodos();
            var listaDeCampeonatos = appCampeonato.ListarTodos();
            ViewBag.DropDownValues = new SelectList(listaDeCampeonatos, "id", "Nome");
            return View(listaDePartida);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult PaginaInicial()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult GerarPartidas(FormCollection form)
        {

            string idCampeonatoSelecionado = form["MeusCampeonatos"];
            appPartida.GerarPartidasAutomaticamente(idCampeonatoSelecionado);
            return Redirect("Index");
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar(Partida partida)
        {
            if (ModelState.IsValid)
            {
                appPartida.Salvar(partida);
                return RedirectToAction("Index");
            }
            return View(partida);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Editar(string id)
        {
            var partida = appPartida.ListarPorId(id);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Editar(Partida partida)
        {
            //if (ModelState.IsValid)
            //{
            if (partida.InverterMandante)
            {
                appPartida.InverterManadate(ref partida);
            }
            appPartida.Salvar(partida);
            //return RedirectToAction("Index");
            //}
            return RedirectToAction("Index");
        }

        public ActionResult Detalhes(string id)
        {
            var partida = appPartida.ListarPorId(id);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Excluir(string id)
        {
            var Partida = appPartida.ListarPorId(id);

            if (Partida == null)
                return HttpNotFound();

            return View(Partida);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult ExcluirConfirmado(string id)
        {
            var partida = appPartida.ListarPorId(id);
            appPartida.Excluir(partida);
            return RedirectToAction("Index");
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Resultado(string id)
        {
            var partida = appPartida.ListarPorId(id);
            partida.TimeMandanteObj = appTime.ListarPorId(partida.IdTimeMandante);
            partida.TimeVisitanteObj = appTime.ListarPorId(partida.IdTimeVisitante);

            //Descomentar para rodar com o tabela de jogadores.
            partida.TimeMandanteObj.ListaJogadores = appJogador.ListarPorTimeCampeonato(partida.IdCampeonato, partida.IdTimeMandante);
            partida.TimeVisitanteObj.ListaJogadores = appJogador.ListarPorTimeCampeonato(partida.IdCampeonato, partida.IdTimeVisitante);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        public ActionResult Tabela(string id)
        {
            var partida = appPartida.ListaTabelaPorCampeonato(id);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Resultado(Partida partida)
        {
            //if (ModelState.IsValid)
            //{
            appPartida.Resultado(partida);
            //}
            return RedirectToAction("Index");
        }

        public ActionResult ListarUltimaRodada()
        {
            var listaUltimaRodada = appPartida.ListarUltimaRodada();
            return View(listaUltimaRodada);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult ComentarPartidaOnline(string id)
        {
            Partida partida = appPartida.ListarPorId(id);
            return View(partida);
        }

        public ActionResult Comentario(string id)
        {
            var comentario = appPartida.ComentarioPartida(id);
            return View(comentario);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Comentar(string id, string comentario)
        {
            appPartida.ComentarPartida(id, comentario);
            return RedirectToAction("ComentarPartidaOnline", new RouteValueDictionary(
    new { controller = "Partida", action = "ComentarPartidaOnline", id = id }));
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult FotosVideos(string id)
        {
            var partida = appPartida.ListarPorId(id);
            return View(partida);
        }


        //JSON - Todos os métodos que retornam JSON
        //============================================

        public String ObterTabelaJsonPD()
        {
            return ObterTabelaJson("2");
        }

        public String ObterTabelaJsonSD()
        {
            return ObterTabelaJsonSegunda("3");
        }

        public String ObterTabelaJson(string id)
        {
            Rodada rodada = new Rodada();
            var partida = rodada.ConverterPartidasParaRodada(appPartida.ListaTabelaPorCampeonato(id));

            //Rodada rodadaSemfinal = new Rodada();
            //rodadaSemfinal.Jogo1 = "SemiFinal - X - SemiFinal";
            //rodadaSemfinal.HoraJogo1 = "13:15";

            //rodadaSemfinal.Jogo2 = "SemiFinal - X - SemiFinal";
            //rodadaSemfinal.HoraJogo2 = "15:00";

            //rodadaSemfinal.Campo = "A DEFINIR";

            //rodadaSemfinal.Data = "18/09/2016";
            //rodadaSemfinal.Numero = "Semi-Final";
            
            //Rodada rodadaFinal2 = new Rodada();
            //rodadaFinal2.Jogo1 = "Final - X - Final";
            //rodadaFinal2.HoraJogo1 = "13:15";

            //rodadaFinal2.Jogo2 = "Final - X - Final";
            //rodadaFinal2.HoraJogo2 = "15:00";

            //rodadaFinal2.Campo = "A DEFINIR";

            //rodadaFinal2.Data = "25/09/2016";
            //rodadaFinal2.Numero = "1ª Final";

            //Rodada rodadaFinal1 = new Rodada();
            //rodadaFinal1.Jogo1 = "Final - X - Final";
            //rodadaFinal1.HoraJogo1 = "13:15";

            //rodadaFinal1.Jogo2 = "Final - X - Final";
            //rodadaFinal1.HoraJogo2 = "15:00";

            //rodadaFinal1.Campo = "A DEFINIR";
            //rodadaFinal1.Numero = "2ª Final";

            //rodadaFinal1.Data = "09/10/2016";
            //partida.Add(rodadaSemfinal);
            //partida.Add(rodadaFinal2);
            //partida.Add(rodadaFinal1);

            return JsonConvert.SerializeObject(partida, Formatting.Indented);
        }

        public String ObterTabelaJsonSegunda(string id)
        {
            Rodada rodada = new Rodada();
            var partida = rodada.ConverterPartidasParaRodada(appPartida.ListaTabelaPorCampeonatoSegundaDivisao(id));

            Rodada rodadaSemfinal = new Rodada();
            rodadaSemfinal.Jogo1 = "SemiFinal - X - SemiFinal";
            rodadaSemfinal.HoraJogo1 = "09:20";

            rodadaSemfinal.Jogo2 = "SemiFinal - X - SemiFinal";
            rodadaSemfinal.HoraJogo2 = "11:20";

            rodadaSemfinal.Campo = "A DEFINIR";

            rodadaSemfinal.Data = "18/09/2016";
            rodadaSemfinal.Numero = "Semi-Final";

            Rodada rodadaFinal2 = new Rodada();
            rodadaFinal2.Jogo1 = "Final - X - Final";
            rodadaFinal2.HoraJogo1 = "13:15";

            rodadaFinal2.Jogo2 = "Final - X - Final";
            rodadaFinal2.HoraJogo2 = "15:00";

            rodadaFinal2.Campo = "A DEFINIR";

            rodadaFinal2.Data = "25/09/2016";
            rodadaFinal2.Numero = "1ª Final";

            Rodada rodadaFinal1 = new Rodada();
            rodadaFinal1.Jogo1 = "Final - X - Final";
            rodadaFinal1.HoraJogo1 = "13:15";

            rodadaFinal1.Jogo2 = "Final - X - Final";
            rodadaFinal1.HoraJogo2 = "15:00";

            rodadaFinal1.Campo = "A DEFINIR";
            rodadaFinal1.Numero = "2ª Final";

            rodadaFinal1.Data = "09/10/2016";
            partida.Add(rodadaSemfinal);
            partida.Add(rodadaFinal2);
            partida.Add(rodadaFinal1);

            return JsonConvert.SerializeObject(partida, Formatting.Indented);
        }

        public String AoVivo(string id)
        {
            List<AoVivo> comentariosList = (List<AoVivo>)appPartida.PartidaAoVivo(id);
            Partida partida = appPartida.ListarPorId(id);
            AoVivo aoVivo = new AoVivo();
            aoVivo.Data = "Resultado";
            aoVivo.Comentario = partida.GolMandante + "_" + partida.GolVisitante;
            comentariosList.Add(aoVivo);

            return JsonConvert.SerializeObject(comentariosList, Formatting.Indented);
        }

        public String ComentarJson(string id, string comentario)
        {
            try
            {
                appPartida.ComentarPartida(id, comentario);
                return "OK";
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaController: Erro no comentário da partida." + ex.Message, TratamentoLog.NivelLog.Erro);
                return "";
            }
        }

        public String ListarUltimaRodadaJson()
        {
            var listaUltimaRodada = appPartida.ListarUltimaRodada();            
            return JsonConvert.SerializeObject(listaUltimaRodada, Formatting.Indented);
        
        }

        public String ListarProximaRodadaJson()
        {
            Rodada rodada = new Rodada();
            var listaProximaRodada = rodada.ConverterPartidasParaRodada(appPartida.ListarProximaRodadaJson());
            return JsonConvert.SerializeObject(listaProximaRodada, Formatting.Indented);

        }
    }
}

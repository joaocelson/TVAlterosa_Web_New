using Campeonato.Aplicacao;
using Campeonato.Dominio;
using Campeonato.UI.WEB.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campeonato.UI.WEB.Areas.Admin.Controllers
{
    public class JogadorController : Controller
    {
        private readonly JogadorAplicacao appJogador;
        private readonly CampeonatoAplicacao appCampeonato;
        public JogadorController()
        {
            appJogador = JogadorAplicacaoConstrutor.JogadorAplicacaoADO();
            appCampeonato = CampeonatoAplicacaoConstrutor.CampeonatoAplicacaoADO();
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Index()
        {
            var listaJogadores = appJogador.ListarTodos();
            return View(listaJogadores);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult PaginaInicial()
        {
            return View();
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar()
        {
            var listaDeCampeonatos = appCampeonato.ListarTodos();
            // ViewBag.Campeonatos = new SelectList(listaDeCampeonatos, "Id", "Nome");
            ViewData["Campeonatos"] = new SelectList(listaDeCampeonatos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar(Jogador jogador, FormCollection form)
        {
            try
            {
                string idCampeonatoSelecionado = form["Campeonato"];
                jogador.Campeonato = new Campeonatos();
                jogador.Campeonato.Id = Convert.ToInt16(idCampeonatoSelecionado);


                string idTimeSelecionado = form["Time"];
                jogador.Time = new Time();
                jogador.Time.Id = Convert.ToInt16(idTimeSelecionado);

                appJogador.Salvar(jogador);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(jogador);
            }
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Editar(string id)
        {
            var jogador = appJogador.ListarPorId(id);
            var listaDeCampeonatos = appCampeonato.ListarTodos();
            // ViewBag.Campeonatos = new SelectList(listaDeCampeonatos, "Id", "Nome");
            ViewData["Campeonatos"] = new SelectList(listaDeCampeonatos, "Id", "Nome");
           
            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Editar(Jogador jogador)
        {
            appJogador.Salvar(jogador);

            return RedirectToAction("Index");
        }

        public ActionResult Detalhes(string id)
        {
            var jogador = appJogador.ListarPorId(id);

            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Excluir(string id)
        {
            var jogador = appJogador.ListarPorId(id);

            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult ExcluirConfirmado(string id)
        {
            var jogador = appJogador.ListarPorId(id);
            appJogador.Excluir(jogador);
            return RedirectToAction("Index");
        }
    }
}
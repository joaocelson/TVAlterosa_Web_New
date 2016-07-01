using Campeonato.Aplicacao;
using Campeonato.Dominio;
using Campeonato.UI.WEB.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Campeonato.UI.WEB.Areas.Admin
{
    public class BolaoController : Controller
    {
        private readonly PartidaAplicacao appPartida;
        private readonly CampeonatoAplicacao appCampeonato;
        private readonly BolaoAplicacao appBolao;
        private readonly UsuarioAplicacao appUsuario;

        public BolaoController()
        {
            appPartida = PartidaAplicacaoConstrutor.PartidaAplicacaoADO();
            appCampeonato = CampeonatoAplicacaoConstrutor.CampeonatoAplicacaoADO();
            appBolao = BolaoAplicacaoConstrutor.BolaoAplicacaoADO();
            appUsuario = UsuarioAplicacaoConstrutor.UsuarioAplicacaoADO();
        }

        public ActionResult Index()
        {
            Usuario usuario;
            RecuperarUsuarioLogado(out usuario);

            usuario = appUsuario.ValidarUsuarioEmail(usuario);
            var listaBolao = appBolao.ListarBoloes();
            return View(listaBolao);
        }

        private void RecuperarUsuarioLogado(out Usuario usuario)
        {
            usuario = new Usuario();
            usuario.LoginEmail = HttpContext.User.Identity.Name;
        }

        [PermissoesFiltro(Roles = "Visitante, Admin")]
        public ActionResult Participar(string id)
        {
            Usuario usuario = new Usuario();
            RecuperarUsuarioLogado(out usuario);
            usuario = appUsuario.ValidarUsuarioEmail(usuario);
            List<Partida> listaPartidasRegistrado = new List<Partida>();
            IEnumerable<Partida> listaPartidasPorBolao = appPartida.ListarProximaRodadaPorBolao(id);
            foreach (Partida partida in listaPartidasPorBolao)
            {
                Bolao bolao = appBolao.ListarTodosPorUsuarioPartida(usuario.Id, partida.Id);
                if (bolao != null)
                {
                    Partida partidaNova = partida;
                    partidaNova.GolMandante = bolao.GolMandante.ToString();
                    partidaNova.GolVisitante = bolao.GolVisitante.ToString();
                    listaPartidasRegistrado.Add(partidaNova);
                }
                else
                {
                    listaPartidasRegistrado.Add(partida);
                }

            }
            return View(listaPartidasRegistrado);
        }

        [HttpPost]
        [PermissoesFiltro(Roles = "Visitante, Admin")]
        public ActionResult Salvar(Partida partida)
        {
            Usuario usuario;
            RecuperarUsuarioLogado(out usuario);
            usuario = appUsuario.ValidarUsuarioEmail(usuario);

            Bolao bolao = new Bolao();
            bolao.GolMandante = Convert.ToInt16(partida.GolMandante);
            bolao.GolVisitante = Convert.ToInt16(partida.GolVisitante);
            bolao.Usuario = appUsuario.ValidarUsuarioEmail(usuario);
            bolao.Partida = partida;
            partida.Campeonatos = appCampeonato.ListarPorId(partida.IdCampeonato);
            appBolao.Salvar(bolao);
            return RedirectToAction("Participar", new RouteValueDictionary(
            new { controller = "Bolao", action = "Participar", Id = partida.Campeonatos.IdBola }));
        }

        public ActionResult Editar(List<object> listaPartidas)
        {
            return View();
        }

        [HttpPost]
        [PermissoesFiltro(Roles = "Visitante, Admin")]
        public ActionResult Cadastrar(Bolao bolao)
        {
            return View();
        }

        [PermissoesFiltro(Roles = "Visitante, Admin")]
        public ActionResult Palpite(string id)
        {
            var partida = appPartida.ListarPorId(id);
            return View(partida);
        }

        public ActionResult ClassificacaoBolao(string id)
        {
            IEnumerable<Bolao> listaClassificacao = appBolao.ListarClassificacaoPorBolao(id);
            return View(listaClassificacao);
        }

        [PermissoesFiltro(Roles = "Visitante, Admin")]
        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Regras()
        {
            return View();
        }

        public ActionResult VencedorRodada(string id)
        {
            var listaClassificacao = appBolao.VencedorRodada(id);
            return View(listaClassificacao);
        }

        public ActionResult CadastrarUsuario()
        {
            return RedirectToAction("Cadastrar", new RouteValueDictionary(
                new { controller = "Usuario", action = "Cadastrar" }));
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult PontuarBolao()
        {
            return View();
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Pontuar(string data)
        {
            IEnumerable<Partida> listaPartida = appPartida.ListarPartidasPorData(data);

            if (appBolao.PontuarBolao(listaPartida))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("PontuarBolao");
            }
        }

        //METODOS JSON
        //================================================

        //Lista os boloes
        public String IndexJson()
        {
            Usuario usuario;
            RecuperarUsuarioLogado(out usuario);

            usuario = appUsuario.ValidarUsuarioEmail(usuario);
            var listaBolao = appBolao.ListarBoloes();
            return JsonConvert.SerializeObject(listaBolao, Formatting.Indented);
        }

        //Lista as partidas da proxima rodada.
        public String ParticiparJson(string id, string  emailUsuario, string senha)
        {
            Usuario usuario = new Usuario();
            //RecuperarUsuarioLogado(out usuario);
            usuario.LoginEmail = emailUsuario;
            usuario.Senha = senha;
            usuario = appUsuario.ValidarUsuarioEmail(usuario);
            List<Partida> listaPartidasRegistrado = new List<Partida>();
            IEnumerable<Partida> listaPartidasPorBolao = appPartida.ListarProximaRodadaPorBolao(id);
            foreach (Partida partida in listaPartidasPorBolao)
            {
                Bolao bolao = appBolao.ListarTodosPorUsuarioPartida(usuario.Id, partida.Id);
                if (bolao != null)
                {
                    Partida partidaNova = partida;
                    partidaNova.GolMandante = bolao.GolMandante.ToString();
                    partidaNova.GolVisitante = bolao.GolVisitante.ToString();
                    listaPartidasRegistrado.Add(partidaNova);
                }
                else
                {
                    listaPartidasRegistrado.Add(partida);
                }

            }
            return JsonConvert.SerializeObject(listaPartidasRegistrado, Formatting.Indented);
        }

        //Lista a classificação geral do bolão
        public String VencedorRodadaJson(string id)
        {
            List<string[]> listaClassificacao = appBolao.VencedorRodada(id);
            String[] strin = new string[] { "", "", "" };
            listaClassificacao.Add(strin);
            var classificacao = listaClassificacao.Select(p => new
            {
                Posicao = p.ElementAt(0),
                NomeTime = p.ElementAt(1),
                Pontos = p.ElementAt(2)
            });
            return JsonConvert.SerializeObject(classificacao, Formatting.Indented);
        }

        //Habilita a partida para o usuário inserir os palpites da partida - Retorna uma partida
        public String PalpiteJson(string id)
        {
            var partida = appPartida.ListarPorId(id);
            return JsonConvert.SerializeObject(partida, Formatting.Indented);
        }

        //Salva o palpite do usuário e retorna OK caso tenha sido salvo
        public String SalvarJson(Partida partida, string emailUsuario, string senha)
        {
            try
            {
                Usuario usuario = new Usuario();
                //RecuperarUsuarioLogado(out usuario);
                usuario.LoginEmail = emailUsuario;
                usuario.Senha = senha;
                usuario = appUsuario.ValidarUsuarioEmail(usuario);

                Bolao bolao = new Bolao();
                bolao.GolMandante = Convert.ToInt16(partida.GolMandante);
                bolao.GolVisitante = Convert.ToInt16(partida.GolVisitante);
                bolao.Usuario = appUsuario.ValidarUsuarioEmail(usuario);
                bolao.Partida = partida;
                partida.Campeonatos = appCampeonato.ListarPorId(partida.IdCampeonato);
                appBolao.Salvar(bolao);
                return "OK";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.RepositorioADO;

namespace Campeonato.Aplicacao
{
    public class CampeonatoAplicacao
    {
        private readonly IRepositorio<Campeonatos> repositorio;

        public CampeonatoAplicacao(IRepositorio<Campeonatos> repo)
        {
            repositorio = repo;
        }

        public void Salvar(Campeonatos campeonato)
        {
            repositorio.Salvar(campeonato);
        }

        public void Excluir(Campeonatos campeonato)
        {
            repositorio.Excluir(campeonato);
        }

        public IEnumerable<Campeonatos> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Campeonatos ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

               public List<Campeonatos> ConverterListausuario()
        {

            return null;

        }

        public void AdicionarTimes(List<Time> listaTimesAdicionar, string idCampeonato)
        {
            CampeonatoRepositorioADO campADO = new CampeonatoRepositorioADO();
            campADO.AdicionarTimes(listaTimesAdicionar, idCampeonato);
        }

        public IEnumerable<Artilheiro> ArtilhariaPorCampeonato(string id)
        {
            CampeonatoRepositorioADO campADO = new CampeonatoRepositorioADO();
            return campADO.ArtilhariaPorCampeonato(id);
        }

        public IEnumerable<Artilheiro> ArtilhariaPorCampeonatoGeral()
        {
            CampeonatoRepositorioADO campADO = new CampeonatoRepositorioADO();
            return campADO.ArtilhariaPorCampeonatoGeral();
        }

        public List<Noticia> Noticias()
        {
            CampeonatoRepositorioADO campADO = new CampeonatoRepositorioADO();
            return campADO.Noticias();
        }

        public bool GravarToken(string token)
        {
            CampeonatoRepositorioADO campADO = new CampeonatoRepositorioADO();
           return campADO.GravarToken(token);
        }

        public List<String> ObterTokens()
        {
            CampeonatoRepositorioADO campADO = new CampeonatoRepositorioADO();
            return campADO.ObterTokens();
        }
    }
}

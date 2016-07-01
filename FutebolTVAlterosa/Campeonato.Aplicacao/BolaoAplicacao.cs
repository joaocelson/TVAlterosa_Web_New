using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class BolaoAplicacao
    {
        private readonly IRepositorio<Bolao> repositorio;

        public BolaoAplicacao(IRepositorio<Bolao> repo)
        {
            repositorio = repo;
        }

        public IEnumerable<Bolao> ListarTodosPorUsuario(string idUsuario)
        {
            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.ListarTodosPorUsuario(idUsuario);
        }

        public Bolao ListarPorId(string id)
        {
            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.ListarPorId(id);
        }

        public void Salvar(Bolao bolao)
        {
            repositorio.Salvar(bolao);
        }
        
        public object ListarBoloes()
        {

            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.ListarBoloes();
        }

        public Bolao ListarTodosPorUsuarioPartida(int idUsuario, int idPartida)
        {
            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.ListarTodosPorUsuarioPartida(idUsuario, idPartida);
        }

        public IEnumerable<Bolao> ListarClassificacaoPorBolao(string id)
        {
            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.ListarClassificacaoPorBolao(id); 
        }

        public Boolean PontuarBolao(IEnumerable<Partida> listaPartidas)
        {
            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.PontuarBolao(listaPartidas); 

        }

        public List<string[]> VencedorRodada(string id)
        {
            BolaoRepositorioADO repo = new BolaoRepositorioADO();
            return repo.VencedorRodada(id);
        }
    }
}

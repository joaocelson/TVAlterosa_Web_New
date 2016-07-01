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
    public class JogadorAplicacao
    {
        private readonly IRepositorio<Jogador> repositorio;

        public JogadorAplicacao(IRepositorio<Jogador> repo)
        {
            repositorio = repo;
        }

        public Jogador ListarPorId(string id)
        {
            JogadorRepositorioADO repo = new JogadorRepositorioADO();
            return repo.ListarPorId(id);
        }

        public void Salvar(Jogador jogador)
        {
            repositorio.Salvar(jogador);
        }


        public IEnumerable<Jogador> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public List<Jogador> ListarJogadores()
        {
            return (List<Jogador>)repositorio.ListarTodos();
        }

        public void Excluir(Jogador jogador)
        {
            repositorio.Excluir(jogador);
        }

        public List<Jogador> ListarPorTimeCampeonato(string campeonato, string time)
        {
            JogadorRepositorioADO jogadorDAO = new JogadorRepositorioADO();
            return jogadorDAO.ListarPorTimeCampeonato(campeonato, time);
        }
    }
}

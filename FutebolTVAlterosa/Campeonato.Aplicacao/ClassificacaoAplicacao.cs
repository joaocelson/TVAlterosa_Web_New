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
    public class ClassificacaoAplicacao
    {
        private readonly IRepositorio<Classificacao> repositorio;

        public ClassificacaoAplicacao(IRepositorio<Classificacao> repo)
        {
            repositorio = repo;
        }

        public IEnumerable<Classificacao> ListarClassificacao()
        {
            return repositorio.ListarTodos();
        }

        public void Salvar(Classificacao classificacao)
        {
            repositorio.Salvar(classificacao);
        }

        public void Excluir(Classificacao classificacao)
        {
            repositorio.Excluir(classificacao);
        }

        public IEnumerable<Classificacao> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public IEnumerable<Classificacao> ListarClassicacaoPorCampeonato(string idCampeonato)
        {
            ClassificacaoRepositorioADO classificacaoADO = new ClassificacaoRepositorioADO();
            return classificacaoADO.ListarClassicacaoPorCampeonato(idCampeonato);
        }

        public Classificacao ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

        public List<Classificacao> ConverterListaclassificacao()
        {

            return null;

        }


        public IEnumerable<Campeoes> ListarCampeoesPorCampeonato(string id)
        {
            ClassificacaoRepositorioADO classificacaoADO = new ClassificacaoRepositorioADO();
            return classificacaoADO.ListarCampeoesPorCampeonato(id);
        }

        
    }
}


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
    public class TimeAplicacao
    {
        private readonly IRepositorio<Time> repositorio;

        public TimeAplicacao(IRepositorio<Time> repo)
        {
            repositorio = repo;
        }

        public void Salvar(Time time)
        {
            repositorio.Salvar(time);
        }

        public void Excluir(Time time)
        {
            repositorio.Excluir(time);
        }

        public IEnumerable<Time> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Time ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

        public IEnumerable<Time> ListarTimesCampeonato(string idCampeonato)
        {
            TimeRepositorioADO timeRepositorioADO = new TimeRepositorioADO();
            return timeRepositorioADO.ListarTimesCampeonato(idCampeonato);
        }
    }
}

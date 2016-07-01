using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.RepositorioEF
{
    public class TimeRepositorioEF : IRepositorio<Time>
    {
        private readonly Contexto contexto;

        public TimeRepositorioEF()
        {
            contexto = new Contexto();
        }

        public void Salvar(Time entidade)
        {
            if (entidade.Id > 0)
            {
                var TimeAlterar = contexto.Times.First(x => x.Id == entidade.Id);
                TimeAlterar.Nome = entidade.Nome;
                TimeAlterar.DataFundacao = entidade.DataFundacao;
            }
            else
            {
                contexto.Times.Add(entidade);
            }
            contexto.SaveChanges();
        }

        public void Excluir(Time entidade)
        {
            var TimeExcluir = contexto.Times.First(x => x.Id == entidade.Id);
            contexto.Set<Time>().Remove(TimeExcluir);
            contexto.SaveChanges();
        }

        public IEnumerable<Time> ListarTodos()
        {
            return contexto.Times;
        }

        public Time ListarPorId(string id)
        {
            int idInt;
            Int32.TryParse(id, out idInt);
            return contexto.Times.First(x => x.Id == idInt);
        }
    }
}

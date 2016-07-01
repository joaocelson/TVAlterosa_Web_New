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
    public class NoticiaAplicacao 
    {
        private readonly IRepositorio<Noticia> repositorio;

        public NoticiaAplicacao(IRepositorio<Noticia> repo)
        {
            repositorio = repo;
        }

        public void Salvar(Noticia noticia)
        {
            repositorio.Salvar(noticia);
        }

        public void Excluir(Noticia noticia)
        {
            repositorio.Excluir(noticia);
        }

        public IEnumerable<Noticia> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Noticia ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

    }
}

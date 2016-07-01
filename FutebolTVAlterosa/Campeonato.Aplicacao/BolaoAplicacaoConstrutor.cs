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
    public class BolaoAplicacaoConstrutor
    {
        private readonly IRepositorio<Bolao> repositorio;

        public static BolaoAplicacao BolaoAplicacaoADO()
        {
            return new BolaoAplicacao(new BolaoRepositorioADO());
        }

        public void Salvar(Bolao bolao)
        {
            repositorio.Salvar(bolao);
        }

        public void Excluir(Bolao bolao)
        {
            repositorio.Excluir(bolao);
        }

        public IEnumerable<Bolao> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Bolao ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }
    }
}

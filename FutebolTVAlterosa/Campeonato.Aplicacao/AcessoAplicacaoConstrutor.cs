using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class AcessoAplicacaoConstrutor
    {
        public static AcessoAplicacao AcessoAplicacaoADO()
        {
            return new AcessoAplicacao(new AcessoRepositorioADO());
        }
    }
}

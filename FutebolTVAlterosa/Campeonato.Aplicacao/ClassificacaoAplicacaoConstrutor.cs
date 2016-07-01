using Campeonato.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campeonato.RepositorioADO;

namespace Campeonato.Aplicacao
{
    public class ClassificacaoAplicacaoConstrutor
    {
        public static ClassificacaoAplicacao ClassificacaoAplicacaoADO()
        {
            return new ClassificacaoAplicacao(new ClassificacaoRepositorioADO());
        }
    }
}

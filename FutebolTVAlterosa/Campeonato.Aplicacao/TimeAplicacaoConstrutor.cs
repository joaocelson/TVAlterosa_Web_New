
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class TimeAplicacaoConstrutor
    {
        public static TimeAplicacao TimeAplicacaoADO()
        {
            return new TimeAplicacao(new TimeRepositorioADO());
        }

    }
}

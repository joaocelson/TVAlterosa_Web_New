
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class PartidaAplicacaoConstrutor
    {
        public static PartidaAplicacao PartidaAplicacaoADO()
        {
            return new PartidaAplicacao(new PartidaRepositorioADO());
        }

        //public static TimeAplicacao TimeAplicacaoEF()
        //{
        //    return new TimeAplicacao(new PartidaRepositorioEF());
        //}
    }
}

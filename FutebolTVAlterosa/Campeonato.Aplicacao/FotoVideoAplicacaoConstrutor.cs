using Campeonato.Dominio;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class FotoVideoAplicacaoConstrutor
    {
        public static FotoVideoAplicacao FotoVideoADO()
        {
            return new FotoVideoAplicacao(new FotoVideoRepositorioADO());
        }
    }
}

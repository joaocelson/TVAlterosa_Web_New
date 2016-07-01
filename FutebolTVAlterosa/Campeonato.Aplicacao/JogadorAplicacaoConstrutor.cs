using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class JogadorAplicacaoConstrutor
    {
        public static JogadorAplicacao JogadorAplicacaoADO()
        {
            return new JogadorAplicacao(new JogadorRepositorioADO());
        }
    }
}

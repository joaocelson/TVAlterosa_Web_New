using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class NoticiaAplicacaoConstrutor
    {
        public static NoticiaAplicacao NoticiaAplicacaoADO()
        {
            return new NoticiaAplicacao(new NoticiaRepositorioADO());
        }
    }
}

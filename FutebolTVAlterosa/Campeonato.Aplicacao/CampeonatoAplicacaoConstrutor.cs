
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campeonato.RepositorioADO;

namespace Campeonato.Aplicacao
{
   public class CampeonatoAplicacaoConstrutor
   {
        public static CampeonatoAplicacao CampeonatoAplicacaoADO()
        {
            return new CampeonatoAplicacao(new CampeonatoRepositorioADO());
        }

       //public static usuarioAplicacao usuarioAplicacaoEF()
       //{
       //    return new usuarioAplicacao(new usuarioRepositorioEF());
       //}
    }
}

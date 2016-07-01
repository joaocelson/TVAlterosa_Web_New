using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campeonato.Aplicacao;
using Campeonato.RepositorioADO;

namespace Campeonato.Aplicacao
{
   public class UsuarioAplicacaoConstrutor
   {
        public static UsuarioAplicacao UsuarioAplicacaoADO()
        {
            return new UsuarioAplicacao(new UsuarioRepositorioADO());
        }

       //public static usuarioAplicacao usuarioAplicacaoEF()
       //{
       //    return new usuarioAplicacao(new usuarioRepositorioEF());
       //}
    }
}

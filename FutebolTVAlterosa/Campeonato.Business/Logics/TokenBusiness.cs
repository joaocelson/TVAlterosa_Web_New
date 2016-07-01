using Campeonato.Dominio;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Business.Logics
{
    public class TokenBusiness
    {

        public List<Token_Web> ObterToken()
        {
            return TokenDAO.ObterToken();
        }

        //public void InserirToken(Token item)
        //{
        //    TokenDAO.InserirToken(item);
        //}  

    }
}

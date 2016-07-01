using Campeonato.Dominio;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Business.Logics
{
    public class TimeBusiness
    {

        public List<Time_Web> ObterTime()
        {
            return TimeDAO.ObterTime();
        }

        public void InserirTime(Time_Web item)
        {
            TimeDAO.InserirTime(item);
        }

        public void RemoveTime(Time_Web item)
        {
            TimeDAO.RemoveTime(item);
        }

        public void AtualizaTime(Time_Web item)
        {
            TimeDAO.AtualizaTime(item);
        }

    }
}

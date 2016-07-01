using Campeonato.Dominio;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Business.Logics
{
    public class CampeonatoBusiness
    {

        public List<Campeonato_Web> ObterCampeonato()
        {
            return CampeonatoDAO.ObterCampeonato();
        }

        public void InserirCampeonato(Campeonato_Web item)
        {
            CampeonatoDAO.InserirCampeonato(item);
        }

        public void RemoveCampeonato(Campeonato_Web item)
        {
            CampeonatoDAO.RemoveCampeonato(item);
        }

        public void AtualizaCampeonato(Campeonato_Web item)
        {
            CampeonatoDAO.AtualizaCampeonato(item);
        }

    }
}

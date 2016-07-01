using Campeonato.Dominio;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Business.Logics
{
    public class ArtilhariaBusiness
    {

        public List<Artilharia_Web> ObterArtilharia()
        {
            return ArtilhariaDAO.ObterArtilharia();
        }

        public void InserirArtilharia(Artilharia_Web item)
        {
            ArtilhariaDAO.InserirArtilharia(item);
        }

        public void RemoveArtilharia(Artilharia_Web item)
        {
            ArtilhariaDAO.RemoveArtilharia(item);
        }

        public void AtualizaArtilharia(Artilharia_Web item)
        {
            ArtilhariaDAO.AtualizaArtilharia(item);
        }

    }
}

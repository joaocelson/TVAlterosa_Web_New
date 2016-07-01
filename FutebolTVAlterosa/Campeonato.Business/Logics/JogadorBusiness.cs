using Campeonato.Dominio;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Business.Logics
{
    public class JogadorBusiness
    {

        public List<Jogador_Web> ObterJogador()
        {
            return JogadorDAO.ObterJogador();
        }

        public void InserirJogador(Jogador_Web item)
        {
            JogadorDAO.InserirJogador(item);
        }

        public void RemoveJogador(Jogador_Web item)
        {
            JogadorDAO.RemoveJogador(item);
        }

        public void RemoveArtilharia(Jogador_Web item)
        {
            JogadorDAO.RemoveArtilharia(item);
        }

        public void AtualizaJogador(Jogador_Web item)
        {
            JogadorDAO.AtualizaJogador(item);
        }

    }
}

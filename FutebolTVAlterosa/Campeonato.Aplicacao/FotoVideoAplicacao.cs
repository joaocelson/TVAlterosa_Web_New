using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class FotoVideoAplicacao
    {

        private readonly IRepositorio<FotosVideos> repositorio;

        public FotoVideoAplicacao(IRepositorio<FotosVideos> repo)
        {
            repositorio = repo;
        }


        public IEnumerable<FotosVideos> ListarTime(string id)
        {
            FotoVideoRepositorioADO fotoVideoADO = new FotoVideoRepositorioADO();
            return fotoVideoADO.ListarTime(id);
        }



        public IEnumerable<FotosVideos> ListarFotoVideoTimePartidaCampeonato(Partida partida)
        {

            FotoVideoRepositorioADO fotoVideoADO = new FotoVideoRepositorioADO();
            return fotoVideoADO.ListarFotoVideoTimePartidaCampeonato(partida);
        }

        public IEnumerable<string> ListarDataFotoVideo()
        {

            FotoVideoRepositorioADO fotoVideoADO = new FotoVideoRepositorioADO();
            return fotoVideoADO.ListarDataFotoVideo();
        }

        public IEnumerable<FotosVideos> ListarFotoVideoPorData(string data)
        {
            FotoVideoRepositorioADO fotoVideoADO = new FotoVideoRepositorioADO();
            return fotoVideoADO.ListarFotoVideoPorData(data);
        }

        public IEnumerable<FotosVideos> ListarFotosVideosParaEditar()
        {
            FotoVideoRepositorioADO fotoVideoADO = new FotoVideoRepositorioADO();
            return fotoVideoADO.ListarFotosVideosParaEditar();
        }

        public Boolean Inserir(FotosVideos fotosVideos)
        {
            FotoVideoRepositorioADO fotoVideoADO = new FotoVideoRepositorioADO();
            return fotoVideoADO.Inserir(fotosVideos);

        }
    }

}

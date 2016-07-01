using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.Dominio.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Campeonato.RepositorioADO
{
    public class FotoVideoRepositorioADO : IRepositorio<FotosVideos>
    {

        private Contexto contexto;

        public void Salvar(FotosVideos entidade)
        {
            throw new NotImplementedException();
        }

        public void Excluir(FotosVideos entidade)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FotosVideos> ListarTodos()
        {
            throw new NotImplementedException();
        }

        public FotosVideos ListarPorId(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FotosVideos> ListarTime(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * " +
                           "FROM fotos_videos WHERE id_time = {0}", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }

        }

        public IEnumerable<FotosVideos> ListarFotoVideoTimePartidaCampeonato(Partida partida)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * " +
                            "FROM fotos_videos WHERE (id_time = {0}", partida.IdTimeMandante);
                strQuery += string.Format(" OR id_time = {0} ", partida.IdTimeVisitante);
                strQuery += string.Format(" ) AND id_campeonato = {0} ", partida.IdCampeonato);
                strQuery += string.Format(" AND id_partida = {0} order by id_partida", partida.Id);

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }

        }

        private IEnumerable<FotosVideos> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var fotosVideos = new List<FotosVideos>();
            while (reader.Read())
            {
                var temObjeto = new FotosVideos()
                {
                    Id = reader["id"].ToString(),
                    IdCampeonato = reader["id_campeonato"].ToString(),
                    IdTime = reader["id_time"].ToString(),
                    Rodada = reader["rodada"].ToString(),
                    IdPartida = reader["id_partida"].ToString(),
                    Caminho = reader["caminho"].ToString(),
                    Descricao = reader["descricao"].ToString(),
                    Video = reader["video"].ToString()

                };
                fotosVideos.Add(temObjeto);
            }
            reader.Close();
            return fotosVideos;
        }


        public IEnumerable<string> ListarDataFotoVideo()
        {
            using (contexto = new Contexto())
            {
                //var strQuery = string.Format("select CONVERT (char(10),p.data_partida, 103) data from fotos_videos f inner join partida p on p.id= f.id_partida group by CONVERT (char(10),p.data_partida, 103) ORDER BY data desc");

//#if DEBUG
//                 var strQuery = string.Format("select CAST(b.data AS DATETIME) dtConvertida from (	select distinct CONVERT(char(10),data, 105) data from (	select distinct p.data_partida data from fotos_videos f inner join partida p on p.id= f.id_partida)a )b order by dtConvertida  desc");
//#else
                var strQuery = string.Format("select CAST(b.data AS DATETIME) dtConvertida from (	select distinct CONVERT(char(10),data, 101) data from (	select distinct p.data_partida data from fotos_videos f inner join partida p on p.id= f.id_partida)a )b order by dtConvertida  desc");
//#endif

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);

                List<string> listaDatasPartidas = new List<string>();

                while (retornoDataReader.Read())
                {
                    CultureInfo culture = CultureInfo.GetCultureInfo("pt-BR");

                    Thread.CurrentThread.CurrentUICulture = culture;
                    Thread.CurrentThread.CurrentCulture = culture;

                    DateTime data = DateTime.Parse(retornoDataReader["dtConvertida"].ToString());

                    listaDatasPartidas.Add(data.ToString().Substring(0, 10));
                }

                return listaDatasPartidas;
            }
        }

        public IEnumerable<FotosVideos> ListarFotoVideoPorData(string data)
        {
            using (contexto = new Contexto())
            {
#if DEBUG
                var strQuery = string.Format("select * from fotos_videos f inner join partida p on p.id = f.id_partida where  CONVERT (char(10),p.data_partida, 103) = '{0}' order by f.id_partida", data);

#else
                var strQuery = string.Format("select * from fotos_videos f inner join partida p on p.id = f.id_partida where  CONVERT (char(10),p.data_partida, 103) = '{0}' order by f.id_partida", data);
                
#endif

               //var strQuery = string.Format("select * from fotos_videos f inner join partida p on p.id = f.id_partida where  CONVERT (char(10),p.data_partida, 103) = '{0}' order by f.id_partida", data);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public IEnumerable<FotosVideos> ListarFotosVideosParaEditar()
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("select * from fotos_videos");
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public bool Inserir(FotosVideos fotosVideos)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("INSERT INTO fotos_videos (id_time, id_partida, id_campeonato, caminho, descricao, video) ");
                    strQuery += string.Format("VALUES ({0},{1},{2},'{3}','{4}',{5})", fotosVideos.IdTime, fotosVideos.IdPartida, fotosVideos.IdCampeonato,
                                                  fotosVideos.Caminho, fotosVideos.Descricao, fotosVideos.Video);
                    contexto.ExecutaComando(strQuery);
                    return true;
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("FotoVideoRepositorioADO::InserirFotoVideo:. Erro : " + ex.Message, TratamentoLog.NivelLog.Erro);

                return false;
            }
        }
    }
}

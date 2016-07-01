using Campeonato.Dominio.contrato;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Campeonato.Dominio.util;
using System.Data;

namespace Campeonato.RepositorioADO
{
    public class CampeonatoRepositorioADO : IRepositorio<Campeonatos>
    {
        private Contexto contexto;

        private void Inserir(Campeonatos Campeonato)
        {
            var strQuery = "";
            strQuery += " INSERT INTO Campeonato (nome, data_inicio) ";
            strQuery += string.Format(" VALUES ('{0}','{1}') ",
                Campeonato.Nome, Campeonato.DataInicio
                );
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        private void Alterar(Campeonatos Campeonato)
        {
            var strQuery = "";
            strQuery += " UPDATE Campeonato SET ";
            strQuery += string.Format(" Nome = '{0}', ", Campeonato.Nome);
            strQuery += string.Format(" data_inicio = '{0}', ", Campeonato.DataInicio);
            strQuery += string.Format(" WHERE Id = {0} ", Campeonato.Id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void Salvar(Campeonatos Campeonato)
        {
            if (Campeonato.Id > 0)
                Alterar(Campeonato);
            else
                Inserir(Campeonato);
        }

        public void Excluir(Campeonatos Campeonato)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM Campeonato WHERE Id = {0}", Campeonato.Id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public IEnumerable<Campeonatos> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM Campeonato ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Campeonatos ListarPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM Campeonato WHERE Id = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Campeonatos> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var Campeonato = new List<Campeonatos>();
            while (reader.Read())
            {
                int idBolao = 0;
                //if (reader["id_bolao"] != null && !reader["id_bolao"].ToString().Equals(""))
                //{
                //    idBolao = Convert.ToInt16(reader["id_bolao"].ToString());
                //}


                var temObjeto = new Campeonatos()
                {
                    Id = Convert.ToInt32(reader["id"].ToString()),
                    Nome = reader["Nome"].ToString(),
                    //IdBola = idBolao,
                    DataInicio = DateTime.Parse(reader["data_inicio"].ToString())
                };
                Campeonato.Add(temObjeto);
            }
            reader.Close();
            return Campeonato;
        }

        public void AdicionarTimes(List<Time> times, string idCampeonato)
        {
            SqlTransaction transacao = null;
            try
            {
                using (contexto = new Contexto())
                {
                    transacao = contexto.MinhaConexao().BeginTransaction(IsolationLevel.ReadCommitted);

                    foreach (Time time in times)
                    {
                        var strQuery = "";
                        strQuery += " INSERT INTO time_campeonato (id_time, id_campeonato) ";
                        strQuery += string.Format(" VALUES ('{0}','{1}') ",
                            time.Id, idCampeonato);
                        contexto.ExecutaComandoTransacao(strQuery, transacao);
                        TratamentoLog.GravarLog("Times: " + time.Nome + " associado ao campeonato: " + idCampeonato);
                    }
                    transacao.Commit();
                }
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                TratamentoLog.GravarLog("PartidaRepositorioADO::GerarPartidaAutomaticamenteTime:. Erro ao geras Partidas" + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        public IEnumerable<Artilheiro> ArtilhariaPorCampeonato(String idCampeonato)
        {
            using (contexto = new Contexto())
            {
                var strQuery = @" SELECT t.Nome Nome, ar.numero_gols Numero_Gols, tm.Nome Time  FROM artilharia ar
                                    INNER JOIN t_jogador t on (t.id = ar.id_jogador)
                                    INNER JOIN times tm on (tm.id = ar.id_time)
                                    WHERE t.DATA_INATIVACAO IS NULL 
                                          and ar.id_campeonato = " + idCampeonato;
                if (idCampeonato.Equals("3"))
                {
                    strQuery += @" OR ar.id_campeonato = 4";
                }
                strQuery += @" ORDER BY ar.numero_gols desc";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjetoArtilharia(retornoDataReader);
            }

        }

        //ESSA ARTILHARIA SERA PARA TODOS OS CAMPEONATOS
        public IEnumerable<Artilheiro> ArtilhariaPorCampeonatoGeral()
        {
            using (contexto = new Contexto())
            {
                var strQuery = @" SELECT t.Nome Nome, ar.numero_gols Numero_Gols, tm.Nome Time  FROM artilharia ar
                                    INNER JOIN t_jogador t on (t.id = ar.id_jogador)
                                    INNER JOIN times tm on (tm.id = ar.id_time)
                                        WHERE t.DATA_INATIVACAO IS NULL ";
                strQuery += @" ORDER BY ar.numero_gols desc";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjetoArtilharia(retornoDataReader);
            }

        }


        public List<Noticia> Noticias()
        {
            using (contexto = new Contexto())
            {
                var strQuery = @" SELECT n.*, t.* FROM noticia n
                                    INNER JOIN times tm on (tm.id_time = n.id_time)";

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjetoNoticia(retornoDataReader);
            }

        }

        private List<Noticia> TransformaReaderEmListaDeObjetoNoticia(SqlDataReader reader)
        {
            List<Noticia> noticias = new List<Noticia>();
            while (reader.Read())
            {
                Time time = new Time();
                Noticia noticia = new Noticia();
                noticia.Titulo = reader["titulo"].ToString();
                noticia.Corpo = reader["noticia"].ToString();
                noticia.DataNoticia = DateTime.Parse(reader["data_noticia"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                time.EscudoPequeno = reader["escudo"].ToString();
                time.Nome = reader["Numero_Gols"].ToString();
                noticias.Add(noticia);
            }
            reader.Close();
            return noticias;

        }

        private IEnumerable<Artilheiro> TransformaReaderEmListaDeObjetoArtilharia(SqlDataReader reader)
        {
            var artilharia = new List<Artilheiro>();
            while (reader.Read())
            {
                var temObjeto = new Artilheiro()
                {
                    Nome = reader["Nome"].ToString(),
                    Time = reader["Time"].ToString(),
                    NumeroGols = reader["Numero_Gols"].ToString(),
                };
                artilharia.Add(temObjeto);
            }
            reader.Close();
            return artilharia;
        }

        public bool GravarToken(String token)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = "";
                    strQuery += " INSERT INTO token (token) ";
                    strQuery += string.Format(" VALUES ('{0}') ", token);
                    contexto.ExecutaComando(strQuery);
                    TratamentoLog.GravarLog("Token: " + token + " associado ao usuarioID: ");
                    return true;
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::GravarToken:. Erro ao geras Partidas" + ex.Message, TratamentoLog.NivelLog.Erro);
                return false;
            }
        }

        public List<String> ObterTokens()
        {
            try
            {
                List<String> tokens = new List<String>();
                using (contexto = new Contexto())
                {
                    var strQuery = @" SELECT DISTINCT * FROM token";

                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (retornoDataReader.Read())
                    {
                        tokens.Add(retornoDataReader["token"].ToString());
                    }
                }
                return tokens;
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ObterTokens:. Erro ao geras Partidas" + ex.Message, TratamentoLog.NivelLog.Erro);
                return new List<string>();
            }
        }
    }
}

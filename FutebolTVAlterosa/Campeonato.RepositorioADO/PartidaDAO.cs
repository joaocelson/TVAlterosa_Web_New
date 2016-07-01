using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Campeonato.Dominio.util;

namespace Campeonato.RepositorioADO
{
    public class PartidaDAO
    {

        public static List<Partida_Web> ObterPartida(string campeonato, string dataInicio, string dataFim)
        {
            List<Partida_Web> listaPartidas = new List<Partida_Web>();

            dataFim = dataFim.Split(' ')[0];

            try
            {
                String SQL = @"SELECT ID, ID_TIME_MANDANTE, ID_TIME_VISITANTE, ID_CAMPEONATO, DATA_PARTIDA, LOCAL_PARTIDA,
	                                  RODADA, REMARCADA_PARTIDA, GOL_TIME_MANDANTE, GOL_TIME_VISITANTE, PONTOS_COMPUTADO,
	                                  NOVA_DATA_PARTIDA, ESTADIO 
	                                FROM PARTIDA
                                   WHERE DATA_INATIVACAO IS NULL
                                   AND DATA_PARTIDA >= CONVERT(datetime, '" + dataInicio + @"', 103) 
                                   AND DATA_PARTIDA <= CONVERT(datetime, '" + dataFim + " 23:59:59', 103) ";
                                 if (!string.IsNullOrEmpty(campeonato))
                                      SQL += " AND ID_CAMPEONATO = '" + campeonato + "' ";
                                  SQL += " ORDER BY DATA_PARTIDA DESC";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Partida_Web partida = new Partida_Web();
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) partida.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME_MANDANTE"))) partida.IdTimeMandante = reader.GetInt32(reader.GetOrdinal("ID_TIME_MANDANTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME_VISITANTE"))) partida.IdTimeVisitante = reader.GetInt32(reader.GetOrdinal("ID_TIME_VISITANTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_CAMPEONATO"))) partida.Id_Campeonato = reader.GetInt32(reader.GetOrdinal("ID_CAMPEONATO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("DATA_PARTIDA"))) partida.DataPartida = reader.GetDateTime(reader.GetOrdinal("DATA_PARTIDA"));
                            if (!reader.IsDBNull(reader.GetOrdinal("LOCAL_PARTIDA"))) partida.LocalPartida = reader.GetString(reader.GetOrdinal("LOCAL_PARTIDA"));
                            if (!reader.IsDBNull(reader.GetOrdinal("RODADA"))) partida.Rodada = reader.GetString(reader.GetOrdinal("RODADA"));
                            
                            partida.RemarcadaString = reader.GetBoolean(reader.GetOrdinal("REMARCADA_PARTIDA")) == true ? "SIM" : "NÃO";

                            if (!reader.IsDBNull(reader.GetOrdinal("GOL_TIME_MANDANTE"))) partida.GolMandante = Convert.ToString(reader.GetInt32(reader.GetOrdinal("GOL_TIME_MANDANTE")));
                            if (!reader.IsDBNull(reader.GetOrdinal("GOL_TIME_VISITANTE"))) partida.GolVisitante = Convert.ToString(reader.GetInt32(reader.GetOrdinal("GOL_TIME_VISITANTE")));
                            if (!reader.IsDBNull(reader.GetOrdinal("PONTOS_COMPUTADO")))
                                partida.PontosComputadosString = reader.GetInt32(reader.GetOrdinal("PONTOS_COMPUTADO")) == 0 ? "SIM" : "NÃO";
                            if (!reader.IsDBNull(reader.GetOrdinal("NOVA_DATA_PARTIDA")))
                                partida.DataPartidaRemarcadaString = Convert.ToString(reader.GetDateTime(reader.GetOrdinal("NOVA_DATA_PARTIDA")));
                            else
                                partida.DataPartidaRemarcadaString = "";
                            if (!reader.IsDBNull(reader.GetOrdinal("ESTADIO"))) partida.Estadio = reader.GetString(reader.GetOrdinal("ESTADIO"));

                            listaPartidas.Add(partida);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaPartidas;
        }

        public static List<ResultadoPartida_Web> ObterPartidaResultado(string campeonato, string dataInicio, string dataFim)
        {
            List<ResultadoPartida_Web> listaPartidas = new List<ResultadoPartida_Web>();

            dataFim = dataFim.Split(' ')[0];

            try
            {
                String SQL = @"SELECT ID, ID_TIME_MANDANTE, ID_TIME_VISITANTE, ID_CAMPEONATO, DATA_PARTIDA, LOCAL_PARTIDA,
	                                  RODADA, REMARCADA_PARTIDA, GOL_TIME_MANDANTE, GOL_TIME_VISITANTE, PONTOS_COMPUTADO,
	                                  NOVA_DATA_PARTIDA, ESTADIO 
	                                FROM PARTIDA
                                   WHERE DATA_INATIVACAO IS NULL
                                   AND DATA_PARTIDA >= CONVERT(datetime, '" + dataInicio + @"', 103) 
                                   AND DATA_PARTIDA <= CONVERT(datetime, '" + dataFim + " 23:59:59', 103) ";
                if (!string.IsNullOrEmpty(campeonato))
                    SQL += " AND ID_CAMPEONATO = '" + campeonato + "' ";
                SQL += " ORDER BY DATA_PARTIDA DESC";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ResultadoPartida_Web partida = new ResultadoPartida_Web();
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) partida.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME_MANDANTE"))) partida.IdTimeMandante = reader.GetInt32(reader.GetOrdinal("ID_TIME_MANDANTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME_VISITANTE"))) partida.IdTimeVisitante = reader.GetInt32(reader.GetOrdinal("ID_TIME_VISITANTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_CAMPEONATO"))) partida.Id_Campeonato = reader.GetInt32(reader.GetOrdinal("ID_CAMPEONATO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("DATA_PARTIDA"))) partida.DataPartida = reader.GetDateTime(reader.GetOrdinal("DATA_PARTIDA"));

                            if (!reader.IsDBNull(reader.GetOrdinal("GOL_TIME_MANDANTE"))) partida.GolMandante = Convert.ToString(reader.GetInt32(reader.GetOrdinal("GOL_TIME_MANDANTE")));
                            if (!reader.IsDBNull(reader.GetOrdinal("GOL_TIME_VISITANTE"))) partida.GolVisitante = Convert.ToString(reader.GetInt32(reader.GetOrdinal("GOL_TIME_VISITANTE")));                          

                            listaPartidas.Add(partida);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaPartidas;
        }

        public static void InserirPartida(Partida_Web item)
        {
            string dataPartida = null;
            if(item.DataPartida != null){
                dataPartida = item.DataPartida.ToString("yyyy-MM-dd HH:mm:ss"); 
            }
            int remarcada = 0;
            if (item.Remarcada == true)
            {
                remarcada = 1;
            }
            int pontosComputados = 0;
            if (item.PontosComputados == true)
            {
                pontosComputados = 1;
            }
            string dataNovaPartida = "null";
            if (item.DataPartidaRemarcada != null && remarcada == 1)
            {
                DateTime data = Convert.ToDateTime(item.DataPartidaRemarcada);
                dataNovaPartida = data.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string golMandante = "null";
            if (item.GolMandante != null)
            {
                golMandante = item.GolMandante;
            }
            string golVisitante = "null";
            if (item.GolVisitante != null)
            {
                golVisitante = item.GolVisitante;
            }
            try
            {

                String SQL = @"INSERT INTO PARTIDA (ID_TIME_MANDANTE, ID_TIME_VISITANTE, ID_CAMPEONATO, DATA_PARTIDA, LOCAL_PARTIDA,
	                                                RODADA, REMARCADA_PARTIDA, GOL_TIME_MANDANTE, GOL_TIME_VISITANTE, PONTOS_COMPUTADO,
	                                                NOVA_DATA_PARTIDA, ESTADIO) 
	                                        VALUES (" + item.IdTimeMandante + ", " + item.IdTimeVisitante + ", " + item.Id_Campeonato + @", 
                                                    '" + dataPartida + "', '" + item.LocalPartida + "', '" + item.Rodada + @"', 
                                                    " + remarcada + ", " + golMandante + ", " + golVisitante + ", " + pontosComputados; 
                                                    
                if(dataNovaPartida == "null")
                    SQL += ", " + dataNovaPartida + ", '" + item.Estadio + "')";
                else
                    SQL += ", '" + dataNovaPartida + "', '" + item.Estadio + "')";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RemovePartida(Partida_Web item)
        {           
            try
            {
                String SQL = @"UPDATE PARTIDA
                                    SET DATA_INATIVACAO = GETDATE()
                                  WHERE ID = " + item.Id;

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AtualizaPartida(Partida_Web item)
        {
            string dataPartida = null;
            if (item.DataPartida != null)
            {
                dataPartida = item.DataPartida.ToString("yyyy-MM-dd HH:mm:ss");
            }
            int remarcada = 0;
            if (item.Remarcada == true)
            {
                remarcada = 1;
            }
            int pontosComputados = 0;
            if (item.PontosComputados == true)
            {
                pontosComputados = 1;
            }
            string dataNovaPartida = "null";
            if (item.DataPartidaRemarcada != null && remarcada == 1)
            {
                DateTime data = Convert.ToDateTime(item.DataPartidaRemarcada);
                dataNovaPartida = data.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string golMandante = "null";
            if (item.GolMandante != null)
            {
                golMandante = item.GolMandante;
            }
            string golVisitante = "null";
            if (item.GolVisitante != null)
            {
                golVisitante = item.GolVisitante;
            }
            try
            {
                String SQL = @"UPDATE PARTIDA
                                    SET ID_TIME_MANDANTE = " + item.IdTimeMandante + ", ID_TIME_VISITANTE = " + item.IdTimeVisitante + ", ID_CAMPEONATO = " + item.Id_Campeonato + @", 
                                        DATA_PARTIDA = '" + dataPartida + "', LOCAL_PARTIDA = '" + item.LocalPartida + "', RODADA = '" + item.Rodada + @"',
                                        REMARCADA_PARTIDA = " + remarcada + ", GOL_TIME_MANDANTE = " + golMandante + ", GOL_TIME_VISITANTE = " + golVisitante + @",
                                        PONTOS_COMPUTADO = " + pontosComputados;

                if (dataNovaPartida == "null")
                    SQL += ", NOVA_DATA_PARTIDA = " + dataNovaPartida + ", ESTADIO = '" + item.Estadio + "'";
                else
                    SQL += ", NOVA_DATA_PARTIDA = '" + dataNovaPartida + "', ESTADIO = '" + item.Estadio + "'";

                SQL += " WHERE ID = " + item.Id;

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Partida_Web ObterPartidaId(int id)
        {
            Partida_Web partida = new Partida_Web();

            try
            {
                String SQL = @"SELECT ID, ID_TIME_MANDANTE, ID_TIME_VISITANTE, ID_CAMPEONATO, DATA_PARTIDA, LOCAL_PARTIDA,
	                                  RODADA, REMARCADA_PARTIDA, GOL_TIME_MANDANTE, GOL_TIME_VISITANTE, PONTOS_COMPUTADO,
	                                  NOVA_DATA_PARTIDA, ESTADIO 
	                                FROM PARTIDA
                                   WHERE DATA_INATIVACAO IS NULL ";                                   
                if (id != 0 && id != null)
                    SQL += " AND ID = " + id;

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) partida.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME_MANDANTE"))) partida.IdTimeMandante = reader.GetInt32(reader.GetOrdinal("ID_TIME_MANDANTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME_VISITANTE"))) partida.IdTimeVisitante = reader.GetInt32(reader.GetOrdinal("ID_TIME_VISITANTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_CAMPEONATO"))) partida.Id_Campeonato = reader.GetInt32(reader.GetOrdinal("ID_CAMPEONATO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("DATA_PARTIDA"))) partida.DataPartida = reader.GetDateTime(reader.GetOrdinal("DATA_PARTIDA"));
                            if (!reader.IsDBNull(reader.GetOrdinal("LOCAL_PARTIDA"))) partida.LocalPartida = reader.GetString(reader.GetOrdinal("LOCAL_PARTIDA"));
                            if (!reader.IsDBNull(reader.GetOrdinal("RODADA"))) partida.Rodada = reader.GetString(reader.GetOrdinal("RODADA"));

                            partida.RemarcadaString = reader.GetBoolean(reader.GetOrdinal("REMARCADA_PARTIDA")) == true ? "SIM" : "NÃO";

                            if (!reader.IsDBNull(reader.GetOrdinal("GOL_TIME_MANDANTE"))) partida.GolMandante = Convert.ToString(reader.GetInt32(reader.GetOrdinal("GOL_TIME_MANDANTE")));
                            if (!reader.IsDBNull(reader.GetOrdinal("GOL_TIME_VISITANTE"))) partida.GolVisitante = Convert.ToString(reader.GetInt32(reader.GetOrdinal("GOL_TIME_VISITANTE")));
                            if (!reader.IsDBNull(reader.GetOrdinal("PONTOS_COMPUTADO")))
                                partida.PontosComputadosString = reader.GetInt32(reader.GetOrdinal("PONTOS_COMPUTADO")) == 1 ? "SIM" : "NÃO";
                            if (!reader.IsDBNull(reader.GetOrdinal("NOVA_DATA_PARTIDA")))
                                partida.DataPartidaRemarcadaString = Convert.ToString(reader.GetDateTime(reader.GetOrdinal("NOVA_DATA_PARTIDA")));
                            else
                                partida.DataPartidaRemarcadaString = "";
                            if (!reader.IsDBNull(reader.GetOrdinal("ESTADIO"))) partida.Estadio = reader.GetString(reader.GetOrdinal("ESTADIO"));
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return partida;
        }
    }
}


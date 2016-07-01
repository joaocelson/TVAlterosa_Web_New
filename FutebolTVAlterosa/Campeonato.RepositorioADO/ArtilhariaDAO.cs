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
    public class ArtilhariaDAO
    {

        public static List<Artilharia_Web> ObterArtilharia()
        {
            List<Artilharia_Web> listaArtilharia = new List<Artilharia_Web>();

            try
            {
                String SQL = @"SELECT ID, ID_JOGADOR, ID_CAMPEONATO, ID_TIME, NUMERO_GOLS, DATA_INATIVACAO
	                                FROM ARTILHARIA
	                              WHERE DATA_INATIVACAO IS NULL
	                              ORDER BY ID_JOGADOR";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artilharia_Web jog = new Artilharia_Web();
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) jog.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_JOGADOR"))) jog.Id_Jogador = reader.GetInt32(reader.GetOrdinal("ID_JOGADOR"));                            
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME"))) jog.Id_Time = reader.GetInt32(reader.GetOrdinal("ID_TIME"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_CAMPEONATO"))) jog.Id_Campeonato = reader.GetInt32(reader.GetOrdinal("ID_CAMPEONATO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("NUMERO_GOLS"))) jog.NumeroGols = Convert.ToString(reader.GetInt32(reader.GetOrdinal("NUMERO_GOLS")));

                            listaArtilharia.Add(jog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaArtilharia;
        }

        public static void InserirArtilharia(Artilharia_Web item)
        {
            try
            {

                String SQL = @"INSERT INTO ARTILHARIA (ID_JOGADOR, ID_CAMPEONATO, ID_TIME, NUMERO_GOLS)
	                                        VALUES ('" + item.Id_Jogador + "', '" + item.Id_Campeonato + "', '" + item.Id_Time + @"', 
                                                    " + item.NumeroGols + ")";

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

        public static void RemoveArtilharia(Artilharia_Web item)
        {
            try
            {
                String SQL = @"UPDATE ARTILHARIA
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

        public static void AtualizaArtilharia(Artilharia_Web item)
        {
            try
            {
                String SQL = @"UPDATE ARTILHARIA
                                    SET ID_JOGADOR = '" + item.Id_Jogador + "', ID_CAMPEONATO = '" + item.Id_Campeonato + "', ID_TIME = '" + item.Id_Time + @"', 
                                        NUMERO_GOLS = " + item.NumeroGols + @"
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
    }
}

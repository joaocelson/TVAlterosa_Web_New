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
    public class JogadorDAO
    {

        public static List<Jogador_Web> ObterJogador()
        {
            List<Jogador_Web> listaJogador = new List<Jogador_Web>();

            try
            {
                String SQL = @"SELECT ID, NOME, POSICAO, DESCRICAO, ID_TIME, ID_CAMPEONATO, DATA_INATIVACAO 
	                                FROM T_JOGADOR
                                  WHERE DATA_INATIVACAO IS NULL
                                  ORDER BY NOME";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Jogador_Web jog = new Jogador_Web();
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) jog.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("NOME"))) jog.Nome = reader.GetString(reader.GetOrdinal("NOME"));
                            if (!reader.IsDBNull(reader.GetOrdinal("POSICAO"))) jog.Posicao = reader.GetString(reader.GetOrdinal("POSICAO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("DESCRICAO"))) jog.Descricao = reader.GetString(reader.GetOrdinal("DESCRICAO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_TIME"))) jog.Id_Time = reader.GetInt32(reader.GetOrdinal("ID_TIME"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ID_CAMPEONATO"))) jog.Id_Campeonato = reader.GetInt32(reader.GetOrdinal("ID_CAMPEONATO"));                            

                            listaJogador.Add(jog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaJogador;
        }

        public static void InserirJogador(Jogador_Web item)
        {
            try
            {

                String SQL = @"INSERT INTO T_JOGADOR (NOME, POSICAO, DESCRICAO, ID_TIME, ID_CAMPEONATO )
	                                        VALUES ('" + item.Nome + "', '" + item.Posicao + "', '" + item.Descricao + @"', 
                                                    " + item.Id_Time + ", " + item.Id_Campeonato + ")";

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

        public static void RemoveJogador(Jogador_Web item)
        {
            try
            {
                String SQL = @"UPDATE T_JOGADOR
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

        public static void RemoveArtilharia(Jogador_Web item)
        {
            try
            {
                String SQL = @"UPDATE ARTILHARIA
                                    SET DATA_INATIVACAO = GETDATE()
                                  WHERE ID_JOGADOR = " + item.Id;

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

        public static void AtualizaJogador(Jogador_Web item)
        {
            try
            {
                String SQL = @"UPDATE T_JOGADOR
                                    SET NOME = '" + item.Nome + "', POSICAO = '" + item.Posicao + "', DESCRICAO = '" + item.Descricao + @"', 
                                        ID_TIME = " + item.Id_Time + ", ID_CAMPEONATO = " + item.Id_Campeonato + @"
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

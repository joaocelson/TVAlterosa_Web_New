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
    public class TimeDAO
    {

        public static List<Time_Web> ObterTime()
        {
            List<Time_Web> listaTime = new List<Time_Web>();

            try
            {
                String SQL = @"SELECT ID, NOME, ESCUDO, PRESIDENTE, DESCRICAO, TELEFONE, DATA_INATIVACAO
	                               FROM TIMES
                                WHERE DATA_INATIVACAO IS NULL
                                  ORDER BY NOME";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Time_Web time = new Time_Web();
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) time.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("NOME"))) time.Nome = reader.GetString(reader.GetOrdinal("NOME"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ESCUDO"))) time.Escudo = reader.GetString(reader.GetOrdinal("ESCUDO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("PRESIDENTE"))) time.Presidente = reader.GetString(reader.GetOrdinal("PRESIDENTE"));
                            if (!reader.IsDBNull(reader.GetOrdinal("DESCRICAO"))) time.Descricao = reader.GetString(reader.GetOrdinal("DESCRICAO"));
                            if (!reader.IsDBNull(reader.GetOrdinal("TELEFONE"))) time.Telefone = reader.GetString(reader.GetOrdinal("TELEFONE"));                            

                            listaTime.Add(time);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaTime;
        }

        public static void InserirTime(Time_Web item)
        {
            try
            {

                String SQL = @"INSERT INTO TIMES (NOME, ESCUDO, PRESIDENTE, DESCRICAO, TELEFONE) 
	                                        VALUES ('" + item.Nome + "', '" + item.Escudo + "', '" + item.Presidente + @"', 
                                                    '" + item.Descricao + "', '" + item.Telefone + "')";

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

        public static void RemoveTime(Time_Web item)
        {           
            try
            {
                String SQL = @"UPDATE TIMES
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

        public static void AtualizaTime(Time_Web item)
        {
            try
            {
                String SQL = @"UPDATE TIMES
                                    SET NOME = '" + item.Nome + "', ESCUDO = '" + item.Escudo + "', PRESIDENTE = '" + item.Presidente  + @"', 
                                        DESCRICAO = '" + item.Descricao + "', TELEFONE = '" + item.Telefone + @"'
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

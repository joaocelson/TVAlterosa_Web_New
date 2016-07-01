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
    public class CampeonatoDAO
    {

        public static List<Campeonato_Web> ObterCampeonato()
        {
            List<Campeonato_Web> listaCampeonato = new List<Campeonato_Web>();

            try
            {
                String SQL = @"SELECT ID, NOME, DATA_INICIO, DATA_INATIVACAO 
	                                FROM CAMPEONATO
                                  WHERE DATA_INATIVACAO IS NULL
                                  ORDER BY NOME";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Campeonato_Web camp = new Campeonato_Web();
                            if (!reader.IsDBNull(reader.GetOrdinal("ID"))) camp.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            if (!reader.IsDBNull(reader.GetOrdinal("NOME"))) camp.Nome = reader.GetString(reader.GetOrdinal("NOME"));
                            if (!reader.IsDBNull(reader.GetOrdinal("DATA_INICIO"))) camp.DataInicio = reader.GetDateTime(reader.GetOrdinal("DATA_INICIO"));                            

                            listaCampeonato.Add(camp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaCampeonato;
        }

        public static void InserirCampeonato(Campeonato_Web item)
        {
            string data = null;
            if (item.DataInicio != null)
            {
                data = item.DataInicio.ToString("yyyy-MM-dd");
            }   
            try
            {

                String SQL = @"INSERT INTO CAMPEONATO (NOME, DATA_INICIO )
	                                        VALUES ('" + item.Nome + "', '" + data + "')";

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

        public static void RemoveCampeonato(Campeonato_Web item)
        {
            try
            {
                String SQL = @"UPDATE CAMPEONATO
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

        public static void AtualizaCampeonato(Campeonato_Web item)
        {
            string data = null;
            if (item.DataInicio != null)
            {
                data = item.DataInicio.ToString("yyyy-MM-dd");
            }  
            try
            {
                String SQL = @"UPDATE CAMPEONATO
                                    SET NOME = '" + item.Nome + "', DATA_INICIO = '" + data + @"'
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

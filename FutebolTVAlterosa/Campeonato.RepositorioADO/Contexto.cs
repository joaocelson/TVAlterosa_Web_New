using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.RepositorioADO
{
    public class Contexto : IDisposable
    {
        private readonly SqlConnection minhaConexao;

        public Contexto()
        {
            minhaConexao = new SqlConnection(ConfigurationManager.ConnectionStrings["CampeonatoConfig"].ConnectionString);
            minhaConexao.Open();
        }

        public void ExecutaComando(string strQuery)
        {
            var cmdComando = new SqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = minhaConexao

            };
            cmdComando.ExecuteNonQuery();
        }

        public SqlDataReader ExecutaComandoComRetorno(string strQuery)
        {
            try
            {
                if (strQuery.Contains(';'))
                {
                    return null;
                }
                else
                {
                    var cmdComando = new SqlCommand(strQuery, minhaConexao);
                    cmdComando.CommandTimeout = 60;
                    return cmdComando.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public void Dispose()
        {
            if (minhaConexao.State == ConnectionState.Open)
                minhaConexao.Close();
        }

        public SqlConnection MinhaConexao()
        {
            return minhaConexao;
        }

        internal void ExecutaComandoTransacao(string strQuery, SqlTransaction transacao)
        {
            var cmdComando = new SqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = minhaConexao,
                Transaction = transacao

            };
            cmdComando.ExecuteNonQuery();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;

namespace Campeonato.RepositorioADO
{
    [global::System.Serializable]
    public class ConexoesException : Exception
    {
        public ConexoesException() { }
        public ConexoesException(string message) : base(message) { }
        public ConexoesException(string message, Exception inner) : base(message, inner) { }
        protected ConexoesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public static class Conexoes
    {
        private static SqlConnection ObterConexaoSql(ref SqlConnection Conn, String NomeConexao, String ConnectionString, String ComandoInicial)
        {
            try
            {
                if (Conn == null) Conn = new SqlConnection(ConnectionString);

                Boolean ConexaoAberta = false;
                while (!ConexaoAberta)
                {
                    switch (Conn.State)
                    {
                        case System.Data.ConnectionState.Open:
                            ConexaoAberta = true;
                            break;

                        case System.Data.ConnectionState.Closed:
                            Conn = new SqlConnection(ConnectionString);
                            Conn.Open();
                            if (ComandoInicial != "") new SqlCommand(ComandoInicial, Conn).ExecuteNonQuery();
                            break;

                        case System.Data.ConnectionState.Executing:
                            for (Int32 Contador = 1; Contador <= 50; Contador++)
                            {
                                Thread.Sleep(100);
                                if (Conn.State != System.Data.ConnectionState.Executing) break;
                                else if (Contador == 50) throw new Exception("Conexão " + NomeConexao + " presa em Execução!");
                            }
                            break;

                        case System.Data.ConnectionState.Broken:
                            Conn.Close();
                            Conn = new SqlConnection(ConnectionString);
                            Conn.Open();
                            if (ComandoInicial != "") new SqlCommand(ComandoInicial, Conn).ExecuteNonQuery();
                            break;

                        case System.Data.ConnectionState.Fetching:
                            for (Int32 Contador = 1; Contador <= 50; Contador++)
                            {
                                Thread.Sleep(100);
                                if (Conn.State != System.Data.ConnectionState.Fetching) break;
                                else if (Contador == 50) throw new Exception("Conexão " + NomeConexao + " presa em Fetching!");
                            }
                            break;

                        case System.Data.ConnectionState.Connecting:
                            for (Int32 Contador = 1; Contador <= 50; Contador++)
                            {
                                Thread.Sleep(100);
                                if (Conn.State != System.Data.ConnectionState.Connecting) break;
                                else if (Contador == 50) throw new Exception("Conexão " + NomeConexao + " presa - Conectando!");
                            }
                            break;

                        default:
                            Conn.Close();
                            Conn = new SqlConnection(ConnectionString);
                            Conn.Open();
                            if (ComandoInicial != "") new SqlCommand(ComandoInicial, Conn).ExecuteNonQuery();
                            break;
                    }

                    Thread.Sleep(50);
                }

                return Conn;
            }
            catch (Exception Ex)
            {
                //TratamentoLog.GravarLog("Conexoes:ObterConexao - " + NomeConexao + ": " + Ex.Message, TratamentoLog.NivelLog.Erro);
                //throw Ex;
                return null;
            }
        }        

        public static SqlConnection ObterConexaoExclusiva()
        {            
            var connectionString = ConfigurationManager.ConnectionStrings["CampeonatoConfig"].ConnectionString;
            SqlConnection Conn = new SqlConnection();

            ObterConexaoSql(ref Conn, "Local", connectionString, "");

            return Conn;
        }
    }
}

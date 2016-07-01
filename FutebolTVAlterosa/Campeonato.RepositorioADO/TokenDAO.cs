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
    public class TokenDAO
    {

        public static List<Token_Web> ObterToken()
        {
            List<Token_Web> listaToken = new List<Token_Web>();

            try
            {
                String SQL = @"SELECT ID, TOKEN FROM TOKEN";

                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
                {
                    SqlCommand comando = new SqlCommand(SQL, conexao);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Token_Web token = new Token_Web();                            
                            if (!reader.IsDBNull(reader.GetOrdinal("TOKEN"))) token.TokenStr = reader.GetString(reader.GetOrdinal("TOKEN"));

                            listaToken.Add(token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaToken;
        }

//        public static void InserirToken(Token item)
//        {
//            try
//            {

//                String SQL = @"INSERT INTO TOKEN (ID, TOKEN )
//	                                        VALUES ('" + item.TokenId + "', '" + item.TokenStr + "')";

//                using (SqlConnection conexao = Conexoes.ObterConexaoExclusiva())
//                {
//                    SqlCommand comando = new SqlCommand(SQL, conexao);
//                    comando.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
    }
}

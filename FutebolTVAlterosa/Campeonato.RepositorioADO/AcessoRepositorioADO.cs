using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.RepositorioADO
{
    public class AcessoRepositorioADO : IRepositorio<Acesso>
    {
        private Contexto contexto;

        public void AtualizarNumeroAcesso()
        {
            try
            {
                var strQuery = "";
                strQuery += " UPDATE acessos SET numero_acessos = (select numero_acessos + 1 from acessos)";
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public int NumeroAcesso()
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = " SELECT * FROM acessos ";
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReader(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private int TransformaReader(SqlDataReader reader)
        {

            while (reader.Read())
            {
                var temObjeto = new Acesso()
                {
                    NumeroAcesso = Convert.ToInt16(reader["numero_acessos"].ToString())
                };

                reader.Close();
                return temObjeto.NumeroAcesso;
            }
            return 0;
        }

        public void Salvar(Acesso entidade)
        {
            throw new NotImplementedException();
        }

        public void Excluir(Acesso entidade)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Acesso> ListarTodos()
        {
            throw new NotImplementedException();
        }

        public Acesso ListarPorId(string id)
        {
            throw new NotImplementedException();
        }
    }
}

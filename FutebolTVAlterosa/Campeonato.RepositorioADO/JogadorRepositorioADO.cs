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
    public class JogadorRepositorioADO : IRepositorio<Jogador>
    {

        private Contexto contexto;

        private void Inserir(Jogador jogador)
        {
            try
            {
                using (contexto = new Contexto())
                {

                    var strQuery = "";
                    strQuery += " INSERT INTO t_jogador (nome, foto, posicao, descricao) ";
                    strQuery += string.Format(" VALUES ('{0}','{1}','{2}','{3}') ",
                        jogador.Nome, jogador.Foto, jogador.Posicao, jogador.Descricao
                        );
                    contexto.ExecutaComando(strQuery);

                    var StrQueryConsulta = "SELECT * from t_jogador t JOIN (SELECT MAX(ID) as id FROM   t_jogador)	max ON t.id = max.id";
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(StrQueryConsulta);
                    Jogador jogadorInserido = TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();

                    var strQuery2 = "";
                    strQuery2 += " INSERT INTO t_jogador_campeonato_time (id_time, id_campeonato, id_jogador) ";
                    strQuery2 += string.Format(" VALUES ({0},{1},{2})",
                        jogador.Time.Id, jogador.Campeonato.Id, jogadorInserido.Id
                        );
                    contexto.ExecutaComando(strQuery2);

                    TratamentoLog.GravarLog("Usuario criado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("Erro ao salvar o usuario.");
            }


        }

        private void Alterar(Jogador jogador)
        {
            var strQuery = "";
            strQuery += " UPDATE t_jogador SET ";
            strQuery += string.Format(" Nome = '{0}', ", jogador.Nome);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void Salvar(Jogador jogador)
        {
            if (jogador.Id > 0)
                Alterar(jogador);
            else
                Inserir(jogador);
        }

        public void Excluir(Jogador jogador)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM t_jogador WHERE Id = {0}", jogador.Id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public IEnumerable<Jogador> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM t_jogador ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Jogador ListarPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM t_jogador WHERE Id = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Jogador> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            if (reader != null)
            {
                var LitaJogador = new List<Jogador>();
                while (reader.Read())
                {

                    var temObjeto = new Jogador()
                    {
                        Id = Convert.ToInt32(reader["id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        Posicao = reader["Posicao"].ToString(),
                        Descricao = reader["Descricao"].ToString(),
                        Foto = reader["Foto"].ToString(),

                    };
                    LitaJogador.Add(temObjeto);
                }
                reader.Close();
                return LitaJogador;
            } return new List<Jogador>();
        }


        public List<Jogador> ListarPorTimeCampeonato(string campeonato, string time)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("select * from t_jogador j inner join t_jogador_campeonato_time ct on ct.id_jogador = j.id where ct.id_time = {0}", time);
                strQuery += string.Format(" and ct.id_campeonato =  {0} ", campeonato);
                strQuery += string.Format(" order by j.nome");
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }
    }
}

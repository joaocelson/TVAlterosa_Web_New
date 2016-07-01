using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.Dominio.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.RepositorioADO
{
    public class ClassificacaoRepositorioADO : IRepositorio<Classificacao>
    {

        private Contexto contexto;

        public ClassificacaoRepositorioADO()
        {

        }

        private List<Classificacao> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            try
            {
                var classificacao = new List<Classificacao>();

                while (reader.Read())
                {
                    var temObjeto = new Classificacao()
                    {
                        Posicao = ((reader.GetName(0) == "POSICAO") ? reader["POSICAO"].ToString() : "0"),
                        Id = reader["Id"].ToString(),
                        IdTime = reader["Id_time"].ToString(),
                        NomeTime = reader["tm_nome"].ToString(),

                        Pontos = reader["pontos"].ToString(),
                        Jogos = reader["jogos"].ToString(),

                        Vitoria = reader["vitoria"].ToString(),
                        Derrota = reader["derrota"].ToString(),
                        Empate = reader["empate"].ToString(),

                        GolPro = reader["gol_pro"].ToString(),
                        GolContra = reader["gol_contra"].ToString(),
                        SaldoGol = reader["saldo_gols"].ToString(),
                        IdCampeonato = reader["id_campeonato"].ToString(),

                    };
                    classificacao.Add(temObjeto);
                }
                reader.Close();
                return classificacao;
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::TransformaReaderEmListaDeObjeto:. Erro ao converte lista de classificação - " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        private List<Campeoes> TransformaReaderEmListaDeObjetoCampeoes(SqlDataReader reader)
        {
            try
            {
                var classificacao = new List<Campeoes>();

                while (reader.Read())
                {
                    var temObjeto = new Campeoes()
                    {
                        Id = reader["Id"].ToString(),
                        IdTime = reader["Id_time"].ToString(),
                        NomeTime = reader["tm_nome"].ToString(),
                        AnoCampeonato = reader["ano_campeonato"].ToString(),
                        DescricaoTitulo = reader["descricao_titulo"].ToString(),
                        IdCampeonato = reader["id_campeonato"].ToString(),
                        EscudoPequeno = (reader["escudo_pequeno"].ToString().Equals("") ? "Vazio.jpg" : reader["escudo_pequeno"].ToString()),
                        NumeroTitulos = reader["Titulos"].ToString()

                    };
                    classificacao.Add(temObjeto);
                }
                reader.Close();
                return classificacao;
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::TransformaReaderEmListaDeObjeto:. Erro ao converte lista de classificação - " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public void Salvar(Classificacao classificao)
        {
            try
            {
                using (contexto = new Contexto())
                {

                    var strQueryCampeonato = "INSERT INTO classificacao ( id_campeonato,id_time,pontos, jogos, vitoria, derrota, empate, gol_pro, gol_contra)";
                    strQueryCampeonato += string.Format(" VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                         classificao.IdCampeonato, classificao.IdTime, 0, 0, 0, 0, 0, 0, 0);

                    contexto.ExecutaComando(strQueryCampeonato);
                    TratamentoLog.GravarLog("ClassificaçãoInserida: " + classificao.IdCampeonato + " x " + "  timeMandandate.Id " + classificao.IdTime);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::Salvar:. Erro a inserir classificação em branco. " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        public void Excluir(Classificacao entidade)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Classificacao> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT c.*,c.gol_pro - c.gol_contra as saldo_gols, tm.nome tm_nome " +
                               "FROM classificacao c INNER JOIN " +
                               "Times tm on tm.id = c.id_time";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader); //TransformaReaderEmListaDeObjeto(retornoDataReader);
            }

        }

        public IEnumerable<Classificacao> ListarClassicacaoPorCampeonato(string id)
        {
            try
            {

                using (contexto = new Contexto())
                {
                    var strQuery = "SELECT POSICAO = Row_Number() Over(Order by c.pontos Desc,  c.vitoria,( c.gol_pro - c.gol_contra) desc), c.*,c.gol_pro - c.gol_contra as saldo_gols, tm.nome tm_nome, c.id_campeonato id_campeonato " +
                                   "FROM classificacao c INNER JOIN " +
                                   "Times tm on tm.id = c.id_time INNER JOIN " +
                                   "Campeonato ca on ca.id = c.id_campeonato ";
                    strQuery += string.Format(" WHERE c.id_campeonato  = '{0}' ", id);
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader); //TransformaReaderEmListaDeObjeto(retornoDataReader);
                }

            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::ListarClassicacaoPorCampeonato:. Erro ao Listar Classificação por Usuario - " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public Classificacao ListarPorId(string id, string idCampeonato, Contexto contexto)
        {
            try
            {

                var strQuery = "SELECT c.*,c.gol_pro - c.gol_contra as saldo_gols, tm.nome tm_nome " +
                               "FROM classificacao c INNER JOIN " +
                               "Times tm on tm.id = c.id_time";
                strQuery += string.Format(" WHERE c.id_time  = '{0}'", id);
                strQuery += string.Format(" AND c.id_campeonato  = '{0}'", idCampeonato);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).First(); //TransformaReaderEmListaDeObjeto(retornoDataReader);

            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::ListarPorId:. Erro ao Listar Classificação por Usuario - " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public Classificacao ListarPorId(string id)
        {
            try
            {

                var strQuery = "SELECT c.*,c.gol_pro - c.gol_contra as saldo_gols, tm.nome tm_nome " +
                               "FROM classificacao c INNER JOIN " +
                               "Times tm on tm.id = c.id_time";
                strQuery += string.Format(" WHERE c.id_time = '{0}'", id);
                //var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery, transacao);
                return null;//TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault(); //TransformaReaderEmListaDeObjeto(retornoDataReader);

            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::ListarPorId:. Erro ao Listar Classificação por Usuario - " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }


        }

        public IEnumerable<Campeoes> ListarCampeoesPorCampeonato(string id)
        {
            try
            {

                using (contexto = new Contexto())
                {
                    var strQuery = "SELECT c.*, tm.nome tm_nome, tm.escudo escudo_pequeno, " +
                                    "(select  count(c2.id_time) from campeoes c2 where c2.id_time = tm.id group by c2.id_time) Titulos " +
                                   "FROM campeoes c INNER JOIN " +
                                   "Times tm on tm.id = c.id_time";
                    strQuery += string.Format(" WHERE c.id_campeonato  = '{0}' ", id);
                    strQuery += string.Format(" ORDER BY c.ano_campeonato");
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjetoCampeoes(retornoDataReader); //TransformaReaderEmListaDeObjeto(retornoDataReader);
                }

            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("ClassificacaoRepositorioADO::ListarClassicacaoPorCampeonato:. Erro ao Listar Classificação por Usuario - " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

    }
}

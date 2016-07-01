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
    public class TimeRepositorioADO : IRepositorio<Time>
    {
        private Contexto contexto;

        private void Inserir(Time time)
        {
            try
            {
                var strQuery = "";
                strQuery += " INSERT INTO Times (nome, data_fundacao, escudo) ";
                strQuery += string.Format(" VALUES ('{0}','{1}','{2}')",
                    time.Nome, time.DataFundacao, time.EscudoPequeno
                    //strQuery += " INSERT INTO Times (nome, data_fundacao, presidente, telefone, descricao) ";
                    //strQuery += string.Format(" VALUES ('{0}','{1}','{2}','{3}','{4}') ",
                    //    time.Nome, time.DataFundacao, time.Presidente, time.Telefone, time.Descricao
                    );
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("TimeRepositoerioADO::Inserir:. Erro ao salvar time. " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        private void Alterar(Time time)
        {
            var strQuery = "";
            strQuery += " UPDATE Times SET ";
            strQuery += string.Format(" Nome = '{0}', ", time.Nome);
            strQuery += string.Format(" data_fundacao = '{0}', ", time.DataFundacao);
            strQuery += string.Format(" presidente = '{0}', ", time.Presidente);
            strQuery += string.Format(" descricao= '{0}', ", time.Descricao);
            strQuery += string.Format(" telefone = '{0}', ", time.Telefone);
            strQuery += string.Format(" escudo = '{0}' ", time.EscudoPequeno);
            strQuery += string.Format(" WHERE Id = {0} ", time.Id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void Salvar(Time time)
        {
            if (time.Id > 0)
                Alterar(time);
            else
                Inserir(time);
        }

        public void Excluir(Time time)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM Times WHERE Id = {0}", time.Id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public IEnumerable<Time> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM Times ORDER BY nome ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Time ListarPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM Times WHERE Id = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Time> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var time = new List<Time>();
            while (reader.Read())
            {
                DateTime DataFundacaoNew = new DateTime();
                if (!reader["data_fundacao"].ToString().Equals(""))
                {
                    DataFundacaoNew = DateTime.Parse(reader["data_fundacao"].ToString());
                }
                var temObjeto = new Time()
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    Nome = reader["Nome"].ToString(),
                    DataFundacao = DataFundacaoNew,
                    Presidente = reader["presidente"].ToString(),
                    Telefone = reader["telefone"].ToString(),
                    Descricao = reader["descricao"].ToString(),
                    EscudoPequeno = (reader["escudo"].ToString().Equals("") ? "Vazio.jpg" : reader["escudo"].ToString())
                };
                time.Add(temObjeto);
            }
            reader.Close();
            return time;
        }

        public IEnumerable<Time> ListarTimesCampeonato(string idCampeonato)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * ");
                strQuery += string.Format("FROM Times t INNER JOIN ");
                strQuery += string.Format("time_campeonato c on c.id_Time = t.id ");
                strQuery += string.Format("WHERE c.id_campeonato = {0}  ", idCampeonato);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }
    }
}

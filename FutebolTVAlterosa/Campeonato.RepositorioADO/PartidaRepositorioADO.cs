using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.Dominio.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Campeonato.RepositorioADO
{
    public class PartidaRepositorioADO : IRepositorio<Partida>
    {
        private Contexto contexto;

        private void Inserir(Partida Partida)
        {
            var strQuery = "";
            strQuery += " INSERT INTO Partida (id_time_mandante, id_time_visitante, data_partida, local_partida, id_campeonato, rodada) ";
            strQuery += string.Format(" VALUES ({0},{1},'{2}','{3}',{4},{5}) ",
            Partida.IdTimeMandante, Partida.IdTimeVisitante, Partida.DataPartida, Partida.LocalPartida, Partida.IdCampeonato, Partida.Rodada);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        private void Alterar(Partida Partida)
        {
            var strQuery = "";
            strQuery += " UPDATE Partida SET ";
            strQuery += string.Format(" id_time_mandante = '{0}', ", Partida.IdTimeMandante);
            strQuery += string.Format(" id_time_visitante = '{0}', ", Partida.IdTimeVisitante);
            //strQuery += string.Format(" gol_time_mandante = '{0}', ", Partida.GolMandante);
            //strQuery += string.Format(" gol_time_visitante = '{0}', ", Partida.GolVisitante);
            strQuery += string.Format(" local_partida = '{0}', ", Partida.LocalPartida);
            strQuery += string.Format(" data_partida = '{0}', ", Partida.DataPartida.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            strQuery += string.Format(" remarcada_partida = '{0}', ", Partida.Remarcada);
            strQuery += string.Format(" rodada = '{0}' ", Partida.Rodada);
            strQuery += string.Format(", estadio = '{0}' ", Partida.Estadio.Trim());
            //strQuery += string.Format(" nova_data_partida = '{0}' ", Partida.DataPartidaRemarcada.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            strQuery += string.Format(" WHERE Id = {0} ", Partida.Id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void Salvar(Partida Partida)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("en-US");

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            if (Partida.Id > 0)
                Alterar(Partida);
            else
                Inserir(Partida);
        }

        public void Excluir(Partida Partida)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM Partida WHERE Id = {0}", Partida.Id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public IEnumerable<Partida> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                               "FROM partida p INNER JOIN " +
                               "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                               "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato  ORDER BY  data_partida";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Partida ListarPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                               "FROM partida p INNER JOIN " +
                               "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                               "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato  WHERE p.Id = {0} ORDER BY CAST(p.rodada AS INT)", id);


                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        public Partida ListarPorId(string id, Contexto contexo)
        {

            var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                           "FROM partida p INNER JOIN " +
                           "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                           "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                           "campeonato c on c.id = p.id_campeonato  WHERE p.Id = {0} ORDER BY CAST(p.rodada AS INT)", id);
            var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
            return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();

        }

        private List<Partida> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            FotoVideoRepositorioADO fotoVideo = new FotoVideoRepositorioADO();

            var Partida = new List<Partida>();
            while (reader.Read())
            {
                DateTime DataPartidaNew = new DateTime();
                DateTime DataPartidaRemarcada = new DateTime();
                Boolean partidaRemarcada = false;
                Boolean PontosComputados = false;
                if (!reader["data_partida"].ToString().Equals(""))
                {
                    DataPartidaNew = DateTime.Parse(reader["data_partida"].ToString());
                }

                if (!reader["nova_data_partida"].ToString().Equals(""))
                {
                    DataPartidaRemarcada = DateTime.Parse(reader["nova_data_partida"].ToString());
                }
                if (!reader["remarcada_partida"].ToString().Equals(""))
                {
                    partidaRemarcada = Convert.ToBoolean(reader["remarcada_partida"].ToString());
                }
                if (reader["pontos_computado"] != null && !reader["pontos_computado"].ToString().Equals(""))
                {
                    if (reader["pontos_computado"].ToString().Equals("1")) { PontosComputados = true; }
                }

                var temObjeto = new Partida()
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    IdCampeonato = reader["Id_campeonato"].ToString(),
                    IdTimeMandante = reader["id_time_mandante"].ToString(),
                    IdTimeVisitante = reader["id_time_visitante"].ToString(),
                    GolMandante = reader["gol_time_mandante"].ToString(),
                    GolVisitante = reader["gol_time_visitante"].ToString(),
                    DataPartida = DataPartidaNew,
                    DataPartidaRemarcada = DataPartidaRemarcada,
                    LocalPartida = reader["local_partida"].ToString(),
                    Remarcada = partidaRemarcada,
                    TimeMandante = reader["tm_nome"].ToString(),
                    Rodada = (reader["rodada"].ToString().Equals("0") ? "Semi-Final" : (reader["rodada"].ToString().Equals("1000") ? "Final" : reader["rodada"].ToString())),
                    PontosComputados = PontosComputados,
                    TimeVisitante = reader["tv_nome"].ToString(),
                    EscudoPequenoMandante = (reader["tm_escudo"].ToString().Equals("") ? "Vazio.jpg" : reader["tm_escudo"].ToString()),
                    EscudoPequenoVisitante = (reader["tv_escudo"].ToString().Equals("") ? "Vazio.jpg" : reader["tv_escudo"].ToString()),
                    Estadio = reader["estadio"].ToString().Trim(),
                    


                };
                temObjeto.ListaFotosVideos = fotoVideo.ListarFotoVideoTimePartidaCampeonato(temObjeto);
                Partida.Add(temObjeto);
            }
            reader.Close();
            return Partida;
        }

        public void GerarPartidaAutomaticamenteTime(List<Time> listaTimes, String idCampeonatoSelecionado)
        {
            SqlTransaction transacao = null;
            try
            {
                using (contexto = new Contexto())
                {
                    transacao = contexto.MinhaConexao().BeginTransaction(IsolationLevel.ReadCommitted);

                    for (int i = 0; i < listaTimes.ToList().Count; i++)
                    {
                        for (int j = i + 1; j <= listaTimes.ToList().Count - 1; j++)
                        {
                            Time timeMandandate = listaTimes[i];
                            Time timeVisitante = listaTimes[j];

                            var strQuery = "INSERT INTO partida (id_campeonato,id_time_mandante,id_time_visitante)";
                            strQuery += string.Format(" VALUES ('{0}','{1}','{2}')",
                                 idCampeonatoSelecionado, timeMandandate.Id, timeVisitante.Id);

                            contexto.ExecutaComandoTransacao(strQuery, transacao);
                            TratamentoLog.GravarLog("idCampeonatoSelecionado: " + idCampeonatoSelecionado + " x " + "  timeMandandate.Id " + timeMandandate.Id + " x " + "  timeVisitante.Id " + timeVisitante.Id);
                        }

                        //Inserir a tabela em branco ao gerar as partidas
                        ClassificacaoRepositorioADO classificacao = new ClassificacaoRepositorioADO();
                        Classificacao objClassificacao = new Classificacao();
                        objClassificacao.IdCampeonato = idCampeonatoSelecionado;
                        objClassificacao.IdTime = listaTimes[i].Id.ToString();
                        classificacao.Salvar(objClassificacao);
                    }

                    transacao.Commit();
                }
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                TratamentoLog.GravarLog("PartidaRepositorioADO::GerarPartidaAutomaticamenteTime:. Erro ao geras Partidas" + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        public void AtualizarResultado(Partida partida)
        {
            SqlTransaction transacao = null;
            try
            {
                using (contexto = new Contexto())
                {
                    ClassificacaoRepositorioADO classificacao = new ClassificacaoRepositorioADO();
                    Partida partidaBancoDados = this.ListarPorId(partida.Id.ToString(), contexto);
                    Classificacao classificacaoMandante = classificacao.ListarPorId(partida.IdTimeMandante, partida.IdCampeonato, contexto);
                    Classificacao classificacaoVisitante = classificacao.ListarPorId(partida.IdTimeVisitante, partida.IdCampeonato, contexto);


                    transacao = contexto.MinhaConexao().BeginTransaction(IsolationLevel.ReadCommitted);



                    //Se os pontos a partida já foram computados, temos que atualizar a tabela retirando os dados computados
                    if (partida.PontosComputados)
                    {

                        RemoverClassificacaoVisitante(partidaBancoDados, ref transacao, ref classificacaoVisitante);

                        RemoverClassificacaoMandante(partidaBancoDados, ref transacao, ref classificacaoMandante);

                    }

                    //Classificacao classificacaoMandante2 = classificacao.ListarPorId(partida.IdTimeMandante, contexto2);
                    //Classificacao classificacaoVisitante2 = classificacao.ListarPorId(partida.IdTimeVisitante, contexto2);


                    AtualizarClassificacaoVisitante(partida, ref transacao, ref classificacaoVisitante);

                    AtualizarClassificacaoMandante(partida, ref transacao, ref classificacaoMandante);


                    var strQuery = "";
                    strQuery += " UPDATE partida SET";
                    strQuery += string.Format(" gol_time_mandante = '{0}', ", partida.GolMandante);
                    strQuery += string.Format(" gol_time_visitante = '{0}', ", partida.GolVisitante);
                    strQuery += string.Format(" pontos_computado = '{0}' ", 1);
                    strQuery += string.Format(" WHERE Id = {0} ", partida.Id);
                    contexto.ExecutaComandoTransacao(strQuery, transacao);

                    transacao.Commit();
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::AtualizarResultado:. Erro ao AtualizarResultado - " + ex.Message, TratamentoLog.NivelLog.Erro);
                transacao.Rollback();
            }
        }

        private void RemoverClassificacaoMandante(Partida partida, ref SqlTransaction transacao, ref Classificacao classificacaoMandante)
        {
            try
            {
                //Classificacao  = classificacao.ListarPorId(partida.IdTimeMandante, contexto2);
                int vitorias, derrotas, empates, pontos, golsPro, golsContra, jogos;
                golsPro = Convert.ToInt16(classificacaoMandante.GolPro) - Convert.ToInt16(partida.GolMandante);
                golsContra = Convert.ToInt16(classificacaoMandante.GolContra) - Convert.ToInt16(partida.GolVisitante);
                jogos = Convert.ToInt16(classificacaoMandante.Jogos) - 1;

                if (Convert.ToInt16(partida.GolMandante) > Convert.ToInt16(partida.GolVisitante))
                {
                    pontos = Convert.ToInt16(classificacaoMandante.Pontos) - 3;
                    vitorias = Convert.ToInt16(classificacaoMandante.Vitoria) - 1;
                    derrotas = Convert.ToInt16(classificacaoMandante.Derrota);
                    empates = Convert.ToInt16(classificacaoMandante.Empate);

                }
                else if (Convert.ToInt16(partida.GolMandante) < Convert.ToInt16(partida.GolVisitante))
                {
                    pontos = Convert.ToInt16(classificacaoMandante.Pontos);
                    derrotas = Convert.ToInt16(classificacaoMandante.Derrota) - 1;
                    vitorias = Convert.ToInt16(classificacaoMandante.Vitoria);
                    empates = Convert.ToInt16(classificacaoMandante.Empate);
                }
                else
                {
                    pontos = Convert.ToInt16(classificacaoMandante.Pontos) - 1;
                    empates = Convert.ToInt16(classificacaoMandante.Empate) - 1;
                    vitorias = Convert.ToInt16(classificacaoMandante.Vitoria);
                    derrotas = Convert.ToInt16(classificacaoMandante.Derrota);

                }

                classificacaoMandante.Pontos = pontos.ToString();
                classificacaoMandante.Empate = empates.ToString();
                classificacaoMandante.Vitoria = vitorias.ToString();
                classificacaoMandante.Derrota = derrotas.ToString();
                classificacaoMandante.Jogos = jogos.ToString();
                classificacaoMandante.GolPro = golsPro.ToString();
                classificacaoMandante.GolContra = golsContra.ToString();

                var strQuery = "";
                strQuery += " UPDATE classificacao SET ";
                strQuery += string.Format(" pontos = '{0}', ", pontos);
                strQuery += string.Format(" jogos = '{0}', ", jogos);
                strQuery += string.Format(" vitoria = '{0}', ", vitorias);
                strQuery += string.Format(" derrota = '{0}', ", derrotas);
                strQuery += string.Format(" empate = '{0}', ", empates);
                strQuery += string.Format(" gol_pro = '{0}', ", golsPro);
                strQuery += string.Format(" gol_contra = '{0}' ", golsContra);
                strQuery += string.Format(" WHERE id_time = {0} ", partida.IdTimeMandante);
                strQuery += string.Format(" AND id_campeonato = {0} ", partida.IdCampeonato);
                contexto.ExecutaComandoTransacao(strQuery, transacao);
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::RemoverClassificacaoMandante:. Erro ao AtualizarResultado - " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        private void RemoverClassificacaoVisitante(Partida partida, ref SqlTransaction transacao, ref Classificacao classificacaoVisitante)
        {
            try
            {
                //Classificacao classificacaoVisitante = classificacao.ListarPorId(partida.IdTimeVisitante, contexto2);
                int vitorias, derrotas, empates, pontos, golsPro, golsContra, jogos;
                golsPro = Convert.ToInt16(classificacaoVisitante.GolPro) - Convert.ToInt16(partida.GolVisitante);
                golsContra = Convert.ToInt16(classificacaoVisitante.GolContra) - Convert.ToInt16(partida.GolMandante);
                jogos = Convert.ToInt16(classificacaoVisitante.Jogos) - 1;

                if (Convert.ToInt16(partida.GolVisitante) > Convert.ToInt16(partida.GolMandante))
                {
                    pontos = Convert.ToInt16(classificacaoVisitante.Pontos) - 3;
                    vitorias = Convert.ToInt16(classificacaoVisitante.Vitoria) - 1;
                    derrotas = Convert.ToInt16(classificacaoVisitante.Derrota);
                    empates = Convert.ToInt16(classificacaoVisitante.Empate);

                }
                else if (Convert.ToInt16(partida.GolVisitante) < Convert.ToInt16(partida.GolMandante))
                {
                    pontos = Convert.ToInt16(classificacaoVisitante.Pontos);
                    derrotas = Convert.ToInt16(classificacaoVisitante.Derrota) - 1;
                    vitorias = Convert.ToInt16(classificacaoVisitante.Vitoria);
                    empates = Convert.ToInt16(classificacaoVisitante.Empate);
                }
                else
                {
                    pontos = Convert.ToInt16(classificacaoVisitante.Pontos) - 1;
                    empates = Convert.ToInt16(classificacaoVisitante.Empate) - 1;
                    vitorias = Convert.ToInt16(classificacaoVisitante.Vitoria);
                    derrotas = Convert.ToInt16(classificacaoVisitante.Derrota);

                }

                classificacaoVisitante.Pontos = pontos.ToString();
                classificacaoVisitante.Empate = empates.ToString();
                classificacaoVisitante.Vitoria = vitorias.ToString();
                classificacaoVisitante.Derrota = derrotas.ToString();
                classificacaoVisitante.Jogos = jogos.ToString();
                classificacaoVisitante.GolPro = golsPro.ToString();
                classificacaoVisitante.GolContra = golsContra.ToString();

                var strQuery = "";
                strQuery += " UPDATE classificacao SET ";
                strQuery += string.Format(" pontos = '{0}', ", pontos);
                strQuery += string.Format(" jogos = '{0}', ", Convert.ToInt16(classificacaoVisitante.Jogos) - 1);
                strQuery += string.Format(" vitoria = '{0}', ", vitorias);
                strQuery += string.Format(" derrota = '{0}', ", derrotas);
                strQuery += string.Format(" empate = '{0}', ", empates);
                strQuery += string.Format(" gol_pro = '{0}', ", Convert.ToInt16(classificacaoVisitante.GolPro) - Convert.ToInt16(partida.GolVisitante));
                strQuery += string.Format(" gol_contra = '{0}' ", Convert.ToInt16(classificacaoVisitante.GolContra) - Convert.ToInt16(partida.GolMandante));
                strQuery += string.Format(" WHERE id_time = {0} ", partida.IdTimeVisitante);
                strQuery += string.Format(" AND id_campeonato = {0} ", partida.IdCampeonato);
                contexto.ExecutaComandoTransacao(strQuery, transacao);
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::RemoverClassificacaoVisitante:. Erro ao AtualizarResultado - " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        private void AtualizarClassificacaoVisitante(Partida partida, ref SqlTransaction transacao, ref Classificacao classificacaoVisitante)
        {
            try
            {
                // Classificacao classificacaoVisitante = classificacao.ListarPorId(partida.IdTimeVisitante, contexto2);
                int vitorias, derrotas, empates, pontos, golsPro, golsContra, jogos;

                golsPro = Convert.ToInt16(classificacaoVisitante.GolPro) + Convert.ToInt16(partida.GolVisitante);
                golsContra = Convert.ToInt16(classificacaoVisitante.GolContra) + Convert.ToInt16(partida.GolMandante);
                jogos = Convert.ToInt16(classificacaoVisitante.Jogos) + 1;

                if (Convert.ToInt16(partida.GolVisitante) > Convert.ToInt16(partida.GolMandante))
                {
                    pontos = Convert.ToInt16(classificacaoVisitante.Pontos) + 3;
                    vitorias = Convert.ToInt16(classificacaoVisitante.Vitoria) + 1;
                    derrotas = Convert.ToInt16(classificacaoVisitante.Derrota);
                    empates = Convert.ToInt16(classificacaoVisitante.Empate);
                }
                else if (Convert.ToInt16(partida.GolVisitante) < Convert.ToInt16(partida.GolMandante))
                {
                    pontos = Convert.ToInt16(classificacaoVisitante.Pontos);
                    derrotas = Convert.ToInt16(classificacaoVisitante.Derrota) + 1;
                    vitorias = Convert.ToInt16(classificacaoVisitante.Vitoria);
                    empates = Convert.ToInt16(classificacaoVisitante.Empate);
                }
                else
                {
                    pontos = Convert.ToInt16(classificacaoVisitante.Pontos) + 1;
                    empates = Convert.ToInt16(classificacaoVisitante.Empate) + 1;
                    vitorias = Convert.ToInt16(classificacaoVisitante.Vitoria);
                    derrotas = Convert.ToInt16(classificacaoVisitante.Derrota);

                }



                classificacaoVisitante.Pontos = pontos.ToString();
                classificacaoVisitante.Empate = empates.ToString();
                classificacaoVisitante.Vitoria = vitorias.ToString();
                classificacaoVisitante.Derrota = derrotas.ToString();
                classificacaoVisitante.Jogos = jogos.ToString();
                classificacaoVisitante.GolPro = golsPro.ToString();
                classificacaoVisitante.GolContra = golsContra.ToString();


                var strQuery = "";
                strQuery += " UPDATE classificacao SET ";
                strQuery += string.Format(" pontos = '{0}', ", pontos);
                strQuery += string.Format(" jogos = '{0}', ", jogos);
                strQuery += string.Format(" vitoria = '{0}', ", vitorias);
                strQuery += string.Format(" derrota = '{0}', ", derrotas);
                strQuery += string.Format(" empate = '{0}', ", empates);
                strQuery += string.Format(" gol_pro = '{0}', ", golsPro);
                strQuery += string.Format(" gol_contra = '{0}' ", golsContra);
                strQuery += string.Format(" WHERE id_time = {0} ", partida.IdTimeVisitante);
                strQuery += string.Format(" AND id_campeonato = {0} ", partida.IdCampeonato);

                contexto.ExecutaComandoTransacao(strQuery, transacao);


            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::AtualizarClassificacaoVisitante:. Erro ao AtualizarResultado - " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        private void AtualizarClassificacaoMandante(Partida partida, ref SqlTransaction transacao, ref Classificacao classificacaoMandante)
        {
            try
            {
                //Classificacao classificacaoMandante = classificacao.ListarPorId(partida.IdTimeMandante, contexto2);
                int vitorias, derrotas, empates, pontos;

                if (Convert.ToInt16(partida.GolMandante) > Convert.ToInt16(partida.GolVisitante))
                {
                    pontos = Convert.ToInt16(classificacaoMandante.Pontos) + 3;
                    vitorias = Convert.ToInt16(classificacaoMandante.Vitoria) + 1;
                    derrotas = Convert.ToInt16(classificacaoMandante.Derrota);
                    empates = Convert.ToInt16(classificacaoMandante.Empate);

                }
                else if (Convert.ToInt16(partida.GolMandante) < Convert.ToInt16(partida.GolVisitante))
                {
                    pontos = Convert.ToInt16(classificacaoMandante.Pontos);
                    derrotas = Convert.ToInt16(classificacaoMandante.Derrota) + 1;
                    vitorias = Convert.ToInt16(classificacaoMandante.Vitoria);
                    empates = Convert.ToInt16(classificacaoMandante.Empate);
                }
                else
                {
                    pontos = Convert.ToInt16(classificacaoMandante.Pontos) + 1;
                    empates = Convert.ToInt16(classificacaoMandante.Empate) + 1;
                    vitorias = Convert.ToInt16(classificacaoMandante.Vitoria);
                    derrotas = Convert.ToInt16(classificacaoMandante.Derrota);

                }

                var strQuery = "";
                strQuery += " UPDATE classificacao SET ";
                strQuery += string.Format(" pontos = '{0}', ", pontos);
                strQuery += string.Format(" jogos = '{0}', ", Convert.ToInt16(classificacaoMandante.Jogos) + 1);
                strQuery += string.Format(" vitoria = '{0}', ", vitorias);
                strQuery += string.Format(" derrota = '{0}', ", derrotas);
                strQuery += string.Format(" empate = '{0}', ", empates);
                strQuery += string.Format(" gol_pro = '{0}', ", Convert.ToInt16(classificacaoMandante.GolPro) + Convert.ToInt16(partida.GolMandante));
                strQuery += string.Format(" gol_contra = '{0}' ", Convert.ToInt16(classificacaoMandante.GolContra) + Convert.ToInt16(partida.GolVisitante));
                strQuery += string.Format(" WHERE id_time = {0} ", partida.IdTimeMandante);
                strQuery += string.Format(" AND id_campeonato = {0} ", partida.IdCampeonato);
                contexto.ExecutaComandoTransacao(strQuery, transacao);
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::AtualizarClassificacaoMandante:. Erro ao AtualizarResultado - " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        public IEnumerable<Partida> ListaTabelaPorCampeonato(string id)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                   "FROM partida p INNER JOIN " +
                                   "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                   "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato where c.id = {0} ORDER BY  data_partida desc ", id);
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarTabelPorCampeonato:. Erro ao consultar tabela");
                return null;
            }
        }

        public object ListarUltimaRodada()
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                   "FROM partida p INNER JOIN " +
                                   "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                   "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato WHERE  CONVERT (char(10),data_partida, 103)  =   ( ");
                    strQuery += string.Format(" SELECT TOP 1  CONVERT (char(10),data_partida, 103) FROM partida WHERE data_partida <= GETDATE() order by data_partida desc) ORDER BY p.id_campeonato, data_partida desc");
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarUltimaRodada:. Erro ao Consultar última rodada");
                return null;
            }
        }

        public object ListarProximaRodada()
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                   "FROM partida p INNER JOIN " +
                                   "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                   "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato WHERE  CONVERT (char(10),data_partida, 103)  =   ( ");
                    strQuery += string.Format(" SELECT TOP 1  CONVERT (char(10),data_partida, 103) FROM partida WHERE data_partida > GETDATE() order by data_partida) ORDER BY data_partida");
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarProximaRodada:. Erro ao ListarProximaRodada: " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public object ListarProximaRodadaPorNumero()
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                   "FROM partida p INNER JOIN " +
                                   "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                   "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato WHERE  p.rodada  =   ( ");
                    strQuery += string.Format(" SELECT TOP 1  p.rodada  FROM partida p WHERE data_partida > GETDATE() order by data_partida) ORDER BY data_partida");
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarProximaRodada:. Erro ao ListarProximaRodada: " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public IEnumerable<Partida> ListarProximaRodadaPorBolao(string id)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                    "FROM partida p INNER JOIN " +
                                    "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                    "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                    "campeonato c on c.id = p.id_campeonato WHERE  CONVERT (char(10),data_partida, 103)  =   ( ");
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        strQuery += string.Format(" SELECT TOP 1  CONVERT (char(10),data_partida, 103) FROM partida WHERE  CONVERT(char(10),data_partida, 103) = CONVERT (char(10),GETDATE(), 103)  order by data_partida) AND C.ID_BOLAO = {0} ORDER BY data_partida", id);
                    }
                    else
                    {
                        strQuery += string.Format(" SELECT TOP 1  CONVERT (char(10),data_partida, 103) FROM partida WHERE data_partida > GETDATE()  order by data_partida) AND C.ID_BOLAO = {0} ORDER BY data_partida", id);
                    }

                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarProximaRodada:. Erro ao ListarProximaRodada: " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }
             
        public void ComentarPartida(string id, string comentario)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("insert into T_JOGO_ONLINE (id_partida, data_hora_comentario, comentario)");
                strQuery += string.Format(" values ({0}, GetDate(), '{1}')", id, comentario);
                contexto.ExecutaComando(strQuery);

            }
        }

        public IEnumerable<JogoOnline> ComentarioPartida(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_jogo_online WHERE id_partida = {0} ORDER BY data_hora_comentario desc", id);

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjetoComentario(retornoDataReader);
            }
        }

        private List<JogoOnline> TransformaReaderEmListaDeObjetoComentario(SqlDataReader reader)
        {
            try
            {
                List<JogoOnline> comentario = new List<JogoOnline>();

                while (reader.Read())
                {
                    DateTime DataHoraComentario = new DateTime();

                    if (!reader["data_hora_comentario"].ToString().Equals(""))
                    {
                        DataHoraComentario = DateTime.Parse(reader["data_hora_comentario"].ToString());
                    }

                    var temObjeto = new JogoOnline()
                    {
                        Id = reader["Id"].ToString(),
                        DataHoraComentario = reader["data_hora_comentario"].ToString(),
                        Comentario = reader["comentario"].ToString(),
                        IdPartida = reader["id_partida"].ToString()
                    };

                    comentario.Add(temObjeto);
                }
                reader.Close();
                return comentario;
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioaADO::TransformaReaderEmListaDeObjetoComentario: Erro ao converter comentários. ", TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public IEnumerable<Partida> ListarPartidasPorData(string data)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                    "FROM partida p INNER JOIN " +
                                    "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                    "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                    "campeonato c on c.id = p.id_campeonato WHERE  CONVERT (char(10),data_partida, 103)  = '{0}'", data);
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarPartidasPorData:. Erro ao ListarPartidasPorData: " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }

        public IEnumerable<Partida> ListaTabelaPorCampeonatoSegundaDivisao(string id)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.*, tm.nome tm_nome, tm.escudo tm_escudo, tv.nome tv_nome, tv.escudo tv_escudo, c.nome nome_campeonato " +
                                   "FROM partida p INNER JOIN " +
                                   "times tm on tm.id = p.id_time_mandante INNER JOIN " +
                                   "times tv on tv.id = p.id_time_visitante INNER JOIN " +
                                   "campeonato c on c.id = p.id_campeonato where c.id = {0} or c.id = {1} ORDER BY CAST(p.rodada AS INT) ", id, "4");
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::ListarTabelPorCampeonato:. Erro ao consultar tabela");
                return null;
            }
        }

        public List<AoVivo> PartidaAoVivo(string id)
        {
            try
            {  
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format("SELECT p.* "+
                                    "FROM t_jogo_online p " +
                                    "WHERE p.id_partida = " + id + 
                                    "ORDER BY p.data_hora_comentario desc");
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    List<AoVivo> listaComentarios = new List<AoVivo>();
                    while (retornoDataReader.Read())
                    {
                        AoVivo aoVivo = new AoVivo();
                        DateTime data = DateTime.Parse(retornoDataReader["data_hora_comentario"].ToString());
                        aoVivo.Data = data.ToString("dd/MM/yyyy HH:mm:ss");
                        aoVivo.Comentario = retornoDataReader["comentario"].ToString().Trim();
                        listaComentarios.Add(aoVivo);
                    }
                    return listaComentarios;
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaRepositorioADO::PartidaAoVivo:. Erro ao PartidaAoVivo: " + ex.Message, TratamentoLog.NivelLog.Erro);
                return null;
            }
        }
    }

    
}

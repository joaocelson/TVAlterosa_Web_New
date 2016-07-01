using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.Dominio.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.RepositorioADO
{
    public class BolaoRepositorioADO : IRepositorio<Bolao>
    {
        private Contexto contexto;

        private void Inserir(Bolao bolao)
        {
            try
            {
                var strQuery = "";
                strQuery += " INSERT INTO t_bolao (id_partida, id_usuario, gol_time_mandante, gol_time_visitante, pontos_adquiridos) ";
                strQuery += string.Format(" VALUES ({0},{1},{2},{3},{4})",
                    bolao.Partida.Id, bolao.Usuario.Id, bolao.GolMandante, bolao.GolVisitante, 0
                    );
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("BolaoRepositoerioADO::Inserir:. Erro ao salvar bolão. " + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        private void Alterar(Bolao bolao)
        {
            var strQuery = "";
            strQuery += " UPDATE t_bolao SET ";
            strQuery += string.Format(" gol_time_mandante = '{0}', ", bolao.GolMandante);
            strQuery += string.Format(" gol_time_visitante = '{0}' ", bolao.GolVisitante);
            strQuery += string.Format(" WHERE id_usuario = {0} ", bolao.Usuario.Id);
            strQuery += string.Format(" AND id_partida = {0} ", bolao.Partida.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }



        public void Salvar(Bolao bolao)
        {
            Bolao bolaoNovo = ListarTodosPorUsuarioPartida(bolao.Usuario.Id, bolao.Partida.Id);

            if (bolaoNovo != null)
                Alterar(bolao);
            else
                Inserir(bolao);
        }

        public void Excluir(Bolao bolao)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM t_bolao WHERE Id = {0}", bolao.Id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public IEnumerable<Bolao> ListarTodosPorUsuario(string idUsuario)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_bolao where id_usuario = {0}", idUsuario);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Bolao ListarPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_bolao WHERE Id = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        public IEnumerable<Bolao> ListarPorPartida(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_bolao WHERE Id_partida = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public IEnumerable<Bolao> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_bolao ");
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public IEnumerable<Bolao> ListarBoloes()
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM T_BOLAO_CAMPEONATO");

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);

                List<Bolao> listaBoloes = new List<Bolao>();


                while (retornoDataReader.Read())
                {
                    Bolao bolao = new Bolao();
                    bolao.Id = Convert.ToInt16(retornoDataReader["id"].ToString());
                    bolao.Nome = retornoDataReader["nome"].ToString();
                    listaBoloes.Add(bolao);
                }

                return listaBoloes;
            }
        }

        public Bolao ListarBolaoPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM T_BOLAO_CAMPEONATO where id = {0}", id);

                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);

                Bolao bolao = new Bolao();

                while (retornoDataReader.Read())
                {
                    bolao.Id = Convert.ToInt16(retornoDataReader["id"].ToString());
                    bolao.Nome = retornoDataReader["nome"].ToString();
                }

                return bolao;
            }
        }

        private List<Bolao> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var bolao = new List<Bolao>();
            while (reader.Read())
            {
                PartidaRepositorioADO partida = new PartidaRepositorioADO();
                UsuarioRepositorioADO usuario = new UsuarioRepositorioADO();
                var temObjeto = new Bolao()
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    // IdBolaoCampeonato = int.Parse(reader["id_bolao_campeonato"].ToString()),
                    GolMandante = Convert.ToInt16(reader["gol_time_mandante"].ToString()),
                    GolVisitante = Convert.ToInt16(reader["gol_time_visitante"].ToString()),
                    Partida = partida.ListarPorId(reader["id_partida"].ToString()),
                    Usuario = usuario.ListarPorId(reader["id_usuario"].ToString()),
                    PontosAdquiridos = Convert.ToInt16(reader["pontos_adquiridos"].ToString())

                };
                bolao.Add(temObjeto);
            }
            reader.Close();
            return bolao;
        }

        public Bolao ListarTodosPorUsuarioPartida(int idUsuario, int idPartida)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_bolao where id_usuario = {0} AND id_partida = {1}", idUsuario, idPartida);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        public IEnumerable<Bolao> ListarClassificacaoPorBolao(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT * FROM t_bolao where id_usuario = {0} AND id_partida = {1}", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Boolean PontuarBolao(IEnumerable<Partida> listaPartidas)
        {
            try
            {
                //Lista de partidas, são as partidas com resultados reais
                foreach (Partida partida in listaPartidas)
                {
                    //Jogos boloes são as partidas palpitadas
                    IEnumerable<Bolao> jogosBoloes = ListarPorPartida(partida.Id);

                    foreach (Bolao bolao in jogosBoloes)
                    {
                        if (!partida.GolMandante.Equals("") || !partida.GolVisitante.Equals(""))
                        {
                            if ((Convert.ToInt16(partida.GolMandante) > Convert.ToInt16(partida.GolVisitante)) && (bolao.GolMandante > bolao.GolVisitante))
                            {
                                bolao.PontosAdquiridos = 1;

                                if (bolao.GolMandante == Convert.ToInt16(partida.GolMandante) && bolao.GolVisitante == Convert.ToInt16(partida.GolVisitante))
                                {
                                    if (Convert.ToInt16(partida.GolMandante) >= 3 && bolao.GolMandante >= 3)
                                    {
                                        bolao.PontosAdquiridos = bolao.PontosAdquiridos + 2;
                                    }
                                    else
                                    {
                                        bolao.PontosAdquiridos = bolao.PontosAdquiridos + 1;
                                    }
                                }

                            }
                            else if ((Convert.ToInt16(partida.GolMandante) < Convert.ToInt16(partida.GolVisitante)) && (bolao.GolMandante < bolao.GolVisitante))
                            {
                                bolao.PontosAdquiridos = 1;
                                if (bolao.GolMandante == Convert.ToInt16(partida.GolMandante) && bolao.GolVisitante == Convert.ToInt16(partida.GolVisitante))
                                {
                                    if (Convert.ToInt16(partida.GolVisitante) >= 3 && bolao.GolVisitante >= 3)
                                    {
                                        bolao.PontosAdquiridos = bolao.PontosAdquiridos + 2;
                                    }
                                    else
                                    {
                                        bolao.PontosAdquiridos = bolao.PontosAdquiridos + 1;
                                    }
                                }
                            }
                            else if (Convert.ToInt16(partida.GolMandante) == Convert.ToInt16(partida.GolVisitante) && (bolao.GolMandante == bolao.GolVisitante))
                            {
                                bolao.PontosAdquiridos = 1;
                                if (bolao.GolMandante == Convert.ToInt16(partida.GolMandante) && bolao.GolVisitante == Convert.ToInt16(partida.GolVisitante))
                                {
                                    if (Convert.ToInt16(partida.GolVisitante) >= 3 && bolao.GolVisitante >= 3)
                                    {
                                        bolao.PontosAdquiridos = bolao.PontosAdquiridos + 2;
                                    }
                                    else
                                    {
                                        bolao.PontosAdquiridos = bolao.PontosAdquiridos + 1;
                                    }
                                }
                            }

                            var strQuery = string.Format("UPDATE T_BOLAO SET pontos_adquiridos = {0} WHERE id = {1}", bolao.PontosAdquiridos, bolao.Id);
                            using (contexto = new Contexto())
                            {

                                contexto.ExecutaComando(strQuery);
                                TratamentoLog.GravarLog("BolaRepositorioADO::PontuarBolao. Atualizado bolão para o T_BOLA.ID", TratamentoLog.NivelLog.Info);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {

                TratamentoLog.GravarLog("BolaRepositorioADO::PontuarBolao:. Erro ao pontuar bolão. " + ex.Message, TratamentoLog.NivelLog.Erro);
                return false;
            }
        }





        public List<string[]> VencedorRodada(string id)
        {
            var listaUsuarios = new List<string[]>();
            using (contexto = new Contexto())
            {
                var strQuery = string.Format("SELECT  POSICAO = Rank() Over(Order by SUM(PONTOS_ADQUIRIDOS) Desc), U.NOME_USUARIO USUARIO, SUM(PONTOS_ADQUIRIDOS) AS PONTOS ");
                strQuery += string.Format("FROM T_BOLAO AS B INNER JOIN ");
                strQuery += string.Format("USUARIO AS U ON (U.ID_USUARIO = B.ID_USUARIO) INNER JOIN ");
                strQuery += string.Format("PARTIDA AS P ON (P.ID = B.ID_PARTIDA) INNER JOIN ");
                strQuery += string.Format("CAMPEONATO C ON (C.ID = P.ID_CAMPEONATO) ");
                strQuery += string.Format("WHERE C.ID_BOLAO = {0} ", id);
                strQuery += string.Format("GROUP BY U.NOME_USUARIO ORDER BY PONTOS DESC");
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);

                CampeonatoRepositorioADO camp = new CampeonatoRepositorioADO();

                Bolao bolao = ListarBolaoPorId(id);
                string[] usuario = new string[3];
                usuario[0] = "POSIÇÃO";
                usuario[1] = bolao.Nome.ToUpper();
                usuario[2] = "PONTOS";
                listaUsuarios.Add(usuario);
                while (retornoDataReader.Read())
                {
                    usuario = new string[3];
                    usuario[0] = retornoDataReader["POSICAO"].ToString().ToUpper();
                    usuario[1] = retornoDataReader["USUARIO"].ToString().ToUpper();
                    usuario[2] = retornoDataReader["PONTOS"].ToString().ToUpper();

                    listaUsuarios.Add(usuario);
                }

            }
            return listaUsuarios;
        }
    }
}

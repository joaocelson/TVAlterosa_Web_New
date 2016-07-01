using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MRS.Automacao.Uteis
{
    public class TratamentoLog
    {
        public enum NivelLog { Info, Erro, Acao };

        private static String pathPastaLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

        private static String NiveisLogAtual = "Info|Erro|Acao";

        /// <summary>
        /// Altera o path da pasta de logs atualmente sendo utilizada.
        /// </summary>
        /// <param name="Pasta">Novo caminho para a pasta de logs.</param>
        public static void AlterarPastaLog(String PathPasta) { pathPastaLog = PathPasta; }

        /// <summary>
        /// Obtém a pasta de logs atualmente sendo utilizada.
        /// </summary>
        /// <returns>Path para a pasta de logs</returns>
        public static String ObterPastaLog() { return pathPastaLog; }

        /// <summary>
        /// Altera o nível dos logs gravados. Os níveis devem ser separados por vírgula ou por pipe ("|").
        /// Ex: "Erro|Acao" ou "Erro, Acao"
        /// </summary>
        /// <param name="Niveis">Níveis a serem gravados, separados por vírgula ou por pipe ("|").</param>
        public static void AlterarNiveisGravacaoLogs(String Niveis)
        {
            NiveisLogAtual = "";

            char charSeparacao = (Niveis.IndexOf(",") >= 0) ? ',' : '|';
            String[] arrNiveis = Niveis.Split(charSeparacao);
            foreach (String Nivel in arrNiveis) NiveisLogAtual += (NiveisLogAtual == "" ? "" : "|") + Nivel.Trim();
        }

        /// <summary>
        /// Verifica se um nível específico de log está sendo gravado.
        /// </summary>
        /// <param name="Nivel">Nível que se deseja verificar se está sendo gravado.</param>
        /// <returns>Verdadeiro quando estiver sendo gravado o nível especificado, Falso caso contrário.</returns>
        public static Boolean EstaGravandoNivel(NivelLog Nivel)
        {
            return new List<String>(NiveisLogAtual.Split('|')).Contains(Enum.GetName(typeof(NivelLog), Nivel));
        }

        /// <summary>
        /// Grava a mensagem de log desejada. O nível do log gravado será de Erro.
        /// </summary>
        /// <param name="Mensagem">Mensagem de erro a ser gravada.</param>
        public static void GravarLog(String Mensagem)
        {
            GravarLog(Mensagem, NivelLog.Erro, "", "");
        }

        /// <summary>
        /// Grava a mensagem de log desejada, no nível desejado.
        /// Caso o nível desejado não esteja ativo no momento, nada será gravado.
        /// </summary>
        /// <param name="Mensagem">Mensagem de log a ser gravada.</param>
        /// <param name="Nivel">Nível de log da mensagem sendo gravada.</param>
        public static void GravarLog(String Mensagem, NivelLog Nivel)
        {
            GravarLog(Mensagem, Nivel, "", "");
        }

        /// <summary>
        /// Grava a mensagem de log desejada, no nível desejado.
        /// Caso o nível desejado não esteja ativo no momento, nada será gravado.
        /// </summary>
        /// <param name="Mensagem">Mensagem de log a ser gravada.</param>
        /// <param name="Nivel">Nível de log da mensagem sendo gravada.</param>
        /// <param name="SubPasta">Subpasta (opcional) na qual se deseja gravar o log.</param>
        /// <param name="NomeArquivo">Nome do arquivo (opcional) para o qual o log será gravado. Por padrão, o nome é a data, no formato yyyyddmm. A extensão sempre é .txt.</param>
        public static void GravarLog(String Mensagem, NivelLog Nivel, String SubPasta, String NomeArquivo)
        {
            try
            {
                if (EstaGravandoNivel(Nivel))
                {
                    String path = pathPastaLog;
                    String nome = DateTime.Now.ToString("yyyyMMdd");
                    if (SubPasta != "") path = Path.Combine(pathPastaLog, SubPasta);
                    if (NomeArquivo != "") nome = NomeArquivo;

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    File.AppendAllText(Path.Combine(path, nome + ".txt"), DateTime.Now.ToString("HH:mm:ss") + " - " + Enum.GetName(typeof(NivelLog), Nivel) + " - " + Mensagem + "\r\n");
                }
            }
            catch { }
        }
    }
}

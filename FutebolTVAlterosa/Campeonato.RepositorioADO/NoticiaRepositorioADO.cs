using Campeonato.Dominio.contrato;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Campeonato.Dominio.util;
using System.Data;

namespace Campeonato.RepositorioADO
{
    public class NoticiaRepositorioADO : IRepositorio<Noticia>
    {
        private Contexto contexto;

        private void Inserir(Noticia noticia)
        {
            var strQuery = "";
            strQuery += " INSERT INTO noticia (titulo, noticia, data_noticia, id_usuario, id_time) ";
            strQuery += string.Format(" VALUES ('{0}','{1}','{2}',{3},{4})", noticia.Titulo, noticia.Corpo, DateTime.Now, noticia.Usuario.Id, (noticia.Time != null && noticia.Time.Id != null) ? noticia.Time.Id : System.Data.SqlTypes.SqlInt32.Null);
            try
            {
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }

        private void Alterar(Noticia noticia)
        {
            var strQuery = "";
            strQuery += " UPDATE noticia SET ";
            strQuery += string.Format(" titulo = '{0}', ", noticia.Titulo);
            strQuery += string.Format(" noticia = '{0}', ", noticia.Corpo);
            strQuery += string.Format(" data_noticia = '{0}', ", noticia.DataNoticia);
            strQuery += string.Format(" id_usuario = '{0}', ", noticia.Usuario.Id);
            strQuery += string.Format(" id_time = '{0}', ", noticia.Time.Id);
            strQuery += string.Format(" WHERE Id = {0} ", noticia.Id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void Salvar(Noticia noticia)
        {
            if (noticia.Id > 0)
                Alterar(noticia);
            else
                Inserir(noticia);
        }

        public void Excluir(Noticia noticia)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" DELETE FROM noticia WHERE Id = {0}", noticia.Id);
                contexto.ExecutaComando(strQuery);
            }
        }

        public IEnumerable<Noticia> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM noticia ";
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader);
            }
        }

        public Noticia ListarPorId(string id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM noticia WHERE Id = {0} ", id);
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
            }
        }

        private List<Noticia> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            var noticia = new List<Noticia>();
            UsuarioRepositorioADO usuario = new UsuarioRepositorioADO();
            TimeRepositorioADO time = new TimeRepositorioADO();

            while (reader.Read())
            {
                var temObjeto = new Noticia()
                {
                    Id = Convert.ToInt32(reader["id"].ToString()),
                    Titulo = reader["titulo"].ToString(),
                    Corpo = reader["noticia"].ToString(),
                    Usuario = usuario.ListarPorId(reader["id_usuario"].ToString()),
                    DataNoticia = DateTime.Parse(reader["data_noticia"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")

                };
                if (!reader["id_time"].ToString().Equals(""))
                {
                    temObjeto.Time = time.ListarPorId(reader["id_time"].ToString());
                }
                noticia.Add(temObjeto);
            }
            reader.Close();
            return noticia;
        }
    }
}

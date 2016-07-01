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
    public class UsuarioRepositorioADO : IRepositorio<Usuario>
    {
        private Contexto contexto;

        private void Inserir(Usuario usuario)
        {
            try
            {
                var strQuery = "";
                strQuery += " INSERT INTO usuario (nome_usuario, senha_usuario, email_usuario, tipo_usuario, token) ";
                strQuery += string.Format(" VALUES ('{0}','{1}','{2}',{3},'{4}') ",
                    usuario.NomeUsuario, usuario.Senha, usuario.LoginEmail, usuario.TipoUsuario, usuario.Token
                    );
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Alterar(Usuario usuario)
        {
            try
            {
                var strQuery = "";
                strQuery += " UPDATE usuario SET ";
                strQuery += string.Format(" nome_usuario = '{0}', ", usuario.NomeUsuario);
                strQuery += string.Format(" email_usuario  = '{0}', ", usuario.LoginEmail);
                strQuery += string.Format(" senha_usuario = '{0}' ", usuario.Senha);
                strQuery += string.Format(" WHERE Id = {0} ", usuario.Id);
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void Salvar(Usuario usuario)
        {
            if (usuario.Id > 0)
                Alterar(usuario);
            else
                Inserir(usuario);
        }

        public void Excluir(Usuario usuario)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format(" DELETE FROM usuario WHERE Id = '{0}'", usuario.Id);
                    contexto.ExecutaComando(strQuery);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public IEnumerable<Usuario> ListarTodos()
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = " SELECT * FROM usuario";
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public Usuario ListarPorId(string id)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format(" SELECT * FROM usuario WHERE id_usuario = '{0}' ", id);
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public Usuario ValidarUsuario(Usuario usuario)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format(" SELECT * FROM usuario WHERE email_usuario = '{0}' ", usuario.LoginEmail);
                    strQuery += string.Format(" and  senha_usuario = '{0}' ", usuario.Senha);
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    if (retornoDataReader.FieldCount > 0)
                    {

                        return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }

        }

        private List<Usuario> TransformaReaderEmListaDeObjeto(SqlDataReader reader)
        {
            try
            {
                List<Usuario> Usuario = new List<Usuario>();
                while (reader.Read())
                {
                    var temObjeto = new Usuario()
                    {
                        Id = int.Parse(reader["id_usuario"].ToString()),
                        NomeUsuario = reader["nome_usuario"].ToString(),
                        Senha = reader["senha_usuario"].ToString(),
                        LoginEmail = reader["email_usuario"].ToString(),
                        TipoUsuario = Convert.ToInt16(reader["tipo_usuario"].ToString())

                    };
                    Usuario.Add(temObjeto);
                }
                reader.Close();
                return Usuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public IEnumerable<Usuario> ListarUsuariosCampeonato(string idCampeonato)
        //{
        //    using (contexto = new Contexto())
        //    {
        //        var strQuery = string.Format(" SELECT * ");
        //        strQuery += string.Format("FROM Usuarios t INNER JOIN ");
        //        strQuery += string.Format("Usuario_campeonato c on c.id_Usuario = t.id ");
        //        strQuery += string.Format("WHERE c.id_campeonato = {0}  ", 1);
        //        var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
        //        return TransformaReaderEmListaDeObjeto(retornoDataReader);
        //    }
        //}

        public Usuario ValidarUsuarioEmail(Usuario usuario)
        {
            try
            {
                using (contexto = new Contexto())
                {
                    var strQuery = string.Format(" SELECT * FROM usuario WHERE email_usuario = '{0}' ", usuario.LoginEmail);
                    var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    if (retornoDataReader.FieldCount > 0)
                    {

                        return TransformaReaderEmListaDeObjeto(retornoDataReader).FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}

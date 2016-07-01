using Campeonato.Aplicacao;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Campeonato.UI.WEB.Security
{
    public class PermissaoProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            Usuario usuario = new Usuario();
            UsuarioAplicacao appUsuario = UsuarioAplicacaoConstrutor.UsuarioAplicacaoADO();
            usuario.LoginEmail = username;
            usuario = appUsuario.ValidarUsuarioEmail(usuario);
            string[] permissoes = null;
            if (usuario != null)
            {
                if (usuario.TipoUsuario == 1)
                {
                    permissoes = new string[] { "Admin" };
                }
                else
                {
                    permissoes = new string[] { "Visitante" };
                }

                return permissoes.ToArray();
            }
            else
            {
                return null;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using CORE.Auth.Mapeamentos;

namespace CORE.Auth.Servicos
{
    public static class NetlogOAuth
    {
        public static void Login(LogInModel loginModel, HttpRequestBase request, List<string> sistemas)
        {
            try
            {
                //var searchResult = AutenticateActiveDirectory(loginModel);

                //List<string> listaGrupos = GetGruposActiveDirectory(searchResult);

                //if (listaGrupos == null || listaGrupos.Count <= 0)
                //{
                //    throw new Exception("Usuario não possui acesso ao sistema");
                //}

                var identity = new ClaimsIdentity("ApplicationCookie");
                //identity.AddClaim(new Claim(ClaimTypes.Name, GetNomeCompletoUsuario(searchResult)));
                //identity.AddClaim(new Claim(ClaimTypes.Sid, loginModel.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, loginModel.UserName.ToUpper()));
                identity.AddClaim(new Claim(ClaimTypes.Sid, loginModel.UserName.ToUpper()));
                //foreach (string grupo in listaGrupos)
                //{
                //    identity.AddClaim(new Claim(ClaimTypes.Role, grupo));
                //}                

                var ctx = request.GetOwinContext();
                var authManager = ctx.Authentication;

                //indica que o o cookie expira junto com a sessão
                authManager.SignIn(new AuthenticationProperties() { IsPersistent = loginModel.RememberMe }, identity);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o login. " + ex.Message, ex);
            }
        }
       

        /// <summary>
        /// Método responsável por realizar o logout no sistema
        /// </summary>
        /// <param name="request">Request Base</param>
        public static void LogOut(HttpRequestBase request)
        {
            var ctx = request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
        }
   }
}

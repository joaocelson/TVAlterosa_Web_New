using Campeonato.Aplicacao;
using Campeonato.Dominio;
using Campeonato.UI.WEB.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Campeonato.UI.WEB.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        //
        // GET: /Admin/AdminHome/

        UsuarioAplicacao appUsuario;
        public AdminHomeController()
        {
            appUsuario = UsuarioAplicacaoConstrutor.UsuarioAplicacaoADO();
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Logado(Usuario usuario)
        {
            return View(usuario);
        }

        [PermissoesFiltro(Roles = "Visitante, Admin")]
        public ActionResult LogadoVisitante(Usuario usuario)
        {
            if (usuario.LoginEmail == null)
            {
                usuario.LoginEmail = ControllerContext.HttpContext.User.Identity.Name;
                Usuario usu = new Usuario();
                usu = appUsuario.ValidarUsuarioEmail(usuario);

                if (usu.TipoUsuario == 1)
                {
                    return RedirectToAction("Logado");
                }
                else
                {
                    return View(usuario);
                }
            }

            return View(usuario);
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            Usuario usuarioValidado = appUsuario.ValidarUsuario(usuario);
            if (usuarioValidado != null)
            {
                if (usuarioValidado.TipoUsuario == 1)
                {
                    FormsAuthentication.SetAuthCookie(usuarioValidado.LoginEmail, false);
                    FormsAuthentication.RedirectFromLoginPage(usuarioValidado.LoginEmail, false);
                    return RedirectToAction("Logado", new RouteValueDictionary(
                        new { controller = "AdminHome", action = "Login" }));
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(usuarioValidado.LoginEmail, false);
                    FormsAuthentication.RedirectFromLoginPage(usuarioValidado.LoginEmail, false);
                    return RedirectToAction("LogadoVisitante");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login ou senha incorretos. Tente novamente.");
                return View();
            }
        }

        public ActionResult LoginJSON()
        {
            return View();
        }


        //[HttpPost]
        //public ActionResult LoginJSON(Usuario usuario)
        //{
        //    Usuario usuarioValidado = appUsuario.ValidarUsuario(usuario);
        //    if (usuarioValidado != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(usuarioValidado.LoginEmail, false);
        //        FormsAuthentication.RedirectFromLoginPage(usuarioValidado.LoginEmail, false);

        //        System.Web.Script.Serialization.JavaScriptSerializer jSearializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        string jsonString = jSearializer.Serialize(usuario);
        //        return Json(jsonString, JsonRequestBehavior.AllowGet);

        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logoff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        //METODOS JSON
        //===========================================
        //
        // POST: /Account/Login
        [HttpPost]
        public String LoginJson(Usuario usuario)
        {
            var usa = appUsuario.ValidarUsuario(usuario);
            if (usa != null)
            {
                return JsonConvert.SerializeObject(usa, Formatting.Indented);
            }
            else
            {
                return "Erro";
            }
        }
    }
}

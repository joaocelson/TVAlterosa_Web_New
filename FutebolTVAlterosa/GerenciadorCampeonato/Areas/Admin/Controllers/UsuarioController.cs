using Campeonato.Aplicacao;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Campeonato.UI.WEB.Areas.Admin.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioAplicacao appUsuario;

        public UsuarioController()
        {
            appUsuario = UsuarioAplicacaoConstrutor.UsuarioAplicacaoADO();
        }

        //
        // GET: /Admin/Usuario/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Usuario usuario)
        {
            if (appUsuario.ValidarUsuarioEmail(usuario) == null)
            {

                if (ModelState.IsValid)
                {
                    usuario.TipoUsuario = 0;
                    appUsuario.Salvar(usuario);

                    return RedirectToAction("Login", new RouteValueDictionary(
                        new { controller = "AdminHome", action = "Login" }));
                } return View(usuario);
            }
            else
            {
                ModelState.AddModelError("", "Login já cadastrado, tente outro login");
                return View(usuario);
            }

        }

        //METODOS JSON
        //========================================
        [HttpPost]
        public String CadastrarJson(Usuario usuario)
        {
            //TIPO USUARIO = 0 SERÁ USUÁRIO VISITANTE

            //TIPO USUARIO = 1 SERÁ USUÁRIO ADMINISTRADOR

            //TIPO USUARIO = 2 SERÁ USUÁRIO ADMINISTRADOR_TIME

            //TIPO USUARIO = 2 SERÁ USUÁRIO ADMINISTRADOR SEM VINCULO TIME

            try
            {
                if (appUsuario.ValidarUsuarioEmail(usuario) == null)
                {
                    usuario.TipoUsuario = 0;
                    appUsuario.Salvar(usuario);

                    return "OK";
                }
                else
                {
                    ModelState.AddModelError("", "Login já cadastrado, tente outro login");
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "erro";
            }
        }

    }
}
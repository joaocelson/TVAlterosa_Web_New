using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Web.Mvc.Html;
using System.Web;


namespace CORE.Componentes
{
    /// <summary>
    ///     Cria Extensoes do HtmlHelper para o KendoUI
    /// </summary>
    public static class HtmlRazorExtensao
    {

        #region Extensoes

        /// <summary>
        ///     Extensao do MVC HtmlHelper para criacao do componente Grid
        /// </summary>
        /// <typeparam name="T">Tipo utilizado pelo grid</typeparam>
        /// <param name="html">HtmlHelper extendido</param>
        /// <param name="nome">Nome do grid</param>
        /// <returns>Html do grid KendoUI</returns>
        /// <example>
        /// 
        /// @(Html.KendoGrid<TACModel.TipoRequisicaoAcidente>("tipoReqGrid2")
        ///             .ToolbarLimpar("gridAllFilterReset")
        ///             .Columns(columns =>
        ///            {
        ///                columns.Bound(p => p.cdTipoRequisicaoAcidente).Visible(true).Width(110);
        ///                columns.Bound(p => p.dcTipoRequisicao);
        ///                columns.Bound(p => p.nuOrdem).Width(110);
        ///                columns.Bound(p => p.isAtivo).VisualizarCheckBox("cdTipoRequisicaoAcidente");
        ///                columns.Bound(p => p.cdUsuaAlt).Visible(true).Width(160);
        ///                columns.Bound(p => p.dtAlt).Visible(true).Width(150);
        ///            })
        ///            .DataSource(dataSource => dataSource
        ///            .Ajax()
        ///            .Sort(sort => sort.Add(p => p.cdTipoRequisicaoAcidente).Ascending())
        ///            .Events(events => events.Error("errorHandler"))
        ///            .Model(model =>
        ///            {
        ///                model.Id(p => p.cdTipoRequisicaoAcidente);
        ///                model.Field(p => p.cdTipoRequisicaoAcidente).Editable(false);
        ///            })
        ///            .Create(update => update.Action("Create", "TipoRequisicaoAcidente"))
        ///                        .Read(read => read.Action("Read", "TipoRequisicaoAcidente"))
        ///                        .Update(update => update.Action("Update", "TipoRequisicaoAcidente"))
        ///                        .Destroy(update => update.Action("Destroy", "TipoRequisicaoAcidente"))
        ///            )
        ///     )
        /// 
        /// </example>
        public static KendoGridMvc<T> KendoGrid<T>(this HtmlHelper html, string nome, bool readOnly = false) where T : class
        {
            return new KendoGridMvc<T>(html.Kendo().Grid<T>().Name(nome), readOnly);
        }


        /// <summary>
        ///     Extensao do GridBound para criação do checkbox para visualização de campos true/false
        /// </summary>
        /// <typeparam name="TModel">Tipo utilizado pela coluna</typeparam>
        /// <param name="builder">Objeto extendido</param>
        /// <param name="id">Nome do objeto de checkbox</param>
        /// <returns>GridBound de retorno</returns>
        public static GridBoundColumnBuilder<TModel> VisualizarCheckBox<TModel>(this GridBoundColumnBuilder<TModel> builder, string id, string campoAtivo = "IsAtivo") where TModel : class, new()
        {
            var template =
                "<input name='IsAtivo#= " + id + "#>' id='IsAtivo#= " + id + "#' type='checkbox' #= " + campoAtivo + " ? checked='checked' : '' # onclick='return false;' />";

            return builder.HtmlAttributes(new { style = "text-align: center;" }).ClientTemplate(template); ;
        }

        /// <summary>
        ///     ActionLink com a propriedade de validação de segurança
        /// </summary>
        /// <param name="htmlHelper">Tipo do helper MVC</param>
        /// <param name="securityController">True para validar e False para não validar</param>
        /// <param name="linkText">Nome que será apresentado no link</param>
        /// <param name="actionName">Nome da action que será executada</param>
        /// <param name="controllerName">Nome do controlador que será utilizado para executar a action</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, bool securityController, string linkText, string actionName, string controllerName)
        {

            if (securityController)
            {
                if (!SecurityController(htmlHelper.ViewContext.HttpContext, actionName, controllerName))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="successMessage"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static MvcHtmlString MessageBoxes(this HtmlHelper htmlHelper, object successMessage, object errorMessage)
        {
            string successMessageStyle = successMessage == null || successMessage.ToString() == "" ? "display:none" : "";
            string successMessageHandled = successMessage == null || successMessage.ToString() == "" ? "" : HttpUtility.HtmlDecode(successMessage.ToString());
            string html = "<div id='successBox' class='alert alert-success' style='" + successMessageStyle + ";'><strong>Sucesso:</strong><div id='successMessage'>" + successMessageHandled + "</div></div>";
            string errorMessageStyle = errorMessage == null || errorMessage.ToString() == "" ? "display:none" : "";
            string errorMessageHandled = errorMessage == null || errorMessage.ToString() == "" ? "" : HttpUtility.HtmlDecode(errorMessage.ToString());
            html += "<div id='errorBox' class='alert alert-danger' style='" + errorMessageStyle + ";'><strong>Erro:</strong><div id='errorMessage'>" + errorMessageHandled + "</div></div>";            

            return new MvcHtmlString(html);
        }

        #endregion

        #region Metodos privados

        /// <summary>
        ///     Metodo utilizado para validacao das rules de acordo com o action e o controller no contexto do usuario
        /// </summary>
        /// <param name="httpContextBase">Contexto do Helper MVC</param>
        /// <param name="actionName">Nome da action</param>
        /// <param name="controllerName">Controlador da action que será executada</param>
        /// <returns></returns>
        private static bool SecurityController(System.Web.HttpContextBase httpContextBase, string actionName, string controllerName)
        {
            var usuario = httpContextBase.User;

            var contextTemporario = new RequestContext(httpContextBase, new RouteData());
            contextTemporario.RouteData.DataTokens["Area"] = "";
            contextTemporario.RouteData.DataTokens["Namespaces"] = "SGA.Web.Controllers";

            var controlador = ControllerBuilder.Current.GetControllerFactory()
                            .CreateController(contextTemporario, controllerName);

            if (controlador == null)
                return false;

            var classe = controlador.GetType();

            var metodo = classe.GetMethod(actionName);
            if (metodo == null)
                return false;

            var attrs = Attribute.GetCustomAttributes(metodo);
            if (
                attrs.OfType<AuthorizeAttribute>()
                    .Any(
                        a =>
                            a.Roles.Select(rule => rule.ToString(CultureInfo.InvariantCulture))
                                .Any(regra => !usuario.IsInRole(regra))))
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
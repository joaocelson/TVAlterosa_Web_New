using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Web.Routing;
using CORE.Componentes.Personalizados.AutoComplete.Interfaces;
using CORE.Componentes.Personalizados.Timeline.Models;
using CORE.Componentes.Personalizados.Timeline.Helpers;
using CORE.Componentes.Personalizados.AutoComplete.Helpers;

namespace CORE.Componentes.Personalizados.Extensions
{
    /// <summary>
    /// Extensão do HtmlHelper, objeto responsável por renderizar o html na view
    /// </summary>
    public static class HtmlExtension
    {
        /// <summary>
        /// Método responsável por renderizar um componente auto-complete da telerik fortemente tipado na view
        /// </summary>
        /// <typeparam name="T">Provider de informação sobre a entidade tipada para o componente</typeparam>
        /// <param name="helper">Html Helper</param>
        /// <param name="autoCompleteProvider">Instância do objeto que contém definições do comportamento do componente</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString RenderAutoComplete<T>(this HtmlHelper helper, T autoCompleteProvider) where T : IAutoCompleteProvider
        {
            //criação do compoente de auto-complete
            AutoCompleteBuilder autoComplete = helper.Kendo().AutoComplete();

            //configuração do campo que será exibido na seleção do auto-complete
            autoComplete.DataTextField(autoCompleteProvider.DataTextField);

            autoComplete.Name(autoCompleteProvider.Name);
            autoComplete.Filter(FilterType.StartsWith);
            autoComplete.MinLength(autoCompleteProvider.MinLength);

            if (autoCompleteProvider.HtmlAttributes != null)
                autoComplete.HtmlAttributes(autoCompleteProvider.HtmlAttributes);

            //configuração dos handlers de eventos javascript
            if (!String.IsNullOrEmpty(autoCompleteProvider.OnChange))
                autoComplete.Events(i => i.Change(autoCompleteProvider.OnChange));

            if (!String.IsNullOrEmpty(autoCompleteProvider.OnClose))
                autoComplete.Events(i => i.Close(autoCompleteProvider.OnClose));

            if (!String.IsNullOrEmpty(autoCompleteProvider.OnDataBound))
                autoComplete.Events(i => i.DataBound(autoCompleteProvider.OnDataBound));

            if (!String.IsNullOrEmpty(autoCompleteProvider.OnSelect))
                autoComplete.Events(i => i.Select(autoCompleteProvider.OnSelect));

            if (!String.IsNullOrEmpty(autoCompleteProvider.OnOpen))
                autoComplete.Events(i => i.Open(autoCompleteProvider.OnOpen));

            //configuração do template do componente
            if (!string.IsNullOrEmpty(autoCompleteProvider.Template))
            {
                string template = ResourcesHelper.LoadResourceAsString(autoCompleteProvider.Template);
                autoComplete.Template(template);
            }

            if (!string.IsNullOrEmpty(autoCompleteProvider.HeaderTemplate))
            {
                string headerTemplate = ResourcesHelper.LoadResourceAsString(autoCompleteProvider.HeaderTemplate);
                autoComplete.Template(headerTemplate);
            }

            //configuração do end-point (método/controller) de acesso do componente
            autoComplete.DataSource(source =>
              {
                  source.Read(read =>
                  {
                      read.Action(autoCompleteProvider.ActionName, autoCompleteProvider.ControllerName);

                      //handler do evento javascript que envia informações extras para o server
                      if (!String.IsNullOrEmpty(autoCompleteProvider.OnSendData))
                          read.Data(autoCompleteProvider.OnSendData);

                  }).ServerFiltering(true);
              });

            return MvcHtmlString.Create(autoComplete.ToHtmlString());
        }

        /// <summary>
        /// Método responsável por renderizar um link contendo html extra dentro da tag
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="text">Texto exibido na tag do componente</param>
        /// <param name="innerHtml">Html extra a ser inserido entre tags</param>
        /// <param name="action">Método que responderá pelo evendo de click</param>
        /// <param name="controller">Controller que responderá pelo evendo de click</param>
        /// <param name="routeValues">Valores relacionados a rota</param>
        /// <param name="htmlAttributes">Atributos html extra</param>
        /// <returns></returns>
        public static IHtmlString ActionLinkWithInnerHtml(this HtmlHelper htmlHelper, string text, string innerHtml, string action, string controller, object routeValues, object htmlAttributes)
        {
            TagBuilder tagBuilder;
            UrlHelper urlHelper;

            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            tagBuilder = new TagBuilder("a");

            tagBuilder.InnerHtml = innerHtml + "&nbsp;&nbsp;" + text;

            tagBuilder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);

            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tagBuilder.ToString());
        }


        /// <summary>
        /// Método responsável por renderizar uma timeline na view a partir de um IEnumerable<TimelineItem>
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="itens">Itens a serem renderizados na timeline</param>
        /// <returns></returns>
        public static MvcHtmlString RenderTimeline(this HtmlHelper htmlHelper, IEnumerable<TimelineItem> itens)
        {
            //criação do nó principal da timeline
            var unorderedListTimeline = new TagBuilder("ul");
            unorderedListTimeline.Attributes.Add(new KeyValuePair<string, string>("class", "timeline"));

            //iteração dos itens para construção da timeline
            foreach (var timelineItem in itens)
            {
                //criação do item da timeline, setando sua cor configurada
                var timelineColor = new TagBuilder("li");
                timelineColor.Attributes.Add(new KeyValuePair<string, string>("class", TimelineHelper.GetCssByTimelineColor(timelineItem.Color)));

                //criação do nó que conterá as informações sobre a data e hora do evento na timeline
                var timelineTime = new TagBuilder("div");
                timelineTime.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-time"));

                //criação do nó que conterá a data
                var spanDate = new TagBuilder("span");
                spanDate.Attributes.Add(new KeyValuePair<string, string>("class", "date"));
                //spanDate.InnerHtml = timelineItem.DateTime.ToString("dd/MM/yyyy");
                spanDate.InnerHtml = timelineItem.MarcacaoPatioAtual;

                //criação do nó que conterá a hora
                var spanTime = new TagBuilder("span");
                spanTime.Attributes.Add(new KeyValuePair<string, string>("class", "time"));
                //spanTime.InnerHtml = timelineItem.DateTime.ToString("HH:mm");
                spanTime.InnerHtml = timelineItem.SiglaPatio;

                //atribuição do nó de data e de hora ao nó principal relativo a essa informação
                timelineTime.InnerHtml = spanDate.ToString() + spanTime.ToString();

                //criação do nó que conterá o ícone do item da timeline
                var timelineIcon = new TagBuilder("div");
                if (!timelineItem.EntrePatios) timelineIcon.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-icon"));

                //verificação se o item possui css indicando qual ícone a exibir
                if (timelineItem.MiddleIconCss != "")
                {
                    var icon = new TagBuilder("i");
                    icon.Attributes.Add(new KeyValuePair<string, string>("class", timelineItem.MiddleIconCss));

                    timelineIcon.InnerHtml = icon.ToString();
                }

                //criação do nó que conterá o "corpo" da timeline
                var timelineBody = new TagBuilder("div");
                timelineBody.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-body"));

                //criação do nó que conterá o título
                var timelineTitle = new TagBuilder("H2");
                timelineTitle.InnerHtml = timelineItem.Title;

                //criação do nó que conterá a dados antes da descrição do conteúdo do item da timeline
                var timelineAfterContent = new TagBuilder("div");
                timelineAfterContent.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-content"));
                timelineAfterContent.InnerHtml = timelineItem.AfterDescription;

                //criação do nó que conterá a descrição do conteúdo do item da timeline
                var timelineContent = new TagBuilder("div");
                timelineContent.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-content"));
                timelineContent.InnerHtml = timelineItem.Description;

                //criação do nó que conterá a dados depois da descrição do conteúdo do item da timeline
                var timelineBeforeContent = new TagBuilder("div");
                timelineBeforeContent.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-content"));
                timelineBeforeContent.InnerHtml = timelineItem.BeforeDescription;

                //criação do nó que conterá o rodapé do item da timeline
                var timelineFooter = new TagBuilder("div");
                timelineFooter.Attributes.Add(new KeyValuePair<string, string>("class", "timeline-footer"));

                //criação do nó que conterá o link de "leia mais" no rodapé da timeline
                var readMoreLink = new TagBuilder("a");
                readMoreLink.Attributes.Add(new KeyValuePair<string, string>("class", "nav-link pull-right"));
                readMoreLink.Attributes.Add(new KeyValuePair<string, string>("href", "javascript:" + timelineItem.LinkContent + ";"));

                //criação do nó que conterá a imagem alinhada a descrição "Leia mais" no rodapé
                var readMoreImg = new TagBuilder("i");
                readMoreImg.Attributes.Add(new KeyValuePair<string, string>("class", "fa fa-chevron-circle-right"));

                //atribuição dos valores ao link "Leia mais..."
                readMoreLink.InnerHtml = timelineItem.LinkDescription + readMoreImg.ToString();

                //atribuição dos valores ao nó de cabecalho
                //timelineTitle.InnerHtml = readMoreLink.ToString();

                //atribuição dos valores ao nó de rodapé
                if (!String.IsNullOrEmpty(timelineItem.LinkDescription)) timelineFooter.InnerHtml = readMoreLink.ToString();

                //atribuição dos valores ao nó que representa o corpo do item da timeline
                timelineBody.InnerHtml = timelineTitle.ToString() + timelineBeforeContent.ToString() + "<br/>" + timelineContent.ToString() + "<br/>" + timelineAfterContent.ToString() + timelineFooter.ToString();

                //atribuição dos valores ao nó que representa o item completo da timeline
                timelineColor.InnerHtml = timelineTime.ToString() + timelineIcon.ToString() + timelineBody.ToString();

                unorderedListTimeline.InnerHtml += timelineColor.ToString();
            }

            return new MvcHtmlString(unorderedListTimeline.ToString());
        }

        public static MvcHtmlString RenderItensDominioComboBox(this HtmlHelper helper,
                                                                string nome,
                                                                string nomeDominio,
                                                                object htmlAttributes = null,
                                                                bool isToShowItemValue = false)
        {
            ComboBoxBuilder comboBoxBuilder = helper.Kendo().ComboBox();

            comboBoxBuilder.Name(nome);

            comboBoxBuilder.DataValueField("Vl_Item_Dominio");
            comboBoxBuilder.DataTextField("Dc_Item_Dominio");

            if (htmlAttributes != null)
                comboBoxBuilder.HtmlAttributes(htmlAttributes);

            comboBoxBuilder.DataSource(dataSource =>
            {
                dataSource.Read("ItensDominio", "Dominios", new { nomeDominio = nomeDominio, isToShowItemValue = isToShowItemValue });
            });

            return MvcHtmlString.Create(comboBoxBuilder.ToHtmlString());
        }
    }
}
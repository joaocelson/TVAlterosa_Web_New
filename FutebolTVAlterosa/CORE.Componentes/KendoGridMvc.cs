using System;
using Kendo.Mvc.UI;

namespace CORE.Componentes
{
    /// <summary>
    ///     Classe utilizada para padronizar o grid do KendoUI
    /// </summary>
    public class KendoGridMvc<T> : Kendo.Mvc.UI.Fluent.GridBuilder<T> where T : class
    {
        #region Membros

        private readonly Grid<T> _componente;

        #endregion

        #region Construtores

        /// <summary>
        ///     Construtor que padroniza o grid KendoUI
        /// </summary>
        /// <param name="component">Grid a ser extendido</param>
        public KendoGridMvc(Grid<T> component, bool readOnly = false)
            : base(component)
        {
            _componente = component;

            Pageable()
                .Sortable()
                .Scrollable()
                .Filterable()
                .Pageable()
                .Filterable()
                .Pageable()
                .Scrollable()
                .Sortable()
                .Reorderable(r => r.Columns(true))
                .Resizable(r => r.Columns(true))
                .Groupable(g => g.Enabled(false))
                .Editable(editable =>
                {
                    if (!readOnly)
                        editable.Mode(GridEditMode.PopUp);
                    else
                        editable.Enabled(false);
                })
                .HtmlAttributes(new { style = "height:440px;" })
                .DataSource(dataSource => dataSource
                    .Ajax()
                    .PageSize(10))
                .ToolBar(toolBar =>
                {
                    if (!readOnly)
                        toolBar.Create();
                })

                .Columns(columns =>
                    {
                        if (!readOnly)
                        {
                            columns.Command(command =>
                            {

                                command.Edit()
                                    .Text("<i class='glyphicon glyphicon-pencil'></i>")
                                    .HtmlAttributes(new { title = "Editar", @data_toggle = "tooltip", @data_placement = "top" });
                                command.Destroy()
                                    .Text("<i class='glyphicon glyphicon-remove'></i>")
                                    .HtmlAttributes(new { title = "Remover", @data_toggle = "tooltip", @data_placement = "top" });
                            }).Width(80).HtmlAttributes(new { style = "text-align:center;" });
                        }
                    })

                .Events(e =>
                {
                    //if (!readOnly)
                    e.DataBound("(function(){ $('[data-toggle=tooltip]').tooltip();})");
                });

        }

        #endregion

        #region Metodos Publicos

        /// <summary>
        ///     Metodo para Criar um Toolbar com o controle Limpar Filtros
        /// </summary>
        /// <returns></returns>
        public KendoGridMvc<T> ToolbarLimpar(string id)
        {
            ToolbarLimpar(id, "Limpar");

            return this;
        }

        /// <summary>
        ///     Metodo para Criar um Toolbar com o controle Limpar Filtros
        /// </summary>
        /// <returns></returns>
        public KendoGridMvc<T> ToolbarLimpar(string id, string texto)
        {
            //e.preventDefault(); $(\"form.k-filter-menu button[type='reset']\").trigger(\"click\");
            var script = "(function() {$(\"form.k-filter-menu button[type='reset']\").trigger(\"click\");}());";

            ToolBar(toolBar => toolBar.Custom()
                .Text(texto)
                .Url("#")
                .HtmlAttributes(new { id = id, @class = "k-grid-clear k-button-custom", @onclick = script }));

            return this;
        }

        public KendoGridMvc<T> ToolbarExportarExcel(string urlAction, string arquivo)
        {
            var randonGen = new Random();
            var id = "cmdExcel_" + randonGen.Next(0, 1000);

            var script = "(function(){";
            script += "var grid = $(\"#" + _componente.Name + "\").data(\"kendoGrid\");";

            script += "var parameterMap = grid.dataSource.transport.parameterMap;";
            script += "var requestObject = parameterMap({ sort: grid.dataSource.sort(), filter: grid.dataSource.filter(), aggregate: grid.dataSource.aggregate(), group: grid.dataSource.group()});";
            script += "var columns = JSON.stringify(grid.columns);";

            script += "var colunas = new Array();";

            script += "for (var x in grid.columns){ var coluna = {title: grid.columns[x].title, width:grid.columns[x].width, field:grid.columns[x].field}; colunas.push(coluna);};";
            script += "var colunasRequest = JSON.stringify(colunas);";

            script += "var request = decodeURIComponent($.param(requestObject));";

            script += "var pageId = typeof $('#hddPageId').val() === 'undefined' ? -1 : $('#hddPageId').val(); ";

            script += "location.href = \"" + urlAction + "\" + \"?\" + request + \"&arquivo=" + arquivo + "&pageId=\" + pageId + \"&colunas=\" + colunasRequest;";//&pageId=\" + pageId; ";
            script += "}())";

            ToolBar(toolBar => toolBar.Custom()
                .Text("Excel")
                .Url("#_")
                .HtmlAttributes(new { id = id, @class = "k-button-export-excel k-button-custom", @onclick = script, span = "text" }));

            return this;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

    }
}

(function ($) {
    var MyGrid = kendo.ui.Grid.extend({
        init: function (element, options) {
            var _options = new Object;

            _options.columns = options.columns;
            _options.dataSource = options.dataSource;

            _options.autoBind = options.autoBind || false;
            _options.filterable = {
                messages: {
                    info: "Exibir linhas com valores que",
                    filter: "Filtrar",
                    clear: "Limpar",
                    and: "E",
                    or: "Ou",
                },

                operators: {
                    string: {
                        eq: "É igual a",
                        neq: "Não é igual a",
                        startswith: "Começa com",
                        endswith: "Termina com",
                        contains: "Contém",
                        doesnotcontain: "Não contém",
                    },
                    number: {
                        eq: "É igual a",
                        neq: "Não é igual a",
                        gte: "É maior que ou igual a",
                        gt: "É maior que",
                        lte: "É menor que ou igual a",
                        lt: "É menor que",
                    }
                    //lt: "É menor que",
                    //lte:"É maior que ou igual a",
                    //gt: "É maior que",
                    //gt: "É maior que ou igual a",
                    //startswith: "Começa com",
                    //endswith: "Termina com",
                    //contains: "Contém",
                    //ncontains: "Não contém",
                    //The supported operators are: "eq" (equal to), "neq" (not equal to), "lt" (less than), "lte" (less than or equal to), "gt" (greater than)
                    //, "gte" (greater than or equal to), "startswith", "endswith", "contains". The last three are supported only for string fields.
                    //isTrue: "es verdadero",
                    //isFalse: "es falso",
                    //selectValue: "-Selecciona valor-",
                    //operator: "Operador",
                    //value: "Valor",
                    //cancel: "Cancelar"
                }
            }, //options.filterable || true;
            _options.pageable = options.pageable || true;
            _options.resizable = options.resizable || true;
            _options.sortable = options.sortable || true;
            _options.scrollable = options.scrollable || true;
            _options.height = options.height || 250;

            var _toolbar = [
                    {
                        name: "Limpar",
                        text: "Limpar",
                        attr: " href=\"#\" id=\"" + options.toolbarClearButtonName + "\" class='k-grid-clear k-button-custom k-button k-button-icontext' onclick=\"(function() {$(\u0026quot;form.k-filter-menu button[type=\u0026#39;reset\u0026#39;]\u0026quot;).trigger(\u0026quot;click\u0026quot;);}());\""
                    },
                    {
                        name: "Excel",
                        text: "Excel",
                        attr: " id=\"cmdExcel_" + makeid() + "\" class='k-button-export-excel k-button-custom' onclick=\"exportToExcel('" + options.name + "', '" + options.toolbarExcelUrlAction + "', '" + options.toolbarExcelFileName + "');\" span=\"text\""
                    }
            ];

            _options.toolbar = _toolbar;

            kendo.culture("pt-BR");

            kendo.ui.Grid.fn.init.call(this, element, _options);
        },
        options: {
            name: "PortalGrid"
        },
    });
    kendo.ui.plugin(MyGrid);
})(jQuery);

function exportToExcel(componentName, toolbarExcelUrlAction, toolbarExcelFileName) {
    var grid = $("#" + componentName).data("kendoPortalGrid");

    var requestObject = (new kendo.data.transports["aspnetmvc-server"]({ prefix: "" }))
                                                                        .options.parameterMap({
                                                                            page: grid.dataSource.page(),
                                                                            sort: grid.dataSource.sort(),
                                                                            filter: grid.dataSource.filter(),
                                                                            group: grid.dataSource.group()
                                                                        });
    var columns = JSON.stringify(grid.columns);
    var colunas = new Array();
    for (var x in grid.columns) {
        var coluna = { title: grid.columns[x].title, width: grid.columns[x].width, field: grid.columns[x].field };
        colunas.push(coluna);
    }

    var colunasRequest = JSON.stringify(colunas);
    var request = decodeURIComponent($.param(requestObject));
    var pageId = typeof $('#hddPageId').val() === 'undefined' ? -1 : $('#hddPageId').val();

    location.href = toolbarExcelUrlAction + "?" + request + "&arquivo=" + toolbarExcelFileName + "&pageId=" + pageId + "&colunas= " + colunasRequest;
}

function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 5; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}
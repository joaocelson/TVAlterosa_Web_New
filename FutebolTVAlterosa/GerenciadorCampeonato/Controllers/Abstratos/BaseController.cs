using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using System;

namespace GerenciadorCampeonato.Controllers.Abstratos
{
    public abstract class BaseController : Controller
    {
        protected class ColunasGridKendo
        {
            public string title { get; set; }
            public string width { get; set; }
            public string field { get; set; }
        }

        //protected virtual FileResult Exportar([DataSourceRequest] DataSourceRequest request, string arquivo, string colunas, IEnumerable lista)
        //{

        //    var colunasGrid = JsonConvert.DeserializeObject<List<ColunasGridKendo>>(colunas);

        //    IEnumerable dados = lista;

        //    var workbook = new HSSFWorkbook();

        //    var sheet = workbook.CreateSheet();
        //    var headerRow = sheet.CreateRow(0);

        //    int indiceColuna = 0;

        //    foreach (var coluna in colunasGrid)
        //    {
        //        if (coluna.field != null)
        //        {
        //            headerRow.CreateCell(indiceColuna).SetCellValue(coluna.title);
        //            indiceColuna++;
        //        }
        //    }

        //    sheet.CreateFreezePane(0, 1, 0, 1);

        //    int indiceLinha = 1;

        //    foreach (TEnt unidade in dados)
        //    {
        //        var row = sheet.CreateRow(indiceLinha++);

        //        indiceColuna = 0;
        //        foreach (var coluna in colunasGrid)
        //        {
        //            if (coluna.field != null)
        //            {
        //                var propriedade = unidade.GetType().GetProperty(coluna.field);
        //                if (propriedade != null)
        //                {
        //                    var valor = propriedade.GetValue(unidade, null) ?? "";

        //                    row.CreateCell(indiceColuna).SetCellValue(valor.ToString());
        //                    indiceColuna++;
        //                }
        //            }
        //        }
        //    }

        //    indiceColuna = 0;
        //    foreach (var coluna in colunasGrid)
        //    {
        //        if (coluna.field != null)
        //        {
        //            sheet.AutoSizeColumn(indiceColuna);
        //            indiceColuna++;
        //        }
        //    }

        //    var output = new MemoryStream();
        //    workbook.Write(output);

        //    return File(output.ToArray(), "application/vnd.ms-excel", arquivo);
        //}

        protected FileResult Exportar([DataSourceRequest] DataSourceRequest request, IEnumerable dados, string arquivo, string colunas)
        {
            #region Comentários

            //var colunasGrid = JsonConvert.DeserializeObject<List<ColunasGridKendo>>(colunas);

            ////IEnumerable dados = _repositorio.GetAll().ToList().ToDataSourceResult(request).Data;

            //var workbook = new HSSFWorkbook();

            //var sheet = workbook.CreateSheet();
            //var headerRow = sheet.CreateRow(0);

            //int indiceColuna = 0;

            //foreach (var coluna in colunasGrid)
            //{
            //    if (coluna.field != null)
            //    {
            //        headerRow.CreateCell(indiceColuna).SetCellValue(coluna.title);
            //        indiceColuna++;
            //    }
            //}

            //sheet.CreateFreezePane(0, 1, 0, 1);

            //int indiceLinha = 1;

            //if (dados != null)
            //{
            //    foreach (object unidade in dados)
            //    {
            //        var row = sheet.CreateRow(indiceLinha++);

            //        indiceColuna = 0;
            //        foreach (var coluna in colunasGrid)
            //        {
            //            if (coluna.field != null)
            //            {
            //                var propriedade = unidade.GetType().GetProperty(coluna.field);
            //                if (propriedade != null)
            //                {
            //                    var valor = propriedade.GetValue(unidade, null) ?? "";

            //                    row.CreateCell(indiceColuna).SetCellValue(valor.ToString());
            //                    indiceColuna++;
            //                }
            //            }
            //        }
            //    }
            //}

            //indiceColuna = 0;
            //foreach (var coluna in colunasGrid)
            //{
            //    if (coluna.field != null)
            //    {
            //        sheet.AutoSizeColumn(indiceColuna);
            //        indiceColuna++;
            //    }
            //}

            //var output = new MemoryStream();
            //workbook.Write(output);

            //return File(output.ToArray(), "application/vnd.ms-excel", arquivo);

            #endregion

            var colunasGrid = JsonConvert.DeserializeObject<List<ColunasGridKendo>>(colunas);

            IEnumerable _dados = (dados != null) ? dados.ToDataSourceResult(request).Data : null;

            var workbook = new HSSFWorkbook();

            var sheet = workbook.CreateSheet();
            var headerRow = sheet.CreateRow(0);

            int indiceColuna = 0;

            foreach (var coluna in colunasGrid)
            {
                if (coluna.field != null)
                {
                    headerRow.CreateCell(indiceColuna).SetCellValue(coluna.title);
                    indiceColuna++;
                }
            }

            sheet.CreateFreezePane(0, 1, 0, 1);

            if (_dados != null)
            {
                int indiceLinha = 1;


                if (_dados.GetType() == typeof(List<Kendo.Mvc.Infrastructure.AggregateFunctionsGroup>))
                {// Existe agrupamento no grid

                    //Lista os agrupamentos para totalização
                    List<KeyValuePair<string, object>> listaAggregates = new List<KeyValuePair<string, object>>();

                    MontarAgrupamentos(colunasGrid, sheet, ref indiceLinha, _dados, ref listaAggregates);

                    PreencherLinhaTotalizacaoGeral(colunasGrid, sheet, ref indiceLinha, ref listaAggregates);
                }
                else // Sem agrupamento no grid
                {
                    PreencherDadosPlanilha(colunasGrid, _dados, sheet, ref indiceLinha);
                }

                indiceColuna = 0;
                foreach (var coluna in colunasGrid)
                {
                    if (coluna.field != null)
                    {
                        sheet.AutoSizeColumn(indiceColuna);
                        indiceColuna++;
                    }
                }
            }

            var output = new MemoryStream();
            workbook.Write(output);

            return File(output.ToArray(), "application/vnd.ms-excel", arquivo);

        }

        private static void PreencherDadosPlanilha(List<ColunasGridKendo> colunasGrid, IEnumerable dados, NPOI.SS.UserModel.ISheet sheet, ref int indiceLinha)
        {
            foreach (var unidade in dados)
            {
                var row = sheet.CreateRow(indiceLinha++);

                var indiceColuna = 0;
                foreach (var coluna in colunasGrid)
                {
                    if (coluna.field != null)
                    {
                        //var propriedade = unidade.GetType().GetProperty(coluna.field);
                        var propriedade = GetPropertyValue(unidade, coluna.field);
                        if (propriedade != null)
                        {
                            //var valor = propriedade.GetType().GetValue(unidade, null);

                            //dynamic valorConvertido = (propriedade != null) ? ChangeType(propriedade, propriedade.PropertyType) : String.Empty;

                            row.CreateCell(indiceColuna).SetCellValue(propriedade);//.ToString()
                            indiceColuna++;
                        }
                    }
                }
            }
        }

        private static void MontarAgrupamentos(List<ColunasGridKendo> colunasGrid, NPOI.SS.UserModel.ISheet sheet, ref int indiceLinha, IEnumerable grupos, ref List<KeyValuePair<string, object>> listaAggregates)
        {
            NPOI.SS.UserModel.IFont font = sheet.Workbook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            foreach (Kendo.Mvc.Infrastructure.AggregateFunctionsGroup grupo in grupos)
            {
                //Titulo do Grupo
                var row = sheet.CreateRow(indiceLinha++);
                var valorChaveGrupo = grupo.Member + ": " + (grupo.Key == null ? String.Empty : grupo.Key).ToString();
                row.CreateCell(0).SetCellValue(valorChaveGrupo.ToString());

                //Marca o título do grupo com negrito
                row.Cells[0].CellStyle = sheet.Workbook.CreateCellStyle();
                row.Cells[0].CellStyle.SetFont(font);

                if (colunasGrid.Count > 0 && colunasGrid[0].field == null) colunasGrid.RemoveAt(0);

                if (grupo.HasSubgroups)
                {
                    //Mapeia os subgrupos recursivamente
                    MontarAgrupamentos(colunasGrid, sheet, ref indiceLinha, grupo.Items, ref listaAggregates);
                }
                else
                {
                    PreencherDadosPlanilha(colunasGrid, grupo.Items, sheet, ref indiceLinha);

                    //Se há linha de totalização do grupo
                    if (grupo.Aggregates.Count > 0)
                    {
                        row = sheet.CreateRow(indiceLinha++);

                        listaAggregates.AddRange(grupo.Aggregates);

                        foreach (var total in grupo.Aggregates)
                        {
                            var valor = ((Dictionary<string, object>)total.Value).FirstOrDefault().Value;

                            if (valor.GetType() == typeof(decimal))
                            {
                                valor = System.Math.Round((decimal)valor, 2);
                            }

                            int indiceColuna = colunasGrid.IndexOf(colunasGrid.Where(it => it.field == total.Key).FirstOrDefault());

                            var celula = row.CreateCell(indiceColuna);

                            celula.SetCellValue(Convert.ToDouble(valor));
                            celula.CellStyle = sheet.Workbook.CreateCellStyle();
                            celula.CellStyle.SetFont(font);
                        }

                        row = sheet.CreateRow(indiceLinha++);
                    }

                }
            }

            return;
        }

        private static int PreencherLinhaTotalizacaoGeral(List<ColunasGridKendo> colunasGrid, NPOI.SS.UserModel.ISheet sheet, ref int indiceLinha, ref List<KeyValuePair<string, object>> listaAggregates)
        {
            NPOI.SS.UserModel.IFont font = sheet.Workbook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            var rowTotal = sheet.CreateRow(indiceLinha++);

            if (colunasGrid.Count > 0 && colunasGrid[0].field == null) colunasGrid.RemoveAt(0);

            foreach (var coluna in colunasGrid)
            {
                //Lista com os aggregates da coluna no grid
                List<KeyValuePair<string, object>> listaValoresColuna =
                    listaAggregates
                    .Where(it => it.Key == coluna.field)
                    .ToList();

                decimal valorTotalColuna = 0;

                if (listaValoresColuna.Count > 0)
                {
                    string tipoAggregate = ((Dictionary<string, object>)listaValoresColuna.FirstOrDefault().Value).FirstOrDefault().Key.ToString();

                    //Lista de todos os valores das linhas do grid para a coluna atual
                    List<decimal> listaTotal = listaValoresColuna
                        .Select(it => Convert.ToDecimal(((Dictionary<string, object>)it.Value).FirstOrDefault().Value))
                        .ToList();

                    switch (tipoAggregate)
                    {
                        case "Sum":
                            valorTotalColuna = listaTotal.Sum();
                            break;
                        case "Average":
                            valorTotalColuna = listaTotal.Average();
                            break;
                        case "Min":
                            valorTotalColuna = listaTotal.Min();
                            break;
                        case "Max":
                            valorTotalColuna = listaTotal.Max();
                            break;
                        case "Count":
                            valorTotalColuna = listaTotal.Count();
                            break;
                        default:
                            valorTotalColuna = 0;
                            break;
                    }

                    int indiceColuna = colunasGrid.IndexOf(colunasGrid.Where(it => it.field == coluna.field).FirstOrDefault());

                    var celula = rowTotal.CreateCell(indiceColuna);

                    if (valorTotalColuna.GetType() == typeof(decimal))
                    {
                        valorTotalColuna = System.Math.Round((decimal)valorTotalColuna, 2);
                    }

                    celula.SetCellValue(System.Convert.ToDouble(valorTotalColuna));
                    celula.CellStyle = sheet.Workbook.CreateCellStyle();
                    celula.CellStyle.SetFont(font);
                }
            }
            return indiceLinha;
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);

                t = (t == typeof(decimal)) ? typeof(double) : (t == typeof(DateTime)) ? typeof(string) : t;
            }
            else
            {
                if (t == typeof(DateTime))
                {
                    t = typeof(string);
                }
            }

            return Convert.ChangeType(value, t);
        }

        public static dynamic GetPropertyValue(object src, string propName)
        {
            if (propName.Contains("."))//complex type nested 
            { 
                var temp = propName.Split(new char[] { '.' }, 2); 
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]); 
            }
            else { 
                var prop = src.GetType().GetProperty(propName); 
                var prop2 = prop != null ? prop.GetValue(src, null) : null;
                return (prop2 != null) ? ChangeType(prop2, prop.PropertyType) : String.Empty;
            }
        }

    }
}
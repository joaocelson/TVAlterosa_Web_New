using Kendo.Mvc.UI;
using CORE.Componentes.Personalizados.AutoComplete.Interfaces;

namespace CORE.Componentes.Personalizados.AutoComplete.Providers
{
    public class AutoCompleteFamiliaPrefixoTremProvider : IAutoCompleteProvider
    {
        //Propriedades

        //Propriedade que indica qual campo será exibido no componente
        public string DataTextField { get; set; }
        //Nome do componente
        public string Name { get; set; }
        //Tipo de filtro (Começando com | Contendo)
        public FilterType FilterType { get; set; }
        //Nome da action no controller
        public string ActionName { get; set; }
        //Nome do controller
        public string ControllerName { get; set; }
        //Indica se é para realizar o filtro no servidor
        public bool ServerFiltering { get; set; }
        //Nome do arquivo de template para o cabeçalho do resultado
        public string HeaderTemplate { get; set; }
        //Nome do arquivo de template para o resultado
        public string Template { get; set; }
        //Mínimo de caracteres digitados para inicar a pesquisa
        public int MinLength { get; set; }
        //Atributos do html renderizado
        public object HtmlAttributes { get; set; }

        //Eventos Javascript

        //Nome do método em JavaScript que responde ao evento de change do componente
        public string OnChange { get; set; }
        //Nome do método em JavaScript que responde ao evento de select do componente
        public string OnSelect { get; set; }
        //Nome do método em JavaScript que responde ao evento de databound do componente
        public string OnDataBound { get; set; }
        //Nome do método em JavaScript que responde ao evento de close do componente
        public string OnClose { get; set; }
        //Nome do método em JavaScript que responde ao evento de open do componente
        public string OnOpen { get; set; }
        //Nome do método em JavaScript que responde ao evento de senddata do componente
        public string OnSendData { get; set; }

        public AutoCompleteFamiliaPrefixoTremProvider(string name)
        {
            //Propriedades
            DataTextField = "Text";
            ActionName = "FamiliaPrefixoTrem";
            ControllerName = "PrevisaoChegadaTrens";
            Name = name;
            MinLength = 1;
            Template = "_autoCompleteFamiliaPrefixoTremTemplate.tmpl.htm";            

            //Eventos
            //OnChange = "autoCompleteVeicFerroviario_change";
            //OnClose = "autoCompleteVeicFerroviario_close";
            //OnDataBound = "autoCompleteVeicFerroviario_databound";
            //OnOpen = "autoCompleteVeicFerroviario_open";
            //OnSelect = "autoCompleteVeicFerroviario_select";            
        }

    }
}

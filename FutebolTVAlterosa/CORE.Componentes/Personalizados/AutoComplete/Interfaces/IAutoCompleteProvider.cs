using Kendo.Mvc.UI;

namespace CORE.Componentes.Personalizados.AutoComplete.Interfaces
{
    public interface IAutoCompleteProvider
    {
        //Propriedades

        //Propriedade que indica qual campo será exibido no componente
        string DataTextField { get; set; }
        //Nome do componente
        string Name { get; set; }
        //Tipo de filtro (Começando com | Contendo)
        FilterType FilterType { get; set; }
        //Nome da action no controller
        string ActionName { get; set; }
        //Nome do controller
        string ControllerName { get; set; }
        //Indica se é para realizar o filtro no servidor
        bool ServerFiltering { get; set; }
        //Nome do arquivo de template para o cabeçalho do resultado
        string HeaderTemplate { get; set; }
        //Nome do arquivo de template para o resultado
        string Template { get; set; }
        //Mínimo de caracteres digitados para inicar a pesquisa
        int MinLength { get; set; }
        //Atributos do html renderizado
        object HtmlAttributes { get; set; }

        //Eventos Javascript

        //Nome do método em JavaScript que responde ao evento de change do componente
        string OnChange { get; set; }
        //Nome do método em JavaScript que responde ao evento de select do componente
        string OnSelect { get; set; }
        //Nome do método em JavaScript que responde ao evento de databound do componente
        string OnDataBound { get; set; }
        //Nome do método em JavaScript que responde ao evento de close do componente
        string OnClose { get; set; }
        //Nome do método em JavaScript que responde ao evento de open do componente
        string OnOpen { get; set; }
        //Nome do método em JavaScript que responde ao evento de senddata do componente
        string OnSendData { get; set; }
    }
}

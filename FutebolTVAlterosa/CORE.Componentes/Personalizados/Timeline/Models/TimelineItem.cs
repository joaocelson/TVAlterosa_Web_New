using System;
using CORE.Componentes.Personalizados.Timeline.Enums;

namespace CORE.Componentes.Personalizados.Timeline.Models
{
    /// <summary>
    /// Classe que representa um item da timeline
    /// </summary>
    public class TimelineItem
    {
        /// <summary>
        /// Data e hora do evento
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Cor do evento na timeline
        /// </summary>
        public TimelineColor Color { get; set; }        
        /// <summary>
        /// Css (font-awesome) que representará o ícone para o item da timeline
        /// </summary>
        public string MiddleIconCss { get; set; }
        /// <summary>
        /// Título do item da timeline
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Descrição do item da timeline
        /// </summary>
        public string BeforeDescription { get; set; }
        public string Description { get; set; }
        public string AfterDescription { get; set; }
        public string LinkDescription { get; set; }
        public string LinkContent { get; set; }
        public string MarcacaoPatioAtual { get; set; }
        public string SiglaPatio { get; set; }
        public bool EntrePatios { get; set; }
    }
}

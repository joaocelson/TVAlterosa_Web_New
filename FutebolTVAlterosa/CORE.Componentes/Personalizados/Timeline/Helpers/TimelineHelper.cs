using CORE.Componentes.Personalizados.Timeline.Enums;

namespace CORE.Componentes.Personalizados.Timeline.Helpers
{
    /// <summary>
    /// Classe que contém métodos auxiliares para renderização da timeline
    /// </summary>
    public static class TimelineHelper
    {
        /// <summary>
        /// Método resposnável por retornar o nome do css que representa a cor do item da timeline
        /// </summary>
        /// <param name="color">Cor do item da timeline</param>
        /// <returns>String que representa o Css para cor</returns>
        public static string GetCssByTimelineColor(TimelineColor color)
        {
            switch (color)
            {
                case TimelineColor.Yellow:
                    {
                        return "timeline-yellow";                        
                    }
                case TimelineColor.Blue:
                    {
                        return "timeline-blue";                        
                    }
                case TimelineColor.Green:
                    {
                        return "timeline-green";                        
                    }
                case TimelineColor.Purple:
                    {
                        return "timeline-purple";                        
                    }
                case TimelineColor.Red:
                    {
                        return "timeline-red";                        
                    }
                case TimelineColor.Grey:
                    {
                        return "timeline-grey";                        
                    }
                default:
                    return "";
            }
        }

        public static TimelineColor GetNextColor(TimelineColor? color)
        {
            if (color == null)
                return TimelineColor.Blue;

            if (color == TimelineColor.Blue)
                return TimelineColor.Yellow;

            if (color == TimelineColor.Yellow)
                return TimelineColor.Purple;

            if (color == TimelineColor.Purple)
                return TimelineColor.Green;

            if (color == TimelineColor.Green)
                return TimelineColor.Blue;

            return TimelineColor.Blue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Dominio
{
    public class FotosVideos

    {
        public String Id { get; set; }
        [DisplayName("Url da foto")]
        public String Caminho { get; set; }
        [DisplayName("Descrição")]
        public String Descricao { get; set; }
        [DisplayName("Time")]
        public String IdTime { get; set; }
        [DisplayName("Campeonato")]
        public String IdCampeonato { get; set; }
        [DisplayName("Partida")]
        public String IdPartida { get; set; }
        [DisplayName("Rodada")]
        public String Rodada { get; set; }
        [DisplayName("Se Foto 0 - Vídeo 1")]
        public String Video { get; set; }


    }

   
}

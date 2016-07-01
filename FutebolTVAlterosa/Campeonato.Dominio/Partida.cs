using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Dominio
{
    public class Partida
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o time mandante")]
        public string IdTimeMandante { get; set; }

        [Required(ErrorMessage = "Selecione o campeonato")]
        public string IdCampeonato { get; set; }

        [Required(ErrorMessage = "Selecione o time visitante")]
        public string IdTimeVisitante { get; set; }


        public string GolMandante { get; set; }


        public string GolVisitante { get; set; }

        [DisplayName("Local Partida")]
        [Required(ErrorMessage = "Informe o Local partida")]
        public string LocalPartida { get; set; }

        [Required(ErrorMessage = "Informe a data da partida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DataPartida { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DataPartidaRemarcada { get; set; }

        public Boolean Remarcada { get; set; }

        public String TimeMandante { get; set; }

        public String TimeVisitante { get; set; }

        public String EscudoPequenoMandante { get; set; }

        public String EscudoPequenoVisitante { get; set; }

        [Required(ErrorMessage = "Informe a rodada da partida")]
        public String Rodada
        {
            get;
            set;
        }

        public Boolean InverterMandante { get; set; }

        public Boolean PontosComputados { get; set; }

        public IEnumerable<FotosVideos> ListaFotosVideos { get; set; }

        public string NomeCampeonato { get; set; }

        public JogoOnline JogoOnline { get; set; }

        public Campeonatos Campeonatos { get; set; }

        public Time TimeVisitanteObj { get; set; }

        public Time TimeMandanteObj { get; set; }

        public String Estadio { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Campeonato.Dominio
{
    public class Partida_Web
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Time Mandante")]
        [UIHint("GridForeignKey")]
        [Required(ErrorMessage = "O campo Time Mandante é obrigatório.")]
        public int IdTimeMandante { get; set; }

        [Display(Name = "Time Visitante")]
        [UIHint("GridForeignKey")]
        [Required(ErrorMessage = "O campo Time Visitante é obrigatório.")]
        public int IdTimeVisitante { get; set; }

        [Display(Name = "Campeonato")]
        [UIHint("GridForeignKey")]
        [Required(ErrorMessage = "O campo Campeonato é obrigatório.")]
        public int Id_Campeonato { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Gols Mandante")]        
        public string GolMandante { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Gols Visitante")]        
        public string GolVisitante { get; set; }

        [DisplayName("Local Partida")]
        [Required(ErrorMessage = "Informe o Local partida")]
        public string LocalPartida { get; set; }

        [Required(ErrorMessage = "Informe a data da partida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DataPartida { get; set; }

        public Boolean Remarcada { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataPartidaRemarcada { get; set; }

        [HiddenInput(DisplayValue = false)]
        public String DataPartidaRemarcadaString { get; set; }        

        [HiddenInput(DisplayValue = false)]
        public String RemarcadaString { get; set; }
        
        [Required(ErrorMessage = "Informe a rodada da partida")]
        public String Rodada
        {
            get;
            set;
        }

        public Boolean PontosComputados { get; set; }

        [HiddenInput(DisplayValue = false)]
        public String PontosComputadosString { get; set; }        
                      
        public String Estadio { get; set; }
    }
}

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
    public class ResultadoPartida_Web
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
        
        [Display(Name = "Gols Mandante")]        
        public string GolMandante { get; set; }

        [Display(Name = "Gols Visitante")]        
        public string GolVisitante { get; set; }

        [Required(ErrorMessage = "Informe a data da partida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DataPartida { get; set; }
        
    }
}

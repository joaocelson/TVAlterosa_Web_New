using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Campeonato.Dominio
{
    public class Jogador_Web
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Posicao")]
        [Required(ErrorMessage = "O campo Posicao é obrigatório.")]
        public string Posicao { get; set; }

        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo Descricao é obrigatório.")]
        public string Descricao { get; set; }

        [Display(Name = "Time")]
        [UIHint("GridForeignKey")]
        [Required(ErrorMessage = "O campo Time é obrigatório.")]
        public int Id_Time { get; set; }

        [Display(Name = "Campeonato")]
        [UIHint("GridForeignKey")]
        [Required(ErrorMessage = "O campo Campeonato é obrigatório.")]
        public int Id_Campeonato { get; set; }
        
    }
}


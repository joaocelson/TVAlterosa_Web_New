using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Campeonato.Dominio
{
    public class Jogador
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "O campo Foto é obrigatório.")]
        public string Foto { get; set; }

        [Display(Name = "Posicao")]
        [Required(ErrorMessage = "O campo Posicao é obrigatório.")]
        public string Posicao { get; set; }

        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo Descricao é obrigatório.")]
        public string Descricao { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "O campo Time é obrigatório.")]
        public Time Time { get; set; }

        [Display(Name = "Campeonato")]
        [Required(ErrorMessage = "O campo Campeonato é obrigatório.")]
        public Campeonatos Campeonato { get; set; }
        
    }
}


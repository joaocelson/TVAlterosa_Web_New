using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace Campeonato.Dominio
{
    public class Mensagem_Web
    {
        [Display(Name = "Mensagem")]
        [Required(ErrorMessage = "O campo Mensagem é obrigatório.")]
        public string Message { get; set; }

        
        [HiddenInput(DisplayValue = false)]        
        public string TickerText { get; set; }


        [Display(Name = "Título Mensagem")]
        [Required(ErrorMessage = "O campo Título Mensagem é obrigatório.")]
        public string ContentTitle { get; set; }


    }
}

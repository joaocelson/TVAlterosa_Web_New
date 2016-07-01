using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Campeonato.Dominio
{
    public class UsuarioLogin
    {
        [HiddenInput(DisplayValue = false)]
        public int IdUsuario { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Senha { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DataInativacao { get; set; }
    }
}

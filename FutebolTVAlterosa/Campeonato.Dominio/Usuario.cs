using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Campeonato.Dominio
{
    public class Usuario
    {
        public int Id { get; set; }
        public int TipoUsuario { get; set; }


        [Required(ErrorMessage = "Digite o e-mail Usuario")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Digite a Senha")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmaSenha { get; set; }

        [Required(ErrorMessage = "Digite nome do usuário")]
        public string NomeUsuario { get; set; }

        public string Token { get; set; }


    }
}

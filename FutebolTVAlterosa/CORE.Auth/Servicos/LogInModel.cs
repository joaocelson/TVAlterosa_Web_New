using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CORE.Auth.Servicos
{
    public class LogInModel
    {
        [Required]
        [DisplayName("Matricula")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }

    }
}

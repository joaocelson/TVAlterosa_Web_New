
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Campeonato.Dominio
{
    public class Token_Web
    {
        [Key]
        public Guid TokenId { get; set; }

        public String TokenStr { get; set; }

    }
}

using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Dominio
{
    public class Campeonatos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome do campeonato")]
        public string Nome { get; set; }

        [DisplayName("Data Início do Campeonato")]
        [Required(ErrorMessage = "Preencha a data de início do campeonato")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInicio { get; set; }

        [DisplayName("Quantidade de Times Participante")]
        [Required(ErrorMessage = "Preencha a quantidade de times participantes")]

        public int QuantidadeTime { get; set; }

        public int IdBola { get; set; }
    }
}

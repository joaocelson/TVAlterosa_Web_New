using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Campeonato.Dominio
{
    public class Time
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome do Time")]
        public string Nome { get; set; }

        //[Required(ErrorMessage = "Preencha o nome do Presidente")]
        public string Presidente { get; set; }

        //[Required(ErrorMessage = "Preencha o telefone de contato")]
        public string Telefone { get; set; }

        public string Descricao { get; set; }


        //[DisplayName("Data de Fundacao")]
        //[Required(ErrorMessage = "Preencha a data de fundacao do time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataFundacao { get; set; }

        public Boolean SelecionadoCampeonato { get; set; }

        public string EscudoPequeno { get; set; }

        public String EscudoGrande { get; set; }

        public IEnumerable<FotosVideos> ListaFotosVideos { get; set; }

        public List<Jogador> ListaJogadores { get; set; }
    }
}

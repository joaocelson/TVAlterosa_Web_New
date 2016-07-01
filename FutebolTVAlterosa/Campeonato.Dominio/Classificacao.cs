using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Dominio
{
    public class Classificacao
    {
        public String Id { get; set; }
        public String IdTime { get; set; }
        public String IdCampeonato { get; set; }
        public String NomeTime { get; set; }
        public String Pontos { get; set; }
        public String Jogos { get; set; }
        public String Vitoria { get; set; }
        public String Derrota { get; set; }
        public String Empate { get; set; }
        public String GolPro { get; set; }
        public String GolContra { get; set; }
        public String SaldoGol { get; set; }
        public String Posicao { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Dominio
{
    public class Bolao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int GolVisitante { get; set; }
        public int GolMandante { get; set; }
        public int PontosAdquiridos { get; set; }
        public Usuario Usuario { get; set; }
        public Partida Partida { get; set; }
        public Campeonatos Campeonatos { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Dominio
{
    public class TimeCampeonato
    {
        public String Id { get; set; }
        public IEnumerable<Time> Times { get; set; }
        public String IdCampeonato { get; set; }

    }
}

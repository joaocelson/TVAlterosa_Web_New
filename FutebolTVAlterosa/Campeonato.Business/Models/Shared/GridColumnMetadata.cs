using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASSO.Domain.Models.Shared
{
    public class GridColumnMetadata
    {
        public string field { get; set; }
        public string title { get; set; }
        public string type { get; set; }        
        public string template { get; set; }
        public string width { get; set; }
        //public List<Tuple<string, string>> attributes { get; set; }

        //public GridColumnMetadata()
        //{
        //    attributes = new List<Tuple<string, string>>();
        //}
    }
}

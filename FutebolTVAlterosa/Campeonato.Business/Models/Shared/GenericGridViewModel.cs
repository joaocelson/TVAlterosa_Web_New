using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASSO.Domain.Models.Shared
{
    public class GenericGridViewModel
    {
        public List<GridColumnMetadata> Columns { get; set; }
        public dynamic Data { get; set; }
        public string ModelIdFieldName { get; set; }

        public GenericGridViewModel()
        {
            Columns = new List<GridColumnMetadata>();
        }
    }
}

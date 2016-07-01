using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASSO.Domain.Models.Shared
{
    public class TreeViewItemModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool expanded { get; set; }
        public bool hasChildren { get; set; }
        public List<TreeViewItemModel> items { get; set; }

        public TreeViewItemModel()
        {
            items = new List<TreeViewItemModel>();            
        }
    }
}

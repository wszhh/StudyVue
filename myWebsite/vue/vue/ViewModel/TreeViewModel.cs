using System.Collections.Generic;

namespace vue.ViewModel
{
    public class TreeViewModel
    {
        public string id { get; set; }
        public string label { get; set; }
        public List<TreeViewModel> children { get; set; }
    }
}

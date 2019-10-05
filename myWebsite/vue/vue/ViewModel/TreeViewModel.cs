using System.Collections.Generic;

namespace vue.ViewModel
{
    public class TreeViewModel
    {
        public int id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public IEnumerable<TreeViewModel> children { get; set; }
    }
}

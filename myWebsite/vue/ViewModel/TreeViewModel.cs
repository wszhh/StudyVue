using System.Collections.Generic;

namespace vue.ViewModel
{
    public class TreeViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<TreeViewModel> Children { get; set; }
    }
}

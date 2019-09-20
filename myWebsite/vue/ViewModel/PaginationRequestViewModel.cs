using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class PaginationRequestViewModel
    {
        public int limit { get; set; }
        public int page { get; set; }
    }
}

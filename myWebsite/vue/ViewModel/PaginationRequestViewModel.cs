using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class PaginationRequestViewModel
    {
        [Key]
        public int code { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
    }
}

namespace ViewModel
{
    public class PaginationResponeViewModel<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int total { get; set; }
        public T list { get; set; }
    }
}

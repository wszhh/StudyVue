namespace ViewModel
{
    /// <summary>
    /// 不带多余参数的分页数据
    /// </summary>
    public class PaginationRequestViewModel
    {
        public int limit { get; set; }
        public int page { get; set; }
    }

    /// <summary>
    /// 带多余参数的分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationRequestViewModel<T>
    {
        public int limit { get; set; }
        public int page { get; set; }
        public T data { get; set; }
    }
}

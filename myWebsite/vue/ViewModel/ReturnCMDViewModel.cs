namespace vue.ViewModel
{

    public class ReturnCMDViewModel<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }

    }
}

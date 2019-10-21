using System;

namespace vue.ViewModel
{

    public class SigninModel
    {
        public DateTime Date { get; set; }
        public int SignInType { get; set; }


        /// <summary>
        /// 根据签到时间格式化数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int FormatType(DateTime time)
        {

            if (time.Hour >= 9)
            {
                return 2;
            }
            else if (time.Hour < 9)
            {
                return 1;
            }
            return 7;
        }
    }

}

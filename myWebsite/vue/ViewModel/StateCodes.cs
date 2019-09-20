namespace ViewModel
{
    public class StateCodes
    {
        public enum StateCode
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success = 20000,
            /// <summary>
            /// 登录失败，请检查用户名或密码
            /// </summary>
            NameOrPwdError = 10000,
            /// <summary>
            /// 账户已被临时锁定,请稍后再试
            /// </summary>
            IsLocked = 10001,
            /// <summary>
            /// 获取用户信息失败
            /// </summary>
            GetUserInfoError = 10003,
            /// <summary>
            /// token无效
            /// </summary>
            TokenError = 10004,
            /// <summary>
            /// Token刷新失败
            /// </summary>
            TokenRefreshError = 10005,
            /// <summary>
            /// 部门添加失败
            /// </summary>
            AddDepartmentError = 10006,
            /// <summary>
            /// 部门删除失败
            /// </summary>
            DeleteDepartmentError = 10007,
            /// <summary>
            /// 部门编辑失败
            /// </summary>
            EditDepartmentError = 10008,

        }

        //public int SuccessI { get; } = 20000;
    }
}


//public static int GetStateCode(string enumName)
//{
//    try
//    {
//        var values = Enum.GetValues(typeof(StateCode));
//        var ht = new Hashtable();
//        foreach (var val in values)
//        {
//            ht.Add(Enum.GetName(typeof(StateCode), val), val);
//        }
//        return (int)ht[enumName];
//    }
//    catch (Exception e)
//    {
//        throw e;
//    }
//}


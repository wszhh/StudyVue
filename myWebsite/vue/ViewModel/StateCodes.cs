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
            /// <summary>
            /// token或者文件有误
            /// </summary>
            TokenOrFileError = 10009,
            /// <summary>
            /// 更改个人信息失败
            /// </summary>
            ChangeUserInfoError = 10010,
            /// <summary>
            /// token或者信息格式不正确
            /// </summary>
            TokenOrInfoError = 10011,
            /// <summary>
            /// 更改密码失败
            /// </summary>
            ChangePasswordError = 10012,
            /// <summary>
            /// 添加员工失败
            /// </summary>
            AddStaffError = 10013,
            /// <summary>
            /// 用户名已存在
            /// </summary>
            UserNameError = 10014,

            /// <summary>
            /// 修改角色声明失败
            /// </summary>
            ChangeClaimError = 10015,

            /// <summary>
            /// 分配角色失败
            /// </summary>
            SetRoleUsersError = 10016,

            /// <summary>
            /// 删除用户失败
            /// </summary>
            DeleteUserError = 10017,

            /// <summary>
            /// 签到相关错误
            /// </summary>
            AttendanceError = 10018,

        }
    }
}



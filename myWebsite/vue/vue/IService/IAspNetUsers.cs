using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel;
using vue.Areas.Identity.Data;
using vue.ViewModel;

namespace vue.IService
{
    public interface IAspNetUsers
    {
        /// <summary>
        /// 更改用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ReturnCMDViewModel<IActionResult> setUserPhoto(string userId, string newPhotoPath);

        /// <summary>
        /// 更改用户部分信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPhotoPath"></param>
        /// <returns></returns>
        ReturnCMDViewModel<IActionResult> setUserInfo(string userId, UserInfoViewModel NewUserInfo);

        /// <summary>
        /// 获取同事列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        PaginationResponeViewModel<IEnumerable<UserInfoViewModel>> getColleaguesList(PaginationRequestViewModel pagination);

        /// <summary>
        /// 查询同事列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        PaginationResponeViewModel<IEnumerable<UserInfoViewModel>> findColleagueByName(PaginationRequestViewModel<string> pagination);

        /// <summary>
        /// 有权限获取员工列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        PaginationResponeViewModel<IEnumerable<UserInfoViewModel>> getStaffList(PaginationRequestViewModel pagination);

        /// <summary>
        /// 添加员工成功后添加员工信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="NewUserInfo"></param>
        /// <returns></returns>
        ReturnCMDViewModel<IActionResult> setStaffInfo(string UserName, NewUser NewUserInfo);
    }
}

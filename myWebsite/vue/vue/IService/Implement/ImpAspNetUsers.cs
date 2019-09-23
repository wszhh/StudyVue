using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;


namespace vue.IService.Implement
{
    public class ImpAspNetUsers : IAspNetUsers
    {
        private HRCContext db = new HRCContext();

        /// <summary>
        /// 查询同事
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public PaginationResponeViewModel<IEnumerable<UserInfoViewModel>> findColleagueByName(PaginationRequestViewModel<string> pagination)
        {
            var data = db.AspNetUsers.Where(x => x.RealName.Contains(pagination.data)).Skip(pagination.page).Take(pagination.limit)
                .Select(userInfo => new UserInfoViewModel
                {
                    RealName = userInfo.RealName,
                    Birthday = userInfo.Birthday,
                    Sex = userInfo.Sex,
                    PhoneNumber = userInfo.PhoneNumber,
                    Salary = userInfo.Salary,
                    Address = userInfo.Address,
                    JoinTime = userInfo.JoinTime,
                });
            return new PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>()
            {
                list = data,
                total = data.Count(),
            };
        }


        /// <summary>
        /// 获取同事列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public PaginationResponeViewModel<IEnumerable<UserInfoViewModel>> getColleaguesList(PaginationRequestViewModel pagination)
        {
            return new PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>()
            {
                list = db.AspNetUsers.Skip(pagination.page).Take(pagination.limit).Select(userInfo => new UserInfoViewModel
                {
                    RealName = userInfo.RealName,
                    Birthday = userInfo.Birthday,
                    Sex = userInfo.Sex,
                    PhoneNumber = userInfo.PhoneNumber,
                    Salary = userInfo.Salary,
                    Address = userInfo.Address,
                    JoinTime = userInfo.JoinTime,
                }),
                total = db.AspNetUsers.Count()
            };
        }

        /// <summary>
        /// 更改部分用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="NewUserInfo"></param>
        /// <returns></returns>
        public ReturnCMDViewModel<IActionResult> setUserInfo(string userId, UserInfoViewModel NewUserInfo)
        {
            try
            {
                var oldUser = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                oldUser.RealName = NewUserInfo.RealName;
                oldUser.Sex = NewUserInfo.Sex;
                oldUser.Address = NewUserInfo.Address;
                oldUser.Birthday = NewUserInfo.Birthday;
                oldUser.PhoneNumber = NewUserInfo.PhoneNumber;
                oldUser.Introduction = NewUserInfo.Introduction;
                db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ReturnCMDViewModel<IActionResult>() { code = (int)codes.TokenOrInfoError, message = "token或者信息格式不正确" };
            }
            return new ReturnCMDViewModel<IActionResult>() { code = (int)codes.Success, message = "个人信息更改成功" };
        }

        public ReturnCMDViewModel<IActionResult> setUserPhoto(string userId, string newPhotoPath)
        {
            try
            {
                var oldUser = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                oldUser.Photo = newPhotoPath;
                db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ReturnCMDViewModel<IActionResult>() { code = (int)codes.TokenOrFileError, message = "照片更改失败" };
            }
            return new ReturnCMDViewModel<IActionResult>() { code = (int)codes.Success, message = "照片更改成功" };
        }
    }
}

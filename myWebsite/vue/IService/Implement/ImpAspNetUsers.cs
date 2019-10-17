using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;
using vue.Areas.Identity.Data;
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
            var data = db.AspNetUsers.Where(x => x.RealName.Contains(pagination.data))
                .Select(userInfo => new UserInfoViewModel
                {
                    Id = userInfo.Id,
                    RealName = userInfo.RealName,
                    Birthday = userInfo.Birthday,
                    Sex = userInfo.Sex,
                    PhoneNumber = userInfo.PhoneNumber,
                    Salary = userInfo.Salary,
                    Address = userInfo.Address,
                    JoinTime = userInfo.JoinTime,
                    Introduction = userInfo.Introduction
                });
            return new PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>()
            {
                total = data.Count(),
                list = data.Skip(pagination.page).Take(pagination.limit),
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
        /// 更改部分员工信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="NewUserInfo"></param>
        /// <returns></returns>
        public ReturnViewModel<IActionResult> setUserInfo(string userId, UserInfoViewModel NewUserInfo)
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
                return new ReturnViewModel<IActionResult>() { code = (int)codes.TokenOrInfoError, message = "token或者信息格式不正确" };
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.Success, message = "个人信息更改成功" };
        }

        /// <summary>
        /// 更改员工信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="NewUserInfo"></param>
        /// <returns></returns>
        public ReturnViewModel<IActionResult> setStaffInfos(string userId, UserInfoViewModel NewUserInfo)
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
                oldUser.JoinTime = NewUserInfo.JoinTime;
                oldUser.Salary = NewUserInfo.Salary;
                oldUser.DepartmentId = NewUserInfo.DepartmentId;
                db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ReturnViewModel<IActionResult>() { code = (int)codes.TokenOrInfoError, message = "所填写的内容不正确不正确" };
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.Success, message = "员工信息更改成功" };
        }

        /// <summary>
        /// 员工自己更新照片
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPhotoPath"></param>
        /// <returns></returns>
        public ReturnViewModel<IActionResult> setUserPhoto(string userId, string newPhotoPath)
        {
            try
            {
                var oldUser = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                oldUser.Photo = newPhotoPath;
                db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ReturnViewModel<IActionResult>() { code = (int)codes.TokenOrFileError, message = "照片更改失败" };
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.Success, message = "照片更改成功" };
        }



        /// <summary>
        /// 排序搜索获取员工列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public PaginationResponeViewModel<IEnumerable<UserInfoViewModel>> getStaffListByOrder(PaginationRequestViewModel<OrderPropKeywordViewModel> pagination)
        {
            var staffs = db.AspNetUsers.Select(x => x);
            //排序
            switch (pagination.data.Prop + "_" + pagination.data.Order)
            {
                case "id_descending":
                    staffs = staffs.OrderByDescending(s => s.Id);
                    break;
                case "departmentId_descending":
                    staffs = staffs.OrderByDescending(s => s.DepartmentId);
                    break;
                case "salary_descending":
                    staffs = staffs.OrderByDescending(s => s.Salary);
                    break;
            }
            if (!string.IsNullOrEmpty(pagination.data.keyword))
            {
                //搜索
                switch (pagination.data.select)
                {
                    case "id":
                        staffs = staffs.Where(x => x.Id.Contains(pagination.data.keyword));
                        break;
                    case "realName":
                        staffs = staffs.Where(x => x.RealName.Contains(pagination.data.keyword));
                        break;
                    case "departmentId":
                        var id = db.Department.Where(y => y.DepartmentName.Contains(pagination.data.keyword)).FirstOrDefault();
                        staffs = staffs.Where(x => x.DepartmentId == (id == null ? 0 : id.DepartmentId));
                        break;
                }
            }
            return new PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>()
            {
                list = staffs.Skip(pagination.page).Take(pagination.limit).Select(userInfo => new UserInfoViewModel
                {
                    RealName = userInfo.RealName,
                    Birthday = userInfo.Birthday,
                    Sex = userInfo.Sex,
                    PhoneNumber = userInfo.PhoneNumber,
                    Salary = userInfo.Salary,
                    Address = userInfo.Address,
                    JoinTime = userInfo.JoinTime,
                    Id = userInfo.Id,
                    DepartmentId = userInfo.DepartmentId,
                    Introduction = userInfo.Introduction
                }),
                total = staffs.Count()
            };
        }


        public ReturnViewModel<IActionResult> setStaffInfo(string UserName, NewUser NewUserInfo)
        {
            try
            {
                var oldUser = db.AspNetUsers.Where(x => x.UserName == UserName).FirstOrDefault();
                oldUser.RealName = NewUserInfo.RealName;
                oldUser.Sex = NewUserInfo.Sex;
                oldUser.Address = NewUserInfo.Address;
                oldUser.DepartmentId = NewUserInfo.DepartmentId;
                oldUser.JoinTime = NewUserInfo.JoinTime;
                oldUser.Salary = NewUserInfo.Salary;
                oldUser.Birthday = NewUserInfo.Birthday;
                oldUser.PhoneNumber = NewUserInfo.PhoneNumber;
                oldUser.Introduction = NewUserInfo.Introduction;
                oldUser.Photo = NewUserInfo.Photo;
                oldUser.Avatar = NewUserInfo.Avatar;
                db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ReturnViewModel<IActionResult>() { code = (int)codes.TokenOrInfoError, message = "Id未找到或信息错误" };
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.Success, message = "添加成功" };
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.IService.Implement
{
    public class ImpDepartment : IDepartment
    {
        private HRCContext db = new HRCContext();

        /// <summary>
        /// 添加一个部门
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        public ReturnCMDViewModel<Department> AddDepartment(Department adepartment)
        {
            if (db.Department.Select(x => x.DepartmentName == adepartment.DepartmentName).First())
            {
                return new ReturnCMDViewModel<Department>()
                {
                    code = (int)codes.AddDepartmentError,
                    data = adepartment,
                    message = "该部门已存在"
                };
            }
            try
            {
                db.Department.Add(adepartment);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnCMDViewModel<Department>()
                {
                    code = (int)codes.AddDepartmentError,
                    data = adepartment,
                    message = "该部门添加失败"
                    //message = i.Message
                };
            }
            return new ReturnCMDViewModel<Department>()
            {
                code = (int)codes.Success,
                data = adepartment,
            };

        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public ReturnCMDViewModel<PaginationResponeViewModel<IEnumerable<Department>>> GetDepartmentList(PaginationRequestViewModel pagination)
        {
            return new ReturnCMDViewModel<PaginationResponeViewModel<IEnumerable<Department>>>()
            {
                data = new PaginationResponeViewModel<IEnumerable<Department>>()
                {
                    list = db.Department.Skip(pagination.page).Take(pagination.limit),
                    total = db.Department.Count(),
                },
                code = (int)codes.Success,
            };
        }

        /// <summary>
        /// 删一个
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        public ReturnCMDViewModel<Department> DeleteDepartment(Department adepartment)
        {
            try
            {
                var department = db.Department.Where(x => x.DepartmentName == adepartment.DepartmentName).First();
                db.Department.Remove(department);
                db.SaveChanges();

            }
            catch (Exception)
            {
                return new ReturnCMDViewModel<Department>()
                {
                    code = (int)codes.DeleteDepartmentError,
                    data = adepartment,
                    message = "删除失败"
                };
            }
            return new ReturnCMDViewModel<Department>()
            {
                code = (int)codes.Success,
                data = adepartment
            };
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        public ReturnCMDViewModel<Department> EditDepartment(Department adepartment)
        {
            try
            {
                var department = db.Department.Where(x => x.DepartmentId == adepartment.DepartmentId).First();
                department.DepartmentName = adepartment.DepartmentName;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnCMDViewModel<Department>()
                {
                    code = (int)codes.EditDepartmentError,
                    data = adepartment,
                    message = "编辑失败"
                };
            }
            return new ReturnCMDViewModel<Department>()
            {
                code = (int)codes.Success,
                data = adepartment
            };
        }
    }
}
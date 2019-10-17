using System;
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
        public ReturnViewModel<Department> AddDepartment(Department adepartment)
        {
            if (db.Department.Where(x => x.DepartmentName == adepartment.DepartmentName).Any())
            {
                return new ReturnViewModel<Department>()
                {
                    code = (int)codes.AddDepartmentError,
                    data = adepartment,
                    message = $"\"{adepartment.DepartmentName}\"已存在,请更改后重试"
                };
            }
            try
            {
                db.Department.Add(adepartment);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnViewModel<Department>()
                {
                    code = (int)codes.AddDepartmentError,
                    data = adepartment,
                    message = $"\"{adepartment.DepartmentName}\"添加失败"
                    //message = i.Message
                };
            }
            return new ReturnViewModel<Department>()
            {
                code = (int)codes.Success,
                data = adepartment,
                message = $"\"{adepartment.DepartmentName}\"添加成功"
            };

        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Department>>> GetDepartmentList(PaginationRequestViewModel pagination)
         => new ReturnViewModel<PaginationResponeViewModel<IEnumerable<Department>>>()
         {
             data = new PaginationResponeViewModel<IEnumerable<Department>>()
             {
                 list = db.Department.Skip(pagination.page).Take(pagination.limit),
                 total = db.Department.Count(),
             },
             code = (int)codes.Success,
         };

        /// <summary>
        /// 删一个
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        public ReturnViewModel<Department> DeleteDepartment(Department adepartment)
        {
            //先检查该部门下是否还有员工
            if (db.AspNetUsers.Where(x => x.DepartmentId == adepartment.DepartmentId) != null)
            {
                return new ReturnViewModel<Department>()
                {
                    code = (int)codes.DeleteDepartmentError,
                    data = adepartment,
                    message = $"\"{adepartment.DepartmentName}\"删除失败，该部门下还有员工"
                };
            };
            //没有才能删除
            try
            {
                var department = db.Department.Where(x => x.DepartmentId == adepartment.DepartmentId).First();
                db.Department.Remove(department);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnViewModel<Department>()
                {
                    code = (int)codes.DeleteDepartmentError,
                    data = adepartment,
                    message = $"\"{adepartment.DepartmentName}\"删除失败"
                };
            }
            return new ReturnViewModel<Department>()
            {
                code = (int)codes.Success,
                data = adepartment,
                message = $"\"{adepartment.DepartmentName}\"删除成功"
            };
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        public ReturnViewModel<Department> EditDepartment(Department adepartment)
        {
            try
            {
                var department = db.Department.Where(x => x.DepartmentId == adepartment.DepartmentId).First();
                department.DepartmentName = adepartment.DepartmentName;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnViewModel<Department>()
                {
                    code = (int)codes.EditDepartmentError,
                    data = adepartment,
                    message = $"\"{adepartment.DepartmentName}\"编辑失败"
                };
            }
            return new ReturnViewModel<Department>()
            {
                code = (int)codes.Success,
                data = adepartment,
                message = $"\"{adepartment.DepartmentName}\"编辑成功"
            };
        }

        /// <summary>
        /// 不分页获取所有部门
        /// 用于列表展示
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<Department>> GetAllDepartments() => new ReturnViewModel<IEnumerable<Department>>
        {
            code = (int)codes.Success,
            data = db.Department
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService
{
    public interface IDepartment
    {
        /// <summary>
        /// 分页及获取部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        ReturnViewModel<PaginationResponeViewModel<IEnumerable<Department>>> GetDepartmentList(PaginationRequestViewModel pagination);

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<Department>> GetAllDepartments();

        /// <summary>
        /// 添加一个部门
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        ReturnViewModel<Department> AddDepartment(Department adepartment);

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        ReturnViewModel<Department> DeleteDepartment(Department adepartment);

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        ReturnViewModel<Department> EditDepartment(Department adepartment);
    }
}

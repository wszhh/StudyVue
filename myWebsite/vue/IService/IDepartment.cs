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
        ReturnCMDViewModel<PaginationResponeViewModel<IEnumerable<Department>>> GetDepartmentList(PaginationRequestViewModel pagination);

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        ReturnCMDViewModel<IEnumerable<Department>> GetAllDepartments();

        /// <summary>
        /// 添加一个部门
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        ReturnCMDViewModel<Department> AddDepartment(Department adepartment);

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        ReturnCMDViewModel<Department> DeleteDepartment(Department adepartment);

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        ReturnCMDViewModel<Department> EditDepartment(Department adepartment);
    }
}

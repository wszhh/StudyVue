using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;

namespace vue.Controllers
{
    [Authorize(Policy = "CEO")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartment _department;
        public DepartmentController(IDepartment department)
        {
            _department = department;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public ReturnCMDViewModel<PaginationResponeViewModel<IEnumerable<Department>>> GetDepartmentList([FromBody]PaginationRequestViewModel pagination)
        {
            return _department.GetDepartmentList(pagination);
        }

        /// <summary>
        /// 添加一个部门
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnCMDViewModel<Department> AddDepartment([FromBody]Department adepartment)
        {
            return _department.AddDepartment(adepartment);
        }

        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnCMDViewModel<Department> DeleteDepartment([FromBody]Department adepartment)
        {
            return _department.DeleteDepartment(adepartment);
        }

        /// <summary>
        /// 编辑一个部门
        /// </summary>
        /// <param name="adepartment"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnCMDViewModel<Department> EditDepartment([FromBody]Department adepartment)
        {
            return _department.EditDepartment(adepartment);
        }
    }
}
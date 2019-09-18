using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [Authorize(Policy = "Admin")]
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
        public IActionResult GetDepartmentList([FromBody]PaginationRequestViewModel pagination)
        {
            var result = _department.GetDepartmentList(pagination);
            return Ok(new
            {
                code = codes.Success,
                data = new
                {
                    result.list,
                    result.total,
                },
            });
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
            return _department.DeleteDepartment(adepartment);
        }
    }
}
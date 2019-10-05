using System.Collections.Generic;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService
{
    public interface IClaim
    {

        /// <summary>
        /// 获取声明列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Claims> GetClaimList();

        /// <summary>
        /// 获取角色表
        /// </summary>
        /// <returns></returns>
        IEnumerable<RolesViewModel> Roles { get; }
    }
}

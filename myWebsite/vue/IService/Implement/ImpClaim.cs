using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService.Implement
{
    public class ImpClaim : IClaim
    {
        private HRCContext db = new HRCContext();

        /// <summary>
        /// 获取声明列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claims> ClaimList => db.Claims;


        /// <summary>
        /// 获取角色表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RolesViewModel> Roles => db.AspNetRoles.Select(x => new RolesViewModel() { id = x.Id, name = x.Name });
    }
}

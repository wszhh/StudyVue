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

        public IEnumerable<Claims> GetClaimList() => db.Claims;

        public IEnumerable<RolesViewModel> Roles => db.AspNetRoles.Select(x => new RolesViewModel() { name = x.Name });
    }
}

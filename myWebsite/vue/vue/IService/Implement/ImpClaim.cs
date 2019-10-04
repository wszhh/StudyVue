using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService.Implement
{
    public class ImpClaim : IClaim
    {
        private HRCContext db = new HRCContext();

        public IEnumerable<AspNetRoleClaims> GetClaimList()
        {
            return db.AspNetRoleClaims;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vue.DBModel;

namespace vue.IService.Implement
{
    public class ImpLeave : ILeave
    {
        private HRCContext db = new HRCContext();

        public List<Leave> GetLeave()
        {
            return db.Leave.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;
using vue.DBModel;

namespace vue.IService.Implement
{
    public class ImpStuList : IStuList
    {
        private HRCContext db = new HRCContext();
        public PaginationResponeViewModel<IEnumerable<StuList>> GetList(PaginationRequestViewModel pagination)
        {
            return new PaginationResponeViewModel<IEnumerable<StuList>>()
            {
                list = db.StuList.Skip(pagination.page).Take(pagination.limit),
                total = db.StuList.Count()
            };

        }
    }
}

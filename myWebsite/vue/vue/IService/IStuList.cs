using System.Collections.Generic;
using ViewModel;
using vue.DBModel;

namespace vue.IService
{
    public interface IStuList
    {
        PaginationResponeViewModel<IEnumerable<StuList>> GetList(PaginationRequestViewModel pagination);
    }
}

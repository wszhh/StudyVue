using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService
{
    public interface IClaim
    {

        ReturnCMDViewModel<List<TreeViewModel>> GetClaimList();
    }
}

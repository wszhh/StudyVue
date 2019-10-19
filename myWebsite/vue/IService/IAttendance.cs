using System.Collections.Generic;
using vue.DBModel;
using vue.ViewModel;
using static vue.Controllers.AttendanceController;

namespace vue.IService
{
    public interface IAttendance
    {

        /// <summary>
        /// 获取签到的CategoryItem
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<CategoryItems>> GetAttendanceCategory();


        ReturnViewModel<IEnumerable<signinModel>> GetSignInInfoById(string id);
    }
}

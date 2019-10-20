using System.Collections.Generic;
using vue.Areas.Identity.Data;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService
{
    public interface IAttendance
    {

        /// <summary>
        /// 获取签到的CategoryItem
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<CategoryItems>> GetAttendanceCategory();


        /// <summary>
        /// 是否已经签到过
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<bool> IsChecked(string Id);

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ReturnViewModel<bool> Checkin(NewUser user);
    }
}

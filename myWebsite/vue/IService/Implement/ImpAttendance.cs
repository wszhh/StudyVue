using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vue.Controllers;
using vue.DBModel;
using vue.ViewModel;
using static vue.Controllers.AttendanceController;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.IService.Implement
{
    public class ImpAttendance : IAttendance
    {
        private HRCContext db = new HRCContext();

        /// <summary>
        /// 获取签到的CategoryItem
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<CategoryItems>> GetAttendanceCategory() => new ReturnViewModel<IEnumerable<CategoryItems>>()
        {
            code = (int)codes.Success,
            data = db.CategoryItems.Where(x => x.CCategory == "Attendance"),
        };

        /// <summary>
        /// 获取签到
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<signinModel>> GetSignInInfoById(string id)
        {

            DateTime beginTime = DateTime.Parse(DateTime.Now.ToString().Substring(0, 7) + "/01");//本月初
            DateTime endTime = DateTime.Parse(beginTime.AddMonths(1).AddDays(-1).ToShortDateString());//本月最后一天
            List<signinModel> dateList = new List<signinModel>();
            for (DateTime dt = beginTime; dt <= endTime; dt = dt.AddDays(1))
            {
                dateList.Add(new signinModel { Date = dt, SignInType = 7 }); ;
            }
            var result = db.AttendanceSheet.Where(x => x.UserId == "100000002" && Convert.ToDateTime(x.ClockTime).Month == DateTime.Now.Month
              && Convert.ToDateTime(x.ClockTime).Year == DateTime.Now.Year).Select(x => new signinModel()
              {
                  Date = Convert.ToDateTime(x.ClockTime),
                  SignInType = Convert.ToInt32(x.AttendanceType)
              });

            return new ReturnViewModel<IEnumerable<signinModel>>() { code = (int)codes.Success, data = dateList.Union(result) };
        }
    }
}

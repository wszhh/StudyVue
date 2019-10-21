using System;
using System.Collections.Generic;
using System.Linq;
using vue.Areas.Identity.Data;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.IService.Implement
{
    public class ImpAttendance : IAttendance
    {
        private HRCContext db = new HRCContext();

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ReturnViewModel<bool> Checkin(NewUser user)
        {
            var time = DateTime.Now;
            db.AttendanceSheet.Add(new AttendanceSheet
            {
                UserId = user.Id,
                RealName = user.RealName,
                AttendanceStartTime = time.Date,
                DepartmentId = user.DepartmentId,
                ClockTime = time,
                AttendanceType = SigninModel.FormatType(time)
            });
            db.SaveChangesAsync();
            return new ReturnViewModel<bool>()
            {
                code = (int)codes.Success,
                message = "签到成功"
            };
        }





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
        /// 是否已经签到过
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<bool> IsChecked(string Id)
        {
            if (db.AttendanceSheet.Where(x => x.UserId == Id && x.ClockTime.Date == DateTime.Now.Date).FirstOrDefault() != null)
            {
                return new ReturnViewModel<bool>
                {
                    code = (int)codes.Success,
                    data = false
                };
            }
            return new ReturnViewModel<bool>
            {
                code = (int)codes.Success,
                data = true
            };
        }

        /// <summary>
        /// 获取签到
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ReturnViewModel<TwoSiginModel> GetSignInInfoById(string id)
        //{
        //    var aaa = db.AttendanceSheet;
        //    DateTime beginTime = DateTime.Parse(DateTime.Now.ToString().Substring(0, 7) + "-01");//本月初
        //    //DateTime endTime = DateTime.Parse(beginTime.AddMonths(1).AddDays(-1).ToShortDateString());//本月最后一天
        //    DateTime endTime = DateTime.Parse(DateTime.Now.ToShortDateString());//本月最后一天
        //    List<SigninModel> dateList = new List<SigninModel>();
        //    for (DateTime dt = beginTime; dt <= endTime; dt = dt.AddDays(1))
        //    {
        //        dateList.Add(new SigninModel { Date = dt, SignInType = 7 }); ;
        //    }
        //    var result = aaa.Where(x => x.UserId == id && x.ClockTime <= endTime
        //      && x.ClockTime >= beginTime);
        //    return new ReturnViewModel<TwoSiginModel>()
        //    {
        //        code = (int)codes.Success,
        //        data = new TwoSiginModel
        //        {
        //            DateList = dateList,
        //            Result = result.Select(x => new SigninModel()
        //            {
        //                Date = Convert.ToDateTime(x.ClockTime),
        //                SignInType = Convert.ToInt32(x.AttendanceType)
        //            })
        //        }
        //    };
        //}
    }
}

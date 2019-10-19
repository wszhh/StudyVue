using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using vue.Auth;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendance _attendance;
        private HRCContext db = new HRCContext();

        public AttendanceController(IAttendance attendance)
        {
            _attendance = attendance;
        }

        [HttpPost]
        public ReturnViewModel<IEnumerable<CategoryItems>> GetCategory()
        {
            return _attendance.GetAttendanceCategory();
        }


        /// <summary>
        /// 是否已经签到过
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<bool> IsChecked()
        {
            string id = GetIdByToken();
            if (db.AttendanceSheet.Where(x => x.UserId == id && Convert.ToDateTime(x.ClockTime).Date == DateTime.Now.Date).FirstOrDefault() != null)
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
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<bool> Checkin()
        {
            string id = GetIdByToken();
            var ischeck = IsChecked();
            if (!ischeck.data)
            {
                return new ReturnViewModel<bool>
                {
                    code = 23423,
                    message = "已经签到过"
                };
            }
            db.AttendanceSheet.Add(new AttendanceSheet { UserId = id, RealName = id, ClockTime = DateTime.Now });
            db.SaveChangesAsync();
            return new ReturnViewModel<bool>()
            {
                code = (int)codes.Success,
                message = "签到成功"
            };
        }


        /// <summary>
        /// 获取本月考勤信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<signinModel>> GetSignInInfo()
        {
            string id = GetIdByToken();
            if (!string.IsNullOrEmpty(id))
            {
                DateTime beginTime = DateTime.Parse(DateTime.Now.ToString().Substring(0, 7) + "/01");//本月初
                DateTime endTime = DateTime.Parse(beginTime.AddMonths(1).AddDays(-1).ToShortDateString());//本月最后一天
                List<signinModel> dateList = new List<signinModel>();
                for (DateTime dt = beginTime; dt <= endTime; dt = dt.AddDays(1))
                {
                    dateList.Add(new signinModel { Date = dt, SignInType = 7 }); ;
                }
                var result = db.AttendanceSheet.Where(x => x.UserId == id && Convert.ToDateTime(x.ClockTime).Month == DateTime.Now.Month
                  && Convert.ToDateTime(x.ClockTime).Year == DateTime.Now.Year).Select(x => new signinModel()
                  {
                      Date = Convert.ToDateTime(x.ClockTime),
                      SignInType = FormatType(Convert.ToInt32(x.AttendanceType), Convert.ToDateTime(x.ClockTime))
                  });
                return new ReturnViewModel<IEnumerable<signinModel>>() { code = (int)codes.Success, data = dateList.Union(result) };
            }
            return new ReturnViewModel<IEnumerable<signinModel>>()
            {
                code = (int)codes.AttendanceError,
                message = "获取失败"
            };
        }

        /// <summary>
        /// 根据签到时间格式化数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int FormatType(int type, DateTime time)
        {
            if (time.Hour >= 9)
            {
                return 2;
            }
            return type;
        }

        public class signinModel
        {
            public DateTime Date { get; set; }
            public int SignInType { get; set; }
        }


        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public string GetIdByToken()
        {
            return JwtHelper.SerializeJwt(Request.Headers["Authorization"].ToString().Substring(7)).ID;
        }


        /// <summary>
        /// 生成本月日期集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<signinModel> CalTime()
        {
            DateTime beginTime = DateTime.Parse(DateTime.Now.ToString().Substring(0, 7) + "-01");//本月初
            DateTime endTime = DateTime.Parse(beginTime.AddMonths(1).AddDays(-1).ToShortDateString());//本月最后一天
            List<signinModel> dateList = new List<signinModel>();
            for (DateTime dt = beginTime; dt <= endTime; dt = dt.AddDays(1))
            {
                dateList.Add(new signinModel { Date = dt, SignInType = 7 }); ;
            }
            return dateList;
        }
    }
}
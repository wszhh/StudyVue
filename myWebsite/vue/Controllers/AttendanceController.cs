using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vue.Areas.Identity.Data;
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
        private readonly UserManager<NewUser> _userManager;
        private HRCContext db = new HRCContext();

        public AttendanceController(UserManager<NewUser> userManager, IAttendance attendance)
        {
            _userManager = userManager;
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
        public ReturnViewModel<bool> IsChecked() => _attendance.IsChecked(GetIdByToken());


        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReturnViewModel<bool>> Checkin()
        {
            string id = GetIdByToken();
            var ischeck = IsChecked();
            if (!ischeck.data)
            {
                return new ReturnViewModel<bool>
                {
                    code = (int)codes.AttendanceError,
                    message = "今日已经签到过"
                };
            }
            return _attendance.Checkin(await _userManager.FindByIdAsync(id));
        }


        /// <summary>
        /// 获取本月考勤信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<SigninModel>> GetSignInInfo()
        {
            string id = GetIdByToken();
            if (!string.IsNullOrEmpty(id))
            {
                DateTime beginTime = DateTime.Parse(DateTime.Now.ToString().Substring(0, 7) + "-01");//本月初
                //DateTime endTime = DateTime.Parse(beginTime.AddMonths(1).AddDays(-1).ToShortDateString());//本月最后一天
                DateTime endTime = DateTime.Parse(DateTime.Now.ToShortDateString());//本月最后一天
                List<SigninModel> dateList = new List<SigninModel>();
                for (DateTime dt = beginTime; dt <= endTime; dt = dt.AddDays(1))
                {
                    dateList.Add(new SigninModel { Date = dt, SignInType = 7 }); ;
                }
                var result = db.AttendanceSheet.Where(x => x.UserId == id && x.ClockTime.Date <= endTime
                  && x.ClockTime.Date >= beginTime).Select(x => new SigninModel()
                  {
                      Date = x.ClockTime,
                      SignInType = FormatType(x.ClockTime)
                  });
                return new ReturnViewModel<IEnumerable<SigninModel>>()
                {
                    code = (int)codes.Success,
                    data = dateList.Union(result)

                };
            }
            return new ReturnViewModel<IEnumerable<SigninModel>>()
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
        public static int FormatType(DateTime time)
        {

            if (time.Hour >= 9)
            {
                return 2;
            }
            else if (time.Hour < 9)
            {
                return 1;
            }
            return 7;
        }




        public class SigninModel
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
        public IEnumerable<SigninModel> CalTime()
        {
            DateTime beginTime = DateTime.Parse(DateTime.Now.ToString().Substring(0, 7) + "-01");//本月初
            //DateTime endTime = DateTime.Parse(beginTime.AddMonths(1).AddDays(-1).ToShortDateString());//本月最后一天
            DateTime endTime = DateTime.Parse(DateTime.Now.ToShortDateString());//今天
            List<SigninModel> dateList = new List<SigninModel>();
            for (DateTime dt = beginTime; dt <= endTime; dt = dt.AddDays(1))
            {
                dateList.Add(new SigninModel { Date = dt, SignInType = 7 }); ;
            }
            return dateList;
        }
    }
}
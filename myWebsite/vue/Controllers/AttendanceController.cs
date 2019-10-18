using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        HRCContext db = new HRCContext();

        [HttpPost]
        public ReturnViewModel<IEnumerable<CategoryItems>> GetCategory()
        {

            var bb = db.CategoryItems.Where(x => x.CCategory == "Attendance").Select(x => x);
            return new ReturnViewModel<IEnumerable<CategoryItems>>()
            {
                code = (int)codes.Success,
                data = bb
            };

        }


        [HttpPost]
        public ReturnViewModel<IEnumerable<signinModel>> GetSignInInfo()
        {


            var aa = db.AttendanceSheet.Where(x => x.UserId == 100000001 && Convert.ToDateTime(x.ClockTime).Month == DateTime.Now.Month && Convert.ToDateTime(x.ClockTime).Year == DateTime.Now.Year).Select(x => new signinModel()
            {
                Date = Convert.ToDateTime(x.ClockTime),
                SignInType = Convert.ToInt32(x.AttendanceType)
            });


            List<signinModel> data = new List<signinModel>()
            {
                new signinModel{ Date=DateTime.Now.AddDays(-7) , SignInType=1},
                new signinModel{ Date=DateTime.Now.AddDays(-6) , SignInType=1},
                new signinModel{ Date=DateTime.Now.AddDays(-5) , SignInType=1},
                new signinModel{ Date=DateTime.Now.AddDays(-4), SignInType=1},
                new signinModel{ Date=DateTime.Now.AddDays(-3) , SignInType=1},
                //new signinModel{ Date=DateTime.Now.AddDays(-2) , SignInType=2},
                new signinModel{ Date=DateTime.Now.AddDays(-1) , SignInType=1},
                new signinModel{ Date=DateTime.Now , SignInType=4},
            };
            return new ReturnViewModel<IEnumerable<signinModel>>()
            {
                code = (int)codes.Success,
                data = data
            };
        }

        public class signinModel
        {
            public DateTime Date { get; set; }
            public int SignInType { get; set; }
        }
    }
}
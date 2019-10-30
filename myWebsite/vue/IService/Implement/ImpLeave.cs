using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;
using vue.Areas.Identity.Data;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.IService.Implement
{
    public class ImpLeave : ILeave
    {
        private readonly HRCContext db = new HRCContext();

        /// <summary>
        /// 申请请假
        /// </summary>
        /// <param name="leave"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ReturnViewModel<bool> AddLeave(LeaveViewModel leave, NewUser user)
        {
            try
            {
                DateTime LeaveStartTime = Convert.ToDateTime(leave.LeaveTimeSpan.First());
                DateTime LeaveEndTime = Convert.ToDateTime(leave.LeaveTimeSpan.Last());
                TimeSpan days = LeaveEndTime - LeaveStartTime;
                Leave aleave = new Leave
                {
                    UserId = user.Id,
                    RealName = user.RealName,
                    DepartmentId = user.DepartmentId,
                    LeaveStartTime = LeaveStartTime,
                    LeaveEndTime = LeaveEndTime,
                    LeaveReason = leave.LeaveReason,
                    LeaveTime = leave.LeaveTime,
                    LeaveHalfDay = leave.LeaveHalfDay,
                    LeaveState = 3,
                    LeaveDays = days.Days
                };
                db.Leave.Add(aleave);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnViewModel<bool>
                {
                    code = (int)codes.ApplyLeaveError,
                    data = false,
                    message = "申请失败，请检查是否填写正确"
                };
            }
            return new ReturnViewModel<bool>
            {
                code = (int)codes.Success,
                data = true,
                message = "申请成功"
            };

        }

        /// <summary>
        /// 获取请假审批表
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetCheckLeaves(PaginationRequestViewModel pagination)
        {
            var data = db.Leave.Where(x => x.LeaveState == 3);
            return new ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>>
            {
                code = (int)codes.Success,
                data = new PaginationResponeViewModel<IEnumerable<Leave>>
                {
                    list = data.Skip(pagination.page).Take(pagination.limit),
                    total = data.Count()
                }
            };
        }

        /// <summary>
        /// 基于用户id获取请假表
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetLeavesById(PaginationRequestViewModel pagination, string id)
        {
            var data = db.Leave.Where(x => x.UserId == id);
            return new ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>>
            {
                code = (int)codes.Success,
                data = new PaginationResponeViewModel<IEnumerable<Leave>>
                {
                    list = data.Skip(pagination.page).Take(pagination.limit),
                    total = data.Count()
                }
            };
        }

        /// <summary>
        /// 获取请假表
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetLeaves(PaginationRequestViewModel pagination)
        {
            return new ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>>
            {
                code = (int)codes.Success,
                data = new PaginationResponeViewModel<IEnumerable<Leave>>
                {
                    list = db.Leave.OrderByDescending(x => x.LeaveTime).Skip(pagination.page).Take(pagination.limit),
                    total = db.Leave.Count()
                }
            };
        }

        /// <summary>
        /// 获取签到的CategoryItem
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<CategoryItems>> GetLeaveStartCategory() => new ReturnViewModel<IEnumerable<CategoryItems>>
        {
            code = (int)codes.Success,
            data = db.CategoryItems.Where(x => x.CCategory == "LeaveStart"),
        };

        /// <summary>
        /// 审批请假
        /// </summary>
        /// <param name="leave"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ReturnViewModel<bool> CheckLeave(Leave leave, NewUser user)
        {
            try
            {
                var result = db.Leave.Find(leave.LeaveId);
                result.ApproverId = user.Id;
                result.ApproverReason = leave.ApproverReason;
                result.LeaveState = leave.LeaveState;
                result.ApprovalTime = leave.ApprovalTime;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return new ReturnViewModel<bool>
                {
                    code = (int)codes.ApplyLeaveError,
                    data = false,
                    message = "审批失败，请检查是否填写正确"
                };
            }
            return new ReturnViewModel<bool>
            {
                code = (int)codes.Success,
                data = true,
                message = "审批成功"
            };
        }
    }
}

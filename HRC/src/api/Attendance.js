import request from '@/utils/request'

export function GetSignInInfo(data) {
    return request({
        url: '/Attendance/GetSignInInfo',
        method: 'post',
        data
    })
}


export function GetCategory(data) {
    return request({
        url: '/Attendance/GetCategory',
        method: 'post',
        data
    })
}


export function IsChecked(data) {
    return request({
        url: '/Attendance/IsChecked',
        method: 'post',
        data
    })
}

export function Checkin(data) {
    return request({
        url: '/Attendance/Checkin',
        method: 'post',
        data
    })
}


//check attendance
export function GetAttendances(data) {
    return request({
        url: '/Attendance/GetAttendances',
        method: 'post',
        data
    })
}


export function FormatAttendanceType(data) {
    switch (data) {
      case "正常":
        return "success";
      case "未签到":
        return "danger";
      case "迟到":
        return "warning";
      case "请假":
        return "info";
    }
}
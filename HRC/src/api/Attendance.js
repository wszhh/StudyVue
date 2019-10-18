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

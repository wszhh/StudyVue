import request from '@/utils/request'

export function GetUserinfo(token) {
    return request({
        url: '/user/GetUserinfo',
        method: 'get',
        params: { token }
    })
}

export function SetUserInfo(data) {
    return request({
        url: '/user/SetUserInfo',
        method: 'post',
        data
    })
}

export function getColleaguesList(data) {
    return request({
        url: '/user/GetColleaguesList',
        method: 'post',
        data
    })
}

export function FindColleagueByName(data) {
    return request({
        url: '/user/FindColleagueByName',
        method: 'post',
        data
    })
}


//带排序 搜索
export function GetStaffListByOrder(data) {
    return request({
        url: '/user/GetStaffListByOrder',
        method: 'post',
        data
    })
}

export function SetStaffInfo(data) {
    return request({
        url: '/user/SetStaffInfo',
        method: 'post',
        data
    })
}

export function DeleteAStaff(data) {
    return request({
        url: '/user/DeleteAStaff',
        method: 'post',
        data
    })
}

export function GetStaffPhoto(data) {
    return request({
        url: '/user/GetStaffPhoto',
        method: 'post',
        data
    })
}
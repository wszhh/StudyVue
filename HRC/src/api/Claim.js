import request from '@/utils/request'

export function GetClaimTree(data) {
    return request({
        url: '/Claim/GetClaimTree',
        method: 'post',
        data
    })
}

export function GetUsersInRole(data) {
    return request({
        url: '/Claim/GetUsersInRole',
        method: 'post',
        data
    })
}

export function GetRoles(data) {
    return request({
        url: '/Claim/GetRoles',
        method: 'post',
        data
    })
}

export function SetRoleClaim(data) {
    return request({
        url: '/Claim/SetRoleClaim',
        method: 'post',
        data
    })
} 

export function SetRoleUsers(data) {
    return request({
        url: '/Claim/SetRoleUsers',
        method: 'post',
        data
    })
} 


import request from '@/utils/request'

export function getDepartmentList(data) {
    return request({
        url: '/Department/GetDepartmentList',
        method: 'post',
        data
    })
}

export function GetAllDepartments(data) {
    return request({
        url: '/Department/GetAllDepartments',
        method: 'post',
        data
    })
}

export function editDepartment(data) {
    return request({
        url: '/Department/EditDepartment',
        method: 'post',
        data
    })
}

export function addDepartment(data) {
    return request({
        url: '/Department/AddDepartment',
        method: 'post',
        data
    })
}

export function deleteADepartment(data) {
    return request({
        url: '/Department/deleteDepartment',
        method: 'post',
        data
    })
}


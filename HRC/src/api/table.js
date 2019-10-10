import request from '@/utils/request'


export function getList(params) {
  return request({
    url: '/Leave/GetLeaves',
    method: 'get',
    params
  })
}

export function getStuList(data) {
  return request({
    url: '/Home/GetStuList',
    method: 'post',
    data
  })
}

export function editStuList(data) {
  return request({
    url: '/Home/EditStuList',
    method: 'post',
    data
  })
}


import request from '@/utils/request'


export function getList(params) {
  return request({
    url: '/Leave/GetLeaves',
    method: 'get',
    params
  })
}




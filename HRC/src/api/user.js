import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/user/login',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/user/info',
    method: 'get',
    params: { token }
  })
}

export function logout() {
  return request({
    url: '/user/logout',
    method: 'post'
  })
}

export function getTime() {
  return request({
    url: '/home/gettime',
    method: 'get'
  })
}

export function refreshToken(oldToken) {
  return request({
    url: '/user/RefreshToken',
    method: 'get',
    params: { oldToken }
  })
}

export function ChangePassword(data) {
  return request({
    url: '/user/ChangePassword',
    method: 'post',
    data
  })
}

export function CheckUserName(data) {
  return request({
    url: '/user/CheckUserName',
    method: 'post',
    data
  })
}

export function AddStaff(data) {
  return request({
    url: '/user/AddStaff',
    method: 'post',
    data
  })
}

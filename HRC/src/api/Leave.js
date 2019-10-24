import request from '@/utils/request'

export function GetLeaves(data) {
  return request({
    url: '/Leave/GetLeavesById',
    method: 'post',
    data
  })
}

export function GetLeaveStartCategory(data) {
  return request({
    url: '/Leave/GetCategory',
    method: 'post',
    data
  })
}

export function AddLeave(data) {
  return request({
    url: '/Leave/AddLeave',
    method: 'post',
    data
  })
}

//check leave
export function GetCheckLeaves(data) {
  return request({
    url: '/Leave/GetCheckLeaves',
    method: 'post',
    data
  })
}

export function CheckLeave(data) {
  return request({
    url: '/Leave/CheckLeave',
    method: 'post',
    data
  })
}

export function GetAllLeaves(data) {
  return request({
    url: '/Leave/GetLeaves',
    method: 'post',
    data
  })
}


export function FormatLeaveType(data) {
  switch (data) {
    case 1:
      return "success";
    case 2:
      return "danger";
    case 3:
      return "warning";
  }
}


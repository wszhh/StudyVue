import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import { getToken, refreshToken } from '@/utils/auth'

// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 500000 // request timeout
  //为了后端调试就把时间改大
})


// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent

    if (store.getters.token) {
      // let each request carry token
      // ['X-Token'] is a custom headers key
      // please modify it according to the actual situation
      config.headers['Authorization'] = "Bearer " + getToken();
    }
    return config
  },
  error => {
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(err);
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
  */

  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data
    if (res.code == 20000 && res.message != null) {
      Message({
        message: res.message || '(Success)成功',
        type: 'success',
        duration: 5 * 1000
      })
    }
    // if the custom code is not 20000, it is judged as an error.
    if (res.code !== 20000) {
      Message({
        message: "(" + res.code + ")" + res.message || '(19999)未知错误',
        type: 'error',
        duration: 5 * 1000
      })

      // 50008: Illegal token; 50012: Other clients logged in; 50014: Token expired;
      if (res.code === 50008 || res.code === 50012 || res.code === 50014) {
        // to re-login
        MessageBox.confirm('您已被退出登录，您可以取消以停留在此页，或重新登录', 'Confirm logout', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('user/resetToken').then(() => {
          })
        })
      }
      return Promise.reject(new Error(res.message || 'Error'))
    } else {
      return res
    }
  },
  error => {
    if (error.response.status == 401) {
      // 直接将整个请求 return 出去，不然的话，请求会晚于当前请求，无法达到刷新操作 
      return store
        .dispatch("user/refreshToken", getToken())
        .then(() => {
          // Message({
          //   message: "令牌刷新成功",
          //   type: 'success',
          //   duration: 5 * 1000
          // })
          //error.config.isRetryRequest = true;
          //error.config.headers.Authorization = 'Bearer ' + getToken();
          return service(error.config);
        })
        .catch(() => {
          Message({
            message: error.message,
            type: 'error',
            duration: 5 * 1000
          })
          return Promise.reject(error)
        });
    } else if (error.response.status == 404) {
      //console.log('err' + error) // for debug
      Message({
        message: "(404)数据请求失败",
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    } else if (error.response.status == 403) {
      Message({
        message: "(403)没有执行该操作的权限",
        type: 'warning',
        duration: 5 * 1000
      })

    }

  }
);


// var tokenState = response.headers["token-expired"];
// if (tokenState) {
//   refreshToken(getToken()).then(response => {
//     const { data } = response
//     commit('SET_TOKEN', data.token)
//     setToken(data.token)
//   })
// }




// console.log('err' + error) // for debug
// Message({
//   message: error.message,
//   type: 'error',
//   duration: 5 * 1000
// })
// return Promise.reject(error)
// }
// )

export default service

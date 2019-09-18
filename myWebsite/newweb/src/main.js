import Vue from 'vue'
import App from './App.vue'
import router from './router'
import ElementUI from 'element-ui';
import axios from 'axios'
import 'element-ui/lib/theme-chalk/index.css';
import store from './store'
axios.defaults.baseURL = 'https://localhost:44305/api'
// 请求延时
axios.defaults.timeout = 20000
// 自定义判断元素类型JS
function toType(obj) {
  return {}.toString
    .call(obj)
    .match(/\s([a-zA-Z]+)/)[1]
    .toLowerCase();
}
// 参数过滤函数
function filterNull(o) {
  for (var key in o) {
    if (o[key] === null) {
      delete o[key];
    }
    if (toType(o[key]) === "string") {
      o[key] = o[key].trim();
    } else if (toType(o[key]) === "object") {
      o[key] = filterNull(o[key]);
    } else if (toType(o[key]) === "array") {
      o[key] = filterNull(o[key]);
    }
  }
  return o;
}

// http request 拦截器
axios.interceptors.request.use(
  config => {
    //var curTime = new Date()
    //var expiretime = new Date(Date.parse(storeTemp.state.tokenExpire))
    if (window.localStorage.Token && window.localStorage.Token.length >= 128) {//store.state.token 获取不到值？？
      // 判断是否存在token，如果存在的话，则每个http header都加上token
      config.headers.Authorization = "Bearer " + window.localStorage.Token;
      saveRefreshtime();
    }
    return config;
  },
  err => {
    return Promise.reject(err);
  }
);

axios.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    // 超时请求处理
    var originalRequest = error.config;
    if (error.code == 'ECONNABORTED' && error.message.indexOf('timeout') != -1 && !originalRequest._retry) {
      Vue.prototype.$message({
        message: '请求超时！',
        type: 'error'
      });
      originalRequest._retry = true
      return null;
    }
    if (error.response) {
      if (error.response.status == 401) {
        var curTime = new Date()
        var refreshtime = new Date(Date.parse(window.localStorage.refreshtime))
        // 在用户操作的活跃期内
        if (window.localStorage.refreshtime && (curTime <= refreshtime)) {
          return refreshToken({ token: window.localStorage.Token }).then((res) => {
            if (res.success) {
              Vue.prototype.$message({
                message: 'refreshToken success! loading data...',
                type: 'success'
              });
              var token = res.jwtStr.token;
              var expires_in = res.jwtStr.expires_in;
              store.commit("saveToken", token);
              var curTime = new Date();
              var expiredate = new Date(curTime.setSeconds(curTime.getSeconds() + expires_in));
              store.commit("saveTokenExpire", expiredate);
              error.config.__isRetryRequest = true;
              error.config.headers.Authorization = 'Bearer ' + token;
              return axios(error.config);
            } else {
              // 刷新token失败 清除token信息并跳转到登录页面
              ToLogin()
            }
          });
        } else {
          // 返回 401，并且不知用户操作活跃期内 清除token信息并跳转到登录页面
          ToLogin()
        }

      }
      // 403 无权限
      if (error.response.status == 403) {
        Vue.prototype.$message({
          message: '失败！该操作无权限',
          type: 'error'
        });
        return null;
      }
    }
    return ""; // 返回接口返回的错误信息
  }
);


export const refreshToken = params => {
  return axios.get('/Account/RefreshToken', { params: params }).then(res => res.data);
};


//刷新时间
export const saveRefreshtime = params => {
  let nowtime = new Date();
  //console.log("nowtime:" + nowtime);
  let lastRefreshtime = window.localStorage.refreshtime ? new Date(window.localStorage.refreshtime) : new Date(-1);
  //console.log("lastRefreshtime:" + lastRefreshtime)
  let expiretime = new Date(Date.parse(window.localStorage.TokenExpire))
  //console.log("expiretime:" + expiretime);
  //console.log("TokenExpire:" + window.localStorage.TokenExpire);
  //console.log("-----------------------------------------------------------");
  let refreshCount = 1;//滑动系数
  if (lastRefreshtime >= nowtime) {
    lastRefreshtime = nowtime > expiretime ? nowtime : expiretime;
    lastRefreshtime.setMinutes(lastRefreshtime.getMinutes() + refreshCount);
    window.localStorage.refreshtime = lastRefreshtime;
  } else {
    window.localStorage.refreshtime = new Date(-1);
  }
};



const ToLogin = params => {
  store.commit("saveToken", "");
  store.commit("saveTokenExpire", "");
  // store.commit("saveTagsData", "");
  // window.localStorage.removeItem('user');
  // window.localStorage.removeItem('NavigationBar');
  router.replace({
    path: "/",
    query: { redirect: router.currentRoute.fullPath }
  });
  //  window.location.reload()

};

Vue.config.productionTip = false
// 将API方法绑定到全局
Vue.prototype.$axios = axios
Vue.use(ElementUI);
Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')

import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

const store = new Vuex.Store({
  // 初始化的数据
  state: {
    formDatas: null,
    token: null,
    tokenExpire: null,
    tagsStoreList: [],
  },
  // 改变state里面的值得方法
  mutations: {
    getFormData(state, data) {
      state.formDatas = data;
    },
    saveToken(state, data) {
      state.token = data;
      window.localStorage.setItem("Token", data);//就是这里，保存到了 localStorage 中
    },
    saveTokenExpire(state, data) {
      state.tokenExpire = data;
      window.localStorage.setItem("TokenExpire", data);
    },
    saveTagsData(state, data) {
      state.tagsStoreList = data;
      sessionStorage.setItem("Tags", data)
    }
  }
});
// 输出模块
export default store;
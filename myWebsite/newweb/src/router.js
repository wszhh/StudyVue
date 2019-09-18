import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)
const router = new Router({
  mode: "history",
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/Home",
      name: "Home",
      component: () => import('./views/Home.vue'),
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      }
    },
    {
      path: "/",
      name: "Login",
      component: () => import('./views/Login.vue'),
      meta: { NoNeedHome: true }
    },
    {
      path: '/Register',
      name: 'Register',
      component: () => import('./views/Register.vue'),
      meta: { NoNeedHome: true }// 添加该字段，表示不需要home模板
    }
  ]
});
router.beforeEach((to, from, next) => {
  if (to.meta.requireAuth) {  // 判断该路由是否需要登录权限
    if (window.localStorage.Token && window.localStorage.Token.length >= 128) {  // 通过vuex state获取当前的token是否存在
      next();
    }
    else {
      next({
        path: '/',
        query: { redirect: to.fullPath }  // 将跳转的路由path作为参数，登录成功后跳转到该路由
      })
    }
  }
  else {
    next();
  }
})

export default router;




// export default new Router({
//   routes: [
//     {
//       path: '/',
//       name: 'login',
//       //component: login
//       component: () => import('./views/Login.vue'),
//     },
//     {
//       path: '/home',
//       name: 'Home',
//       // route level code-splitting
//       // this generates a separate chunk (about.[hash].js) for this route
//       // which is lazy-loaded when the route is visited.
//       component: () => import('./views/Home.vue'),
//       meta: {
//         requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
//       }
//     },
//     {
//       path: '/Register',
//       name: 'Register',
//       component: () => import('./views/Register.vue')
//     }
//   ]
// })

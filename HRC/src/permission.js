import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login', '/auth-redirect'] // no redirect whitelist

router.beforeEach(async (to, from, next) => {

  //开始进度条
  NProgress.start()

  //设置页面标题
  document.title = getPageTitle(to.meta.title)

  // 确定用户是否已登录
  const hasToken = getToken()

  if (hasToken) {
    if (to.path === '/login') {
      // 如果已登录，请重定向到主页
      next({ path: '/' })
      NProgress.done()
    } else {
      // 确定用户是否已通过getinfo获得其权限角色
      const hasRoles = store.getters.roles && store.getters.roles.length > 0
      if (hasRoles) {
        next()
      } else {
        try {
          // 获取用户信息
          // 注意：角色必须是数组！例如：['admin']或，['developer'，'editor']
          const { roles } = await store.dispatch('user/getInfo')
          //根据角色生成可访问的路由映射
          const accessRoutes = await store.dispatch('permission/generateRoutes', roles)
          //动态添加可访问路由
          router.addRoutes(accessRoutes)

          // 确保add routes完全加载的hack方法
          //设置replace:true，这样导航就不会留下历史记录
          next({ ...to, replace: true })
        } catch (error) {
          // 移除令牌并转到登录页以重新登录
          await store.dispatch('user/resetToken')
          Message.error(error || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  } else {
    /* 没有令牌*/

    if (whiteList.indexOf(to.path) !== -1) {
      // 在登录白名单中，直接进入
      next()
    } else {
      // 没有访问权限的其他页将重定向到登录页。
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  // 完成进度条
  NProgress.done()
})

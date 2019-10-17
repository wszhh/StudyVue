import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */



// //实例化vue的时候只挂载constantRouter
// export default new Router({
//   routes: constantRoutes
// });



//所有权限通用路由表 
//如首页和登录页和一些不用权限的公用页面
export const constantRoutes = [
  {
    path: '/login', component: () => import('@/views/login/index'),
    hidden: true,
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    name: '首页',
    children: [{
      path: 'dashboard',
      name: 'Dashboard',
      component: () => import('@/views/dashboard/index'),
      meta: { title: '首页', icon: 'dashboard', affix: true }
    }]
  },
  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  },
]



// const map = {
//   login: () => import('login/index')      // 异步的方式
// }

//异步挂载的路由
//动态需要根据权限加载的路由表 
export const asyncRoutes = [
  {
    path: '/StaffInfoManage',
    component: Layout,
    redirect: '/StaffInfoManage/index',
    name: 'StaffInfoManage',
    meta: { roles: ["MyInfo_Get", "Colleague_Get", "Staff_Get"], title: '员工资料管理', icon: 'user', noCache: true },
    alwaysShow: true,
    children: [
      {
        path: 'MyInfo',
        name: 'MyInfo',
        component: () => import('@/views/StaffInfoManage/MyInfo/index'),
        meta: { roles: ["MyInfo_Get"], title: '个人信息', icon: 'account-fill', noCache: true }
      },
      {
        path: 'FindColleagues',
        name: 'FindColleagues',
        component: () => import('@/views/StaffInfoManage/FindColleagues/index'),
        meta: { roles: ["Colleague_Get"], title: '查找同事', icon: 'search-fill', noCache: true }
      },
      {
        path: 'StaffManage',
        name: 'StaffManage',
        component: () => import('@/views/StaffInfoManage/StaffManage/index'),
        meta: { roles: ["Staff_Get"], title: '员工管理', icon: 'user-group-fill', noCache: true }
      }
    ]
  },

  {
    path: '/Claim',
    component: Layout,
    children: [{
      path: 'index',
      name: 'claim',
      component: () => import('@/views/claim/index'),
      meta: { roles: ["Claim_Get"], title: '权限管理', icon: 'key-fill', noCache: true }
    }]
  },

  {
    path: '/nested',
    component: Layout,
    redirect: '/nested/menu1',
    name: 'Nested',
    meta: {
      title: '薪资管理',
      icon: 'nested', noCache: true
    },
    children: [
      {
        path: 'menu1',
        component: () => import('@/views/nested/menu1/index'), // Parent router-view
        name: 'Menu1',
        meta: { title: 'Menu1' },
        children: [
          {
            path: 'menu1-1',
            component: () => import('@/views/nested/menu1/menu1-1'),
            name: 'Menu1-1',
            meta: { title: 'Menu1-1' }
          },
          {
            path: 'menu1-2',
            component: () => import('@/views/nested/menu1/menu1-2'),
            name: 'Menu1-2',
            meta: { title: 'Menu1-2' },
            children: [
              {
                path: 'menu1-2-1',
                component: () => import('@/views/nested/menu1/menu1-2/menu1-2-1'),
                name: 'Menu1-2-1',
                meta: { title: 'Menu1-2-1' }
              },
              {
                path: 'menu1-2-2',
                component: () => import('@/views/nested/menu1/menu1-2/menu1-2-2'),
                name: 'Menu1-2-2',
                meta: { title: 'Menu1-2-2' }
              }
            ]
          },
          {
            path: 'menu1-3',
            component: () => import('@/views/nested/menu1/menu1-3'),
            name: 'Menu1-3',
            meta: { title: 'Menu1-3' }
          }
        ]
      },
      {
        path: 'menu2',
        component: () => import('@/views/nested/menu2/index'),
        meta: { title: 'menu2' }
      }
    ]
  },
  {
    path: 'external-link',
    component: Layout,
    children: [
      {
        path: 'https://panjiachen.github.io/vue-element-admin-site/#/',
        meta: { title: '请假管理', icon: 'link' }
      }
    ]
  },
  {
    path: '/page',
    component: Layout,
    children: [
      {
        path: 'index',
        name: 'page',
        component: () => import('@/views/page/index'),
        meta: { title: '考勤管理', icon: 'form', noCache: true }
      }
    ]
  },
  {
    path: '/department',
    component: Layout,
    children: [
      {
        path: 'index',
        name: 'department',
        component: () => import('@/views/department/index'),
        meta: { roles: ["Department_Get"], title: '部门管理', icon: 'tree', noCache: true }
      }
    ]
  },
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  mode: 'history', //后端支持可开
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router

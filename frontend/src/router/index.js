import Vue from 'vue'
import VueRouter from 'vue-router'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import ClientRoleView from '../views/clientRole/ClientRoleView.vue'
import AdminRoleView from '../views/adminRole/AdminRoleView.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'login',
    component: LoginView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/register',
    name: 'register',
    component: RegisterView
  },
  {
    path: '/clientHome',
    name: 'clientHome',
    component: ClientRoleView
  },
  {
    path: '/adminHome',
    name: 'adminHome',
    component: AdminRoleView
  }
]

const router = new VueRouter({
  routes
})

export default router

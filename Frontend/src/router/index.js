import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'
import SignupView from '@/views/SignupView.vue'
import InterestsView from '@/views/InterestsView.vue'
import HomeView from '@/views/HomeView.vue'
import NotificationView from '@/views/NotificationView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/signup',
      name: 'signup',
      component: SignupView,
    },
    {
      path: '/interests',
      name: 'interests',
      component: InterestsView,
    },
    {
      path: '/home',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/',
      redirect: '/login'
    },
    {
    path: '/notifications',
    name: 'notifications',
    component: NotificationView
    },
  ],
})

export default router

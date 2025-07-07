import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
        path: '/login',
        name: 'login',
        component: LoginView
    },
    {
      path: '/groups/:id',
      name: 'GroupDetail',
      component: () => import('../views/GroupDetailView.vue')
    }
  ],
})

export default router

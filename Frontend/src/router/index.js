import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'
import SignupView from '@/views/SignupView.vue'
import InterestsView from '@/views/InterestsView.vue'
import HomeView from '@/views/HomeView.vue'
import CreatePostView from '../views/CreatePostView.vue'
import NotificationView from '@/views/NotificationView.vue'
import ModifyProfileView from '@/views/ModifyProfileView.vue'
import GroupDetailView from '@/views/GroupDetailView.vue'
import UserProfileView from '@/views/UserProfileView.vue'

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
      path: '/profile/modify',
      name: 'modify-profile',
      component: ModifyProfileView,
    },
    {
      path: '/',
      redirect: '/login',
    },
    {
    path: '/notifications',
    name: 'notifications',
    component: NotificationView
    },
    {
      path: '/group/:id/create-post',
      name: 'CreatePost',
      component: CreatePostView
    },
    {
      path: '/group/:id/',
      name: 'GroupDetail',
      component: GroupDetailView
    },
    {
      path: '/user/:id',
      name: 'UserProfile',
      component: UserProfileView
    },
  ],
})

export default router

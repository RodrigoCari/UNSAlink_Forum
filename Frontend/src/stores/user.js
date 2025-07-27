import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useUserStore = defineStore('user', () => {
  const token = ref(localStorage.getItem('token') || '')
  const userId = ref(localStorage.getItem('userId') || '')

  const isAuthenticated = computed(() => !!token.value && !!userId.value)

  function login(newToken, newUserId) {
    token.value = newToken
    userId.value = newUserId
    localStorage.setItem('token', newToken)
    localStorage.setItem('userId', newUserId)
  }

  function logout() {
    token.value = ''
    userId.value = ''
    localStorage.removeItem('token')
    localStorage.removeItem('userId')
  }

  return {
    token,
    userId,
    isAuthenticated,
    login,
    logout,
  }
})

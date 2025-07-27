<template>
  <header class="topbar">
    <SearchBar />
    <div class="actions">
      <button class="icon">💬</button>
      <button class="create-btn">+ Crear</button>
      <button class="icon">👤</button>

      <button
        v-if="!userStore.isAuthenticated"
        class="auth-btn"
        @click="goToLogin"
      >Iniciar sesión</button>

      <button
        v-else
        class="auth-btn"
        @click="logout"
      >Cerrar sesión</button>
    </div>
  </header>
</template>

<script setup>
import SearchBar from '@/components/SearchBar.vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'

const router = useRouter()
const userStore = useUserStore()

const goToLogin = () => {
  router.push('/login')
}

const logout = () => {
  userStore.logout()
  router.push('/home')
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap');

.topbar {
  position: sticky;
  top: 0;
  z-index: 10;
  margin-left: 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 60px;
  padding: 0 1.5rem;
  background-color: #f9fafb;
  border-bottom: 1px solid #e0e0e0;
  font-family: 'Roboto', sans-serif;
}

.actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.icon {
  background: transparent;
  border: none;
  font-size: 1.25rem;
  cursor: pointer;
}

.create-btn {
  background-color: #e5e7eb;
  border: none;
  padding: 0.4rem 1rem;
  border-radius: 20px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}

.create-btn:hover {
  background-color: #d1d5db;
}

.auth-btn {
  background-color: #4f46e5;
  color: white;
  border: none;
  padding: 0.4rem 1rem;
  border-radius: 20px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}

.auth-btn:hover {
  background-color: #3730a3;
}
</style>

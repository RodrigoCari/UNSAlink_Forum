<script setup>
import { RouterLink, useRouter } from 'vue-router'
import { computed } from 'vue'
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()
const router = useRouter()

const isAuthenticated = computed(() => userStore.isAuthenticated)
const userProfileLink = computed(() => `/user/${userStore.userId}`)

const goToCreateGroup = () => {
  router.push('/create-group')
}
</script>

<template>
  <div class="sidebar">
    <div class="logo">
      <img src="@/assets/satellite-icon.svg" alt="UNSAlink logo" class="logo-icon" />
      <span class="logo-text">UNSAlink</span>
    </div>

    <nav class="menu">
      <RouterLink to="/home" class="menu-item" active-class="active">🏠 Inicio</RouterLink>
      <RouterLink to="/explore" class="menu-item" active-class="active">🔍 Explorar</RouterLink>
      <RouterLink to="/notifications" class="menu-item" active-class="active">🔔 Notificaciones</RouterLink>

      <div class="menu-section">Todo</div>
      <div class="menu-item">👥 Grupos</div>
      <div class="menu-item">💬 Foros</div>

      <div class="menu-section">Comunidades</div>
      <div class="menu-item" @click="goToCreateGroup">➕ Crear una comunidad</div>

      <div class="menu-section">Acerca de</div>
      <div class="menu-item">❓ Ayuda</div>
      <div class="menu-item">🛠️ Herramientas</div>

      <div v-if="isAuthenticated" class="menu-section">Mi perfil</div>
      <RouterLink
        v-if="isAuthenticated"
        :to="userProfileLink"
        class="menu-item"
        active-class="active"
      >👤 Ver Perfil</RouterLink>
      <RouterLink
        v-if="isAuthenticated"
        to="/profile/modify"
        class="menu-item"
        active-class="active"
      >✏️ Modificar Perfil</RouterLink>
    </nav>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap');

.sidebar {
  position: fixed;
  top: 0;
  left: 0;
  width: 220px;
  height: 100vh;
  background-color: #2d718f;
  color: white;
  font-family: 'Roboto', sans-serif;
  padding: 1rem;
  overflow-y: auto;
  box-shadow: 2px 0 5px rgba(0, 0, 0, 0.1);
}
.logo {
  display: flex;
  align-items: center;
  margin-bottom: 2rem;
}

.logo-icon {
  width: 28px;
  height: 28px;
  margin-right: 0.5rem;
}

.logo-text {
  font-size: 1.5rem;
  font-weight: 700;
}

.menu {
  display: flex;
  flex-direction: column;
}

.menu-item {
  position: relative;
  padding: 0.5rem 0.75rem 0.5rem 1rem;
  color: white;
  text-decoration: none;
  font-size: 0.95rem;
  border-radius: 4px;
  transition: background 0.2s, padding-left 0.2s;
  cursor: pointer;
}

.menu-item:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.menu-item.active {
  background-color: rgba(255, 255, 255, 0.2);
  padding-left: 1.5rem;
}

.menu-item.active::before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  width: 4px;
  height: 100%;
  background-color: #fff;
  border-radius: 2px;
  transition: all 0.3s ease;
}

.menu-section {
  margin-top: 1.5rem;
  font-size: 0.8rem;
  color: #c2e2f0;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}
</style>

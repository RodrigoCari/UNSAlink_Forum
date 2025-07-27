<template>
    <div class="sidebar">
        <div class="logo">
            <img src="@/assets/satellite-icon.svg" alt="UNSAlink logo" class="logo-icon" />
            <span class="logo-text">UNSAlink</span>
        </div>
        <nav class="menu">
            <RouterLink to="/" class="menu-item">🏠 Inicio</RouterLink>
            <RouterLink to="/explore" class="menu-item">🔍 Explorar</RouterLink>
            <RouterLink to="/notifications" class="menu-item active">🔔 Notificaciones</RouterLink>

            <div class="menu-section">Todo</div>
            <div class="menu-item">👥 Grupos</div>
            <div class="menu-item">💬 Foros</div>

            <div class="menu-section">Comunidades</div>
            <div class="menu-item">➕ Crear una comunidad</div>

            <div class="menu-section">Acerca de</div>
            <div class="menu-item">❓ Ayuda</div>
            <div class="menu-item">🛠️ Herramientas</div>
        </nav>
    </div>
</template>

<script setup>
import { RouterLink } from 'vue-router'
</script>

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
  padding: 0.5rem 0;
  color: white;
  text-decoration: none;
  font-size: 0.95rem;
  border-radius: 4px;
  transition: background 0.2s;
  cursor: pointer;
}

.menu-item:hover {
  background-color: rgba(255, 255, 255, 0.1);
}
.menu-item.active {
  background-color: rgba(255, 255, 255, 0.2);
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

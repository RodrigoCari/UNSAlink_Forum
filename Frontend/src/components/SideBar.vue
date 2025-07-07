<template>
  <nav class="sidebar">
    <div class="logo">UNSAlink</div>
    <ul>
      <li><RouterLink to="/home">🏠 Inicio</RouterLink></li>
      <li><RouterLink to="/explore">🔍 Explorar</RouterLink></li>
      <li><RouterLink to="/notifications">🔔 Notificaciones</RouterLink></li>
      <li><RouterLink to="/groups">👥 Grupos</RouterLink></li>
      <li><RouterLink to="/create">➕ Crear comunidad</RouterLink></li>
    </ul>
    <div class="footer-links">
      <RouterLink to="/about">Acerca de</RouterLink>
      <RouterLink to="/help">Ayuda</RouterLink>
    </div>
  </nav>
</template>

<script setup>
import { RouterLink } from 'vue-router'
</script>

<style scoped>
.sidebar {
  width: 220px;
  background-color: #2d718f;
  height: 100vh;
  color: white;
  padding: 1rem;
  position: fixed;
  left: 0;
  top: 0;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.logo {
  font-weight: bold;
  font-size: 1.3rem;
  margin-bottom: 2rem;
}

.sidebar ul {
  list-style: none;
  padding: 0;
}

.sidebar li {
  margin: 1rem 0;
}

.sidebar a {
  color: white;
  text-decoration: none;
  font-weight: 500;
}

.sidebar a.router-link-active {
  font-weight: bold;
  text-decoration: underline;
}

.footer-links {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  font-size: 0.85rem;
  margin-top: auto;
}
</style>

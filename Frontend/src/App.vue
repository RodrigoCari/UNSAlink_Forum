<script setup>
  import SideBar from '@/components/SideBar.vue'
  import TopBar from '@/components/TopBar.vue'
  import { RouterView } from 'vue-router'
</script>

<template>
  <div class="app">
    <SideBar />
    <div class="main-content">
      <TopBar />
      <div class="router-wrapper">
        <RouterView />
      </div>
    </div>
  </div>
</template>

<style scoped>
.app {
  display: flex;
  height: 100vh;
  overflow: hidden; /* Mantén esto para evitar scroll lateral */
}

.main-content {
  margin-left: 220px;
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden; /* evita scroll doble */
}

/* Agrega este contenedor scrollable para el contenido */
.router-wrapper {
  flex: 1;
  overflow-y: auto;
  padding: 1rem 2rem;
}
</style>

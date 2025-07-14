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
      <RouterView />
    </div>
  </div>
</template>

<style scoped>
  .app {
    display: flex;
    height: 100vh;
    overflow: hidden;
  }

  .main-content {
    margin-left: 220px; /* mismo ancho que SideBar */
    flex: 1;
    display: flex;
    flex-direction: column;
  }
</style>
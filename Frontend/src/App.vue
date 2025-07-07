<script setup>
  import { RouterLink, RouterView } from 'vue-router'
  import SideBar from '@/components/SideBar.vue'
</script>

<template>
  <div class="app">
    <SideBar />
    <main class="main-content">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
  .app {
    display: flex;
    height: 100vh;
    overflow: hidden;
  }

  .main-content {
    margin-left: 220px;
    padding: 1rem;
    padding-top: 64px;
    background: #f5f5f5;
    flex: 1;
    overflow-y: auto;
  }
</style>

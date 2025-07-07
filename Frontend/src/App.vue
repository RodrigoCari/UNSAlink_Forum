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
  }

  .main-content {
      margin-left: 220px;
      padding: 1rem;
      flex: 1;
  }
</style>

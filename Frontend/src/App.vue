<script setup>
  import SideBar from '@/components/SideBar.vue'
  import TopBar from '@/components/TopBar.vue'
  import SearchBar from '@/components/SearchBar.vue'
import { RouterView } from 'vue-router'
</script>

<template>
  <div class="app">
    <SideBar />
    <div class="main-content">
      <SearchBar />
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
  margin-left: 220px; /* deja espacio para el sidebar */
  flex: 1;
  display: flex;
  flex-direction: column;
}
</style>

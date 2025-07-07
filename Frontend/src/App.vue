<script setup>
import { useRoute } from 'vue-router'
import Sidebar from '@/components/Sidebar.vue'

const route = useRoute()
const hideSidebarRoutes = ['/login']
</script>

<template>
  <div class="layout">
    <!-- Sidebar solo si no estás en login -->
    <Sidebar v-if="!hideSidebarRoutes.includes(route.path)" />

    <!-- Contenido de la vista -->
    <div class="content">
      <router-view />
    </div>
  </div>
</template>

<style>
.layout {
  display: flex;
}

.content {
  padding: 0rem;
  width: 100%;
}
</style>

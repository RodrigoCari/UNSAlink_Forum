<template>
  <div class="home-container">
    <h1>Grupos con su última publicación</h1>
    <div class="group-list">
      <div
        v-for="group in groups"
        :key="group.id"
        class="group-card"
        @click="goToGroup(group.id)"
      >
        <h2>{{ group.name }}</h2>
        <p>{{ group.description }}</p>
        <div v-if="group.latestPost">
          <h4>Última publicación: {{ group.latestPost.title }}</h4>
          <p>{{ group.latestPost.content }}</p>
          <small>🕒 {{ new Date(group.latestPost.createdAt).toLocaleString() }}</small>
        </div>
        <div v-else>
          <p><i>Este grupo aún no tiene publicaciones.</i></p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { fetchGroupsWithLatestPosts } from '../services/groupService'
import { useRouter } from 'vue-router'

const groups = ref([])
const router = useRouter()

onMounted(async () => {
  try {
    groups.value = await fetchGroupsWithLatestPosts()
  } catch (e) {
    console.error(e)
    alert('Error cargando los grupos.')
  }
})

function goToGroup(id) {
  router.push(`/group/${id}`)
}
</script>

<style scoped>
.home-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem 1rem;
  font-family: 'Segoe UI', sans-serif;
  color: #222;
}

.group-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1rem;
}

.group-card {
  border: 1px solid #ccc;
  padding: 1rem;
  border-radius: 0.5rem;
  cursor: pointer;
  background: #f9f9f9;
  transition: background 0.3s;
}

.group-card:hover {
  background: #f0f0f0;
}
</style>

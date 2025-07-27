<template>
  <div class="container">
    <!-- Línea 1 -->
    <div class="header-line">
      <span class="bold-text">Perfil</span>
      <span class="bold-text">Intereses</span>
    </div>

    <!-- Línea 2 -->
    <div class="subheader">
      <span class="bold-text">Perfil:</span>
      <span class="normal-text"> Información pública del usuario</span>
    </div>

    <!-- Línea 3 -->
    <div class="section-title">
      <span class="bold-text">General</span>
    </div>

    <!-- Contenido del perfil -->
    <div class="form-cards" v-if="user">
      <div class="card">
        <label>Nombre para mostrar</label>
        <div class="readonly-text">{{ user.name }}</div>
      </div>
      <div class="card">
        <label>Email</label>
        <div class="readonly-text">{{ user.email }}</div>
      </div>
      <div class="card" v-if="user?.interests?.length">
        <label>Intereses</label>
        <div class="readonly-text">{{ user.interests.join(', ') }}</div>
      </div>
    </div>

    <div v-else-if="error" class="error-message">
      <p>{{ error }}</p>
    </div>

    <div v-else>
      <p>Cargando perfil...</p>
    </div>

    <!-- Grupos -->
    <div class="section-title" style="margin-top: 2.5rem;">
      <span class="bold-text">Grupos a los que pertenece</span>
    </div>

    <div class="form-cards" v-if="groups.length">
      <div class="card" v-for="group in groups" :key="group.id">
        <label>{{ group.name }}</label>
        <div class="readonly-text">{{ group.description }}</div>
      </div>
    </div>

    <div v-else-if="!loadingGroups && user">
      <p>No se ha unido a ningún grupo.</p>
    </div>

    <!-- Publicaciones -->
    <div class="section-title" style="margin-top: 2.5rem;">
      <span class="bold-text">Publicaciones del usuario</span>
    </div>

    <div class="form-cards" v-if="posts.length">
      <div class="card" v-for="post in posts" :key="post.id">
        <label>{{ post.title }}</label>
        <div class="readonly-text">{{ post.content }}</div>
      </div>
    </div>

    <div v-else-if="!loadingPosts && user">
      <p>Este usuario no ha publicado nada aún.</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { fetchUserById } from '@/services/userService'
import { fetchGroupsByUser } from '@/services/groupService'
import { fetchPostsByUser } from '@/services/postService'

const route = useRoute()
const user = ref(null)
const groups = ref([])
const posts = ref([])

const error = ref(null)
const loadingGroups = ref(false)
const loadingPosts = ref(false)

const loadUser = async () => {
  try {
    const id = route.params.id
    user.value = await fetchUserById(id)

    loadingGroups.value = true
    groups.value = await fetchGroupsByUser(id)

    loadingPosts.value = true
    posts.value = await fetchPostsByUser(id)
  } catch (err) {
    error.value = err.message || 'No se pudo cargar el perfil'
  } finally {
    loadingGroups.value = false
    loadingPosts.value = false
  }
}

onMounted(loadUser)
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem 1rem;
  font-family: 'Segoe UI', sans-serif;
  color: #222;
}

.header-line {
  display: flex;
  gap: 2rem;
  margin-bottom: 1rem;
}

.subheader {
  margin-bottom: 2rem;
}

.section-title {
  margin-bottom: 1rem;
}

.bold-text {
  font-weight: bold;
  font-size: 1.2rem;
}

.normal-text {
  font-weight: normal;
  font-size: 1.1rem;
}

.form-cards {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.card {
  background-color: white;
  border-radius: 8px;
  padding: 1.2rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
  display: flex;
  flex-direction: column;
}

.card label {
  font-weight: 600;
  margin-bottom: 0.5rem;
}

.readonly-text {
  background-color: #f9f9f9;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
}

.error-message {
  color: red;
  font-weight: bold;
  margin-top: 1rem;
}
</style>

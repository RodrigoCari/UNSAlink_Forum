<template>
  <div class="create-post-wrapper">
    <div class="header">
      <button class="back-btn" @click="onCancel">←</button>
      <h2>Crear Publicación</h2>
    </div>

    <div class="badges">
      <span class="badge">Autor: {{ authorName }}</span>
    </div>

    <!-- Si es compartido -->
    <div v-if="isSharing && originalPost" class="form-group">
      <label class="label">Contenido original:</label>
      <div class="original-content-preview bg-gray-100 p-3 rounded">
        <p class="text-sm text-gray-600">{{ originalPost.content }}</p>
      </div>
    </div>

    <!-- Grupo al que se publicará -->
    <div class="form-group" v-if="isSharing">
      <label class="label">Compartir en:</label>
      <select v-model="form.groupId" class="input-field">
        <option v-for="g in userGroups" :key="g.id" :value="g.id">{{ g.name }}</option>
      </select>
    </div>

    <!-- Grupo fijo si no es compartido -->
    <div class="form-group" v-else>
      <span class="badge">Grupo: {{ groupName }}</span>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <!-- Tipo de publicación -->
    <div class="form-group">
      <select v-model="form.type" class="input-field">
        <option value="QA">Pregunta / Asesoría</option>
        <option value="EducationalContributions">Aporte educativo</option>
        <option value="Discussion">Discusión</option>
        <option value="News">Noticias</option>
        <option value="Tips">Consejos</option>
        <option value="Experiences">Experiencias</option>
        <option value="Requests">Solicitud</option>
        <option value="Opportunities">Oportunidades</option>
      </select>
    </div>

    <!-- Título -->
    <div class="form-group">
      <input v-model="form.title"
             type="text"
             placeholder="Título de la publicación…"
             class="input-field" />
    </div>

    <!-- Contenido solo si no es compartido -->
    <div class="form-group" v-if="!isSharing">
      <textarea v-model="form.content"
                placeholder="Contenido de la publicación…"
                class="textarea-field"></textarea>
    </div>

    <div class="buttons">
      <button class="btn-cancel" @click="onCancel">Cancelar</button>
      <button class="btn-submit" :disabled="isSubmitDisabled" @click="onSubmit">
        {{ loading ? 'Enviando…' : 'Subir' }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { fetchGroup, fetchGroupsByUser } from '../services/groupService'
import { fetchUserById } from '../services/userService'
import { createPost, getPostById, sharePost } from '../services/postService'
const typeMap = {
  QA: 0,
  EducationalContributions: 1,
  Discussion: 2,
  News: 3,
  Tips: 4,
  Experiences: 5,
  Requests: 6,
  Opportunities: 7,
  Shared: 100
};
const route = useRoute()
const router = useRouter()

const routeGroupId = route.params.id
const originalPostId = route.query.originalPostId || null
const isSharing = !!originalPostId

const token = localStorage.getItem('token')
const authorId = localStorage.getItem('userId')

const groupName = ref('…')
const authorName = ref('…')
const originalPost = ref(null)
const userGroups = ref([])
const error = ref('')
const loading = ref(false)

const form = reactive({
  title: '',
  content: '',
  authorId,
  groupId: isSharing ? '' : routeGroupId,
  type: 'QA'
})

const isSubmitDisabled = computed(() => loading.value || !form.title.trim())

onMounted(async () => {
  try {
    const u = await fetchUserById(authorId)
    authorName.value = u.name
  } catch {
    authorName.value = '—desconocido—'
  }

  if (isSharing) {
    try {
      const data = await getPostById(originalPostId)
      originalPost.value = data
      form.content = data.content
    } catch {
      error.value = 'Error al cargar el post original'
    }

    try {
      const res = await fetchGroupsByUser(authorId)
      userGroups.value = res
      if (res.length > 0) form.groupId = res[0].id
    } catch {
      error.value = 'No se pudieron cargar los grupos'
    }
  } else {
    try {
      const g = await fetchGroup(routeGroupId)
      groupName.value = g.name
    } catch {
      groupName.value = '—desconocido—'
    }
  }
})

async function onSubmit() {
  error.value = ''
  loading.value = true
  try {
    if (isSharing) {
      await sharePost({
        originalPostId,
        authorId,
        groupId: form.groupId,
        title: form.title,
        content: form.content,
        type: typeMap[form.type] ?? 0
      })
    } else {
      await createPost(form)
    }
    router.push(`/group/${form.groupId}`)
  } catch (e) {
    error.value = `Error al compartir publicación: ${e.message}`
  } finally {
    loading.value = false
  }
}

function onCancel() {
  router.back()
}
</script>

<style scoped>
  .create-post-wrapper {
    max-width: 600px;
    margin: 3rem auto;
    padding: 2rem;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 2px 12px rgba(0,0,0,0.1);
  }

  .header {
    display: flex;
    align-items: center;
    gap: 1rem;
  }

    .header h2 {
      flex: 1;
      margin: 0;
      font-size: 1.5rem;
    }

  .back-btn {
    background: none;
    border: none;
    font-size: 1.25rem;
    cursor: pointer;
  }

  .badges {
    display: flex;
    gap: 1rem;
    margin: 1rem 0;
  }

  .badge {
    background: #00bcd4;
    color: white;
    padding: 0.4rem 0.8rem;
    border-radius: 9999px;
    font-size: 0.9rem;
  }

  .error {
    color: #c00;
    margin-bottom: 1rem;
  }

  .form-group {
    margin-bottom: 1.25rem;
  }

  .input-field,
  .textarea-field {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
  }

  .textarea-field {
    min-height: 150px;
    resize: vertical;
  }

  .buttons {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1.5rem;
  }

  .btn-cancel {
    background: #f5f5f5;
    border: none;
    padding: 0.6rem 1.2rem;
    border-radius: 4px;
    cursor: pointer;
  }

  .btn-submit {
    background: #00bcd4;
    color: white;
    border: none;
    padding: 0.6rem 1.2rem;
    border-radius: 4px;
    cursor: pointer;
  }

    .btn-submit:disabled {
      background: #90e0eb;
      cursor: not-allowed;
    }
</style>
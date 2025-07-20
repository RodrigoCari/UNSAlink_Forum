<template>
  <div class="create-post-wrapper">
    <!-- Header -->
    <div class="header">
      <button class="back-btn" @click="onCancel">←</button>
      <h2>Crear Publicación</h2>
    </div>

    <!-- Grupo y Autor -->
    <div class="badges">
      <span class="badge">Grupo: {{ groupName }}</span>
      <span class="badge">Autor: {{ authorName }}</span>
    </div>

    <!-- Error general -->
    <div v-if="error" class="error">{{ error }}</div>

    <!-- Título -->
    <div class="form-group">
      <input v-model="form.title"
             type="text"
             placeholder="Título de la publicación…"
             class="input-field" />
    </div>

    <!-- Contenido -->
    <div class="form-group">
      <textarea v-model="form.content"
                placeholder="Contenido de la publicación…"
                class="textarea-field"></textarea>
    </div>

    <!-- Botones -->
    <div class="buttons">
      <button class="btn-cancel" @click="onCancel">Cancelar</button>

      <button class="btn-submit"
              v-if="isSubmitDisabled"
              disabled
              @click="onSubmit">
        {{ loading ? 'Enviando…' : 'Subir' }}
      </button>
      <button class="btn-submit"
              v-else
              @click="onSubmit">
        {{ loading ? 'Enviando…' : 'Subir' }}

      </button>
    </div>
  </div>
</template>

<script setup>
  import { ref, reactive, onMounted, computed } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { fetchGroup } from '../services/groupService'
  import { fetchUserById } from '../services/userService'
  import { createPost } from '../services/postService'

  const route = useRoute()
  const router = useRouter()
  const groupId = route.params.id

  const groupName = ref('…')

  const token = localStorage.getItem('token')
  const authorId = localStorage.getItem('userId')

  const authorName = ref('…')
  const error = ref('')
  const loading = ref(false)

  const form = reactive({
    title: '',
    content: '',
    authorId,
    groupId,
    type: 0    // Discusion por defecto
  })

  const isSubmitDisabled = computed(() => loading.value || !form.title.trim())

  onMounted(async () => {
    try {
      const g = await fetchGroup(groupId)
      groupName.value = g.name
    } catch {
      groupName.value = '—desconocido—'
    }
    try {
      const u = await fetchUserById(authorId)
      authorName.value = u.name
    } catch {
      authorName.value = '—desconocido—'
    }
  })

  async function onSubmit() {
    error.value = ''
    loading.value = true
    try {
      await createPost(form)
      router.push(`/group/${groupId}`)
    } catch (e) {
      error.value = e.message
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

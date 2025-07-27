<template>
  <div class="create-group-page">
    <div class="header">
      <button class="back-btn" @click="onCancel">←</button>
      <h1>Cuéntanos sobre tu comunidad</h1>
      <p class="subtitle">
        Un nombre y una descripción ayudan a las personas a comprender de qué se trata su comunidad.
      </p>
    </div>

    <div class="content">
      <!-- Formulario a la izquierda -->
      <form class="form" @submit.prevent="onSubmit">
        <div class="field">
          <label for="name">Nombre de la comunidad *</label>
          <input id="name"
                 v-model="form.name"
                 type="text"
                 placeholder="Nombre de la comunidad"
                 required />
        </div>

        <div class="field">
          <label for="description">Descripción *</label>
          <textarea id="description"
                    v-model="form.description"
                    placeholder="Descripción de tu comunidad"
                    required></textarea>
        </div>

        <div class="buttons">
          <button type="button" class="btn-cancel" @click="onCancel">
            Cancelar
          </button>
          <button type="submit" class="btn-create">
            Crear
          </button>
        </div>
      </form>

      <!-- Vista previa en vivo a la derecha -->
      <div class="preview-card">
        <h2>{{ preview.name || 'Nombre de la comunidad' }}</h2>
        <p class="stats">

        </p>
        <p class="desc">
          {{ preview.description || 'Descripción de tu comunidad' }}
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { reactive, computed } from 'vue'
  import { useRouter } from 'vue-router'
  import { createGroup } from '@/services/groupService'

  const router = useRouter()

  const token = localStorage.getItem('token')
  const adminId = localStorage.getItem('userId')

  const form = reactive({
    name: '',
    description: '',
    adminId: adminId // si tuvieras token, lo pones aquí
  })

  // Datos estáticos para la preview
  const preview = reactive({
    get name() {
      return form.name.trim()
    },
    get description() {
      return form.description.trim()
    }
  })

  function onCancel() {
    router.back()
  }

  async function onSubmit() {
    try {
      await createGroup({
        name: form.name,
        description: form.description,
        adminId: form.adminId
      })
      router.push('/Group') // ruta a la lista
    } catch (err) {
      console.error(err)
      alert('Error al crear la comunidad: ' + err.message)
    }
  }
</script>

<style scoped>
  .create-group-page {
    padding: 2rem;
  }

  .header {
    margin-bottom: 2rem;
  }

  .back-btn {
    background: none;
    border: none;
    font-size: 1.25rem;
    cursor: pointer;
  }

  .header h1 {
    margin: 0.5rem 0;
    font-size: 2rem;
    font-weight: 600;
  }

  .subtitle {
    color: #666;
    margin: 0;
    font-size: 1rem;
  }

  .content {
    display: flex;
    gap: 2rem;
  }

  .form {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
  }

  .field label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 500;
  }

  .field input,
  .field textarea {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #ccc;
    border-radius: 8px;
    font-size: 1rem;
    resize: vertical;
  }

  .buttons {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
  }

  .btn-cancel {
    background: #f0f0f0;
    border: none;
    padding: 0.6rem 1.2rem;
    border-radius: 20px;
    cursor: pointer;
  }

  .btn-create {
    background: #00bcd4;
    color: white;
    border: none;
    padding: 0.6rem 1.2rem;
    border-radius: 20px;
    cursor: pointer;
  }

  .preview-card {
    flex: 1;
    background: #fff;
    border-radius: 16px;
    padding: 1.5rem;
    box-shadow: 0 4px 12px rgba(0,0,0,0.05);
  }

    .preview-card h2 {
      margin: 0 0 0.5rem;
      font-size: 1.5rem;
    }

  .stats {
    margin: 0 0 1rem;
    color: #888;
    font-size: 0.9rem;
  }

  .desc {
    margin: 0;
    font-size: 1rem;
    line-height: 1.4;
  }
</style>

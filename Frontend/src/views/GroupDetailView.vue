<template>
  <div class="page-wrapper">
    <!-- Título y iconos back/home -->
    <div class="page-header">
      <div class="nav-icons">
        <router-link to="/">
          <img :src="group.imageUrl"
               alt="Home icon"
               class="icon" />
        </router-link>
        <router-link :to="previousRoute">
          <img :src="group.imageUrl"
               alt="Back icon"
               class="icon" />
        </router-link>
      </div>
      <h1 class="page-title">{{ group.name }}</h1>
    </div>

    <!-- Botones de interacción -->
    <div class="action-bar">
      <button class="btn primary">+ Crear publicación</button>
      <button class="btn">Unirse</button>
      <button class="btn icon-only">
        <img :src="group.imageUrl"
             alt="Más opciones"
             class="icon" />
      </button>
    </div>

    <!-- Contenido principal y panel derecho -->
    <div class="content-grid">
      <!-- Contenido principal -->
      <div class="post-card">
        <div class="post-header">
          <small class="meta">
            u/{{ group.author }} | {{ group.date }}
          </small>
          <p class="post-content">{{ group.content }}</p>
        </div>
        <div class="post-image">
          <img :src="group.imageUrl"
               alt="Post " />
        </div>
        <div class="post-actions">
          <button class="icon-btn">⬆️ <span>{{ group.likes }}</span></button>
          <button class="icon-btn">💬 <span>{{ group.comments }}</span></button>
          <button class="icon-btn">🔗 <span>{{ group.shareCount }}</span></button>
          <button class="btn join" v-bind:disabled="joined" @click="onJoin">
            {{ joined ? 'Ya eres miembro' : 'Unirse' }}
          </button>
        </div>
      </div>

      <!-- Panel derecho -->
      <div class="group-panel">
        <h2 class="panel-title">{{ group.groupInfo.title }}</h2>
        <p class="panel-desc">{{ group.groupInfo.description }}</p>
        <div class="panel-meta">
          <div>🔍 Creada el {{ group.groupInfo.created }}</div>
          <div>🌐 {{ group.groupInfo.visibility }}</div>
        </div>
        <div class="panel-stats">
          <div>{{ group.groupInfo.members }} Members</div>
          <div>{{ group.groupInfo.online }} En línea</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted, computed } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useGroupStore } from '../stores/group'
  import { fetchGroup, joinGroup } from '../services/groupService'

  const route = useRoute()
  const router = useRouter()
  // anterior
  const previousRoute = router.options.history.state.back || '/'

  const id = route.params.id
  const store = useGroupStore()
  const joined = computed(() => store.joined)

  // Inicializar objeto
  const group = ref({
    name: 'Cargando…',
    author: '',
    date: '',
    content: '',
    imageUrl: '',
    likes: 0,
    comments: 0,
    shareCount: 0,
    groupInfo: {
      title: '',
      description: '',
      created: '',
      visibility: '',
      members: 0,
      online: 0
    }
  })

  onMounted(async () => {
    const data = await fetchGroup(id)
    if (data) group.value = data
  })

  async function onJoin() {
    await joinGroup(id)
    store.joined = true
  }
</script>

<style scoped>
  .page-wrapper {
    padding: 1rem 2rem;
  }

  .page-header {
    display: flex;
    align-items: center;
    gap: 1rem;
    margin-bottom: 1rem;
  }

  .nav-icons .icon {
    width: 24px;
    height: 24px;
    cursor: pointer;
    border: 1px dashed #999;
  }

  .page-title {
    font-size: 1.75rem;
    font-weight: bold;
    margin: 0;
  }

  .action-bar {
    display: flex;
    gap: 0.75rem;
    margin-bottom: 1.5rem;
  }

  .btn {
    padding: 0.5rem 1.25rem;
    border: none;
    border-radius: 9999px;
    background: #000;
    color: #fff;
    cursor: pointer;
    font-size: 0.95rem;
  }

    .btn.primary {
      background: #000;
    }

    .btn:not(.primary) {
      background: #333;
    }

    .btn.icon-only {
      padding: 0.5rem;
      background: #333;
    }

  .content-grid {
    display: grid;
    grid-template-columns: 1fr 300px;
    gap: 1.5rem;
  }

  .post-card {
    background: #f0f0f0;
    border-radius: 1rem;
    padding: 1.5rem;
  }

  .post-header .meta {
    color: #666;
    font-size: 0.85rem;
  }

  .post-content {
    margin: 0.75rem 0 1rem;
    line-height: 1.4;
    color: #444;
  }

  .post-image img {
    width: 100%;
    border-radius: 0.75rem;
    object-fit: cover;
    background: #ddd;
    min-height: 150px;
  }

  .post-actions {
    display: flex;
    align-items: center;
    gap: 1rem;
    margin-top: 1rem;
  }

  .icon-btn {
    background: none;
    border: none;
    cursor: pointer;
    font-size: 1rem;
    display: flex;
    align-items: center;
  }

    .icon-btn span {
      margin-left: 0.25rem;
      font-weight: bold;
    }

  .btn.join {
    margin-left: auto;
    background: #000;
    color: #fff;
    padding: 0.5rem 1rem;
  }

  .group-panel {
    background: #fff;
    border-radius: 1rem;
    padding: 1.5rem;
  }

  .panel-title {
    margin: 0 0 0.5rem;
    font-size: 1.25rem;
    font-weight: bold;
  }

  .panel-desc {
    margin-bottom: 1rem;
    color: #555;
    line-height: 1.4;
  }

  .panel-meta,
  .panel-stats {
    display: flex;
    gap: 1rem;
    font-size: 0.9rem;
    color: #666;
    margin-bottom: 0.5rem;
  }
</style>

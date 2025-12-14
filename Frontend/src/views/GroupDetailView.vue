<template>
  <div v-if="group" class="page-wrapper">
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
      <small class="meta">
        🔍 Creada el {{ formattedDate }}
      </small>
    </div>

    <!-- Botones de interacción -->
    <div class="action-bar">
      <button class="btn primary" @click="onCreatePost">+ Crear</button>
      <button class="btn" :disabled="joined" @click="onJoin">
        {{ joined ? 'Ya eres miembro' : 'Unirse' }}
      </button>
      <button class="btn icon-only">
        <img :src="group.imageUrl"
             alt="Más opciones"
             class="icon" />
      </button>
    </div>

    <!-- Contenido principal y panel derecho -->
    <div class="content-grid">
      <!-- Contenido principal -->
      <div class="post-list">
        <div v-for="post in posts" :key="post.id" class="post-card">
          <div class="post-header">
            <small class="meta">
              u/{{ post.authorName || 'desconocido' }} | {{ new Date(post.createdAt).toLocaleDateString('es-ES') }}
            </small>
            <div class="post-tag">
              {{ formatType(post.type) }}
            </div>
            <h3 style="margin: 0.25rem 0">{{ post.title }}</h3>
            <p class="post-content">{{ post.content }}</p>
          </div>
          <div class="post-actions">
            <button class="icon-btn">⬆️</button>
            <button class="icon-btn" @click="() => (activePostId = post.id)">💬</button>
            <button class="icon-btn" @click="() => sharePost(post.id)">🔗 Compartir</button>
          </div>

          <!-- Caja de comentario -->
          <div v-if="activePostId === post.id" class="comment-box">
            <textarea
              v-model="commentInputs[post.id]"
              placeholder="Escribe un comentario..."
              class="comment-input"
            ></textarea>
            <div class="comment-actions">
              <button @click="() => (activePostId = null)">Cancelar</button>
              <button @click="() => addComment(post)">Comentar</button>
            </div>

            <!-- Lista de comentarios -->
            <div class="comments-list" v-if="post.comments?.length">
              <div
                v-for="comment in post.comments"
                :key="comment.id"
                class="comment-card"
              >
                <small class="meta">
                  u/{{ comment.author || 'Invitado' }} • {{ new Date(comment.createdAt).toLocaleDateString('es-ES') }}
                </small>
                <p class="comment-content">{{ comment.content }}</p>
                <div class="vote-bar">
                  <button @click="() => vote(comment, 1)">⬆️</button>
                  <span>{{ comment.score }}</span>
                  <button @click="() => vote(comment, -1)">⬇️</button>
                  <button @click="() => toggleReply(comment.id)">💬</button>
                </div>

                <!-- Caja de respuesta -->
                <div v-if="activeReplyBox === comment.id" class="reply-box">
                  <textarea
                    v-model="replyInputs[comment.id]"
                    placeholder="Escribe una respuesta..."
                    class="comment-input"
                  ></textarea>
                  <div class="comment-actions">
                    <button @click="() => (activeReplyBox = null)">Cancelar</button>
                    <button @click="() => replyTo(comment)">Responder</button>
                  </div>
                </div>

                <!-- Subcomentarios -->
                <div class="replies" v-if="comment.replies?.length">
                  <div
                    v-for="reply in comment.replies"
                    :key="reply.id"
                    class="comment-card reply"
                  >
                    <p class="comment-content">{{ reply.content }}</p>
                    <div class="vote-bar">
                      <button @click="() => vote(reply, 1)">⬆️</button>
                      <span>{{ reply.score }}</span>
                      <button @click="() => vote(reply, -1)">⬇️</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Panel derecho -->
      <div class="group-panel">
        <h2 class="panel-title">{{ group.groupInfo.title }}</h2>
        <p class="panel-desc">{{ group.groupInfo.description }}</p>
        <div class="panel-meta">
          <div>🔍 Creada el {{ formattedDate }}</div>
          <div>🌐 {{ group.groupInfo.visibility }}</div>
        </div>
        <div class="panel-stats">
          <div>{{ group.groupInfo.members }} Members</div>
          <div>{{ group.groupInfo.online }} En línea</div>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="page-wrapper">
    <p class="loading">Cargando…</p>
  </div>
</template>

<script setup>
  import { ref, onMounted, computed } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useGroupStore } from '../stores/group'
  import { fetchJoinGroup, joinGroup } from '../services/joinGroupService'
  import { fetchGroupPosts, addComment as sendComment } from '../services/postService';

  const route = useRoute()
  const router = useRouter()
  const previousRoute = router.options.history.state.back || '/'

  const id = route.params.id
  const store = useGroupStore()
  const joined = computed(() => store.joined)

  const group = ref({
    name: '',
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
  const posts = ref([])
  const formattedDate = computed(() => {
    const d = group.value.groupInfo.created || group.value.date
    return d
      ? new Date(d).toLocaleDateString('es-ES', { day: 'numeric', month: 'long', year: 'numeric' })
      : 'Fecha desconocida'
  })
  const commentInputs = ref({})
  const replyInputs = ref({})
  const activePostId = ref(null)
  const activeReplyBox = ref(null)

  function toggleReply(commentId) {
    activeReplyBox.value = activeReplyBox.value === commentId ? null : commentId
  }

  function vote(comment, value) {
    comment.score = (comment.score || 0) + value
  }

  

  async function addComment(post) {
    const input = commentInputs.value[post.id];
    if (!input) return;

    try {
        const author = localStorage.getItem('username') ?? 'Invitado';
        await sendComment(post.id, input);
        commentInputs.value[post.id] = '';
        activePostId.value = null;
        const fetchedPosts = await fetchGroupPosts(id);
        posts.value = fetchedPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
    } catch (e) {
        console.error(e);
        alert('Error al comentar');
    }
  }

  function replyTo(comment) {
    const input = replyInputs.value[comment.id]
    if (!input) return

    const reply = {
      id: Date.now(),
      content: input,
      score: 0
    }

    comment.replies = comment.replies || []
    comment.replies.push(reply)
    replyInputs.value[comment.id] = ''
    activeReplyBox.value = null
  }
  onMounted(async () => {
    try {
      const data = await fetchJoinGroup(id)
      group.value = {
        name: data.name || '—Sin nombre—',
        author: data.adminName || `Admin ${data.adminId || ''}`,
        date: data.creationDate || new Date().toISOString(),
        content: data.description || '—Sin descripción—',
        imageUrl: data.imageUrl || '/assets/default.jpg',
        likes: data.likes ?? 0,
        comments: data.comments ?? 0,
        shareCount: data.shareCount ?? 0,
        groupInfo: {
          title: data.name || '—Sin título—',
          description: data.description || '—Sin descripción—',
          created: data.creationDate || new Date().toISOString(),
          visibility: data.visibility || 'Público',
          members: data.membersCount ?? 0,
          online: data.onlineCount ?? 0
        }
      }

      // 🔽 Aquí se cargan las publicaciones
      const fetchedPosts = await fetchGroupPosts(id)
      posts.value = fetchedPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))

    } catch (e) {
      console.error(e)
    }
  })

  async function onJoin() {
    try {
      await joinGroup(id)
      store.joined = true
    } catch (e) {
      console.error(e)
      alert('No se pudo unir al grupo.')
    }
  }

  function onCreatePost() {
    router.push(`/group/${id}/create-post`)
  }
  function formatType(type) {
    const map = {
      0: 'Pregunta / Asesoría',
      1: 'Aporte educativo',
      2: 'Discusión',
      3: 'Noticias',
      4: 'Consejos',
      5: 'Experiencias',
      6: 'Solicitud',
      7: 'Oportunidades'
    };

    return map[type] ?? 'Otro';
  }
  function sharePost(postId) {
    router.push({
      path: `/group/${id}/create-post`,
      query: { originalPostId: postId }
    })
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

  .meta {
    color: #666;
    font-size: 0.85rem;
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
    display: flex;
    justify-content: space-between;
    gap: 1.5rem;
    align-items: flex-start;
  }
  /* Columna de publicaciones */
  .content-grid > .post-list {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 1.25rem;
  }

  /* Panel lateral derecho */
  .group-panel {
    width: 300px;
    flex-shrink: 0;
    background: #fff;
    border-radius: 1rem;
    padding: 1.5rem;
    position: sticky;
    top: 1rem;
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

  .loading {
    text-align: center;
    margin-top: 2rem;
    color: #888;
  }
  .post-list {
    max-height: calc(100vh - 250px); /* o lo que necesites */
    overflow-y: auto;
  }
  .comment-box, .reply-box {
    margin-top: 1rem;
  }

  .comment-input {
    width: 100%;
    min-height: 50px;
    margin-bottom: 0.5rem;
    padding: 0.5rem;
    border-radius: 0.5rem;
    border: 1px solid #ccc;
    resize: vertical;
  }

  .comment-actions {
    display: flex;
    gap: 0.5rem;
  }

  .comment-card {
    background: #fff;
    border-radius: 0.5rem;
    padding: 0.75rem;
    margin-top: 0.75rem;
  }

  .comment-card.reply {
    margin-left: 2rem;
    background: #f9f9f9;
  }

  .vote-bar {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    margin-top: 0.25rem;
  }

  .comments-list {
    margin-top: 1rem;
  }
  .post-tag {
    display: inline-block;
    background-color: #00bcd4;
    color: white;
    font-size: 0.75rem;
    padding: 0.25rem 0.5rem;
    border-radius: 9999px;
    margin-bottom: 0.5rem;
  }
</style>
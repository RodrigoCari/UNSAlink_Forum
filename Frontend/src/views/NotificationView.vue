<script setup>
import { ref, onMounted } from 'vue'
import { API_BASE } from '@/config'

const notifications = ref([])

function parseJwt (token) {
  try {
    return JSON.parse(atob(token.split('.')[1]))
  } catch {
    return null
  }
}

async function fetchNotifications() {
  const token = localStorage.getItem('token')
  const payload = parseJwt(token)
  const userId = payload?.sub

  if (!userId) {
    console.error("No se pudo obtener el userId del token")
    return
  }

  try {
    const response = await fetch(`${API_BASE}/Notification/user/${userId}`)
    if (!response.ok) throw new Error("Error al obtener notificaciones")
    const data = await response.json()
    console.log("Notificaciones recibidas:", data)
    notifications.value = data
  } catch (err) {
    console.error(err)
  }
}

async function markAsRead(id) {
  try {
    await fetch(`${API_BASE}/Notification/${id}/markAsRead`, {
      method: 'POST'
    })
    console.log(`Notificación ${id} marcada como leída`)
    // actualiza solo local para evitar otro fetch completo
    notifications.value = notifications.value.map(n =>
      n.id === id ? { ...n, isRead: true } : n
    )
  } catch (err) {
    console.error("Error marcando como leída", err)
  }
}

onMounted(() => {
  fetchNotifications()
})
</script>

<template>
  <div class="notifications-container">
    <div class="content">
      <h1 class="title">Notificaciones</h1>
      <div v-if="notifications.length > 0">
        <div 
          v-for="n in notifications" 
          :key="n.id" 
          class="notification-card" 
          :class="{ unread: !n.isRead }"
        >
          <div class="notification-header">
            <div style="display: flex; align-items: center;">
              <span class="dot" v-if="!n.isRead"></span>
              <strong>{{ n.notificationType }}</strong>
            </div>
            <small class="date">{{ new Date(n.createdAt).toLocaleString() }}</small>
          </div>
          <p class="message">{{ n.message }}</p>
          <button 
            v-if="!n.isRead" 
            class="mark-btn"
            @click="markAsRead(n.id)"
          >
            Marcar como leído
          </button>
        </div>
      </div>
      <div v-else>
        <img class="bot-image" src="@/assets/robot.jpg" alt="Bot" />
        <p class="message">Aún no tienes ninguna actividad</p>
        <button class="explore-btn">Visita r/Explorar</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.notifications-container {
  display: flex;
  justify-content: center;
  align-items: center;
  flex: 1;
  height: calc(100vh - 60px); /* resta la altura del topbar */
  background-color: #f3f4f6;
  font-family: 'Montserrat', sans-serif;
}

.content {
  width: 100%;
  max-width: 600px;
}

.title {
  font-size: 1.75rem;
  font-weight: 600;
  margin-bottom: 1rem;
  text-align: center;
}

.notification-card {
  background: white;
  border-radius: 12px;
  padding: 1rem;
  margin-bottom: 1rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  transition: background-color 0.3s;
}

.notification-card.unread {
  border-left: 4px solid #2d9cdb;
  background-color: #e8f4fc;
}

.notification-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.dot {
  height: 10px;
  width: 10px;
  background-color: #2d9cdb;
  border-radius: 50%;
  display: inline-block;
  margin-right: 8px;
}

.date {
  font-size: 0.85rem;
  color: #888;
}

.message {
  margin-top: 0.5rem;
  color: #333;
  text-align: center;
}

.mark-btn {
  margin-top: 0.5rem;
  background-color: #2d9cdb;
  color: white;
  border: none;
  padding: 0.4rem 1rem;
  border-radius: 15px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s;
}

.mark-btn:hover {
  background-color: #228ac7;
}

.bot-image {
  width: 180px;
  margin: 1rem auto;
  display: block;
}

.explore-btn {
  background-color: #2d9cdb;
  color: white;
  border: none;
  padding: 0.6rem 1.5rem;
  border-radius: 20px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s;
  display: block;
  margin: 1rem auto;
}

.explore-btn:hover {
  background-color: #228ac7;
}
</style>

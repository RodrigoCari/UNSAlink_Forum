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
      <span class="normal-text">
        Agrega información adicional para mayor interacción profesional
      </span>
    </div>

    <!-- Línea 3 -->
    <div class="section-title">
      <span class="bold-text">General</span>
    </div>

    <!-- Formulario tipo cards -->
    <div class="form-cards">
      <div class="card">
        <label for="displayName">Nombre para mostrar</label>
        <input
          type="text"
          id="displayName"
          v-model="profile.name"
          placeholder="Tu nombre visible"
        />
      </div>

      <div class="card">
        <label for="email">Email</label>
        <input
          type="email"
          id="email"
          v-model="profile.email"
          placeholder="ejemplo@correo.com"
        />
      </div>

      <div class="card">
        <label>Intereses</label>
        <div class="interest-options">
          <label v-for="item in allInterests" :key="item" class="checkbox-label">
            <input
              type="checkbox"
              :value="item"
              v-model="profile.interests"
            />
            {{ item }}
          </label>
        </div>
      </div>

      <div class="card">
        <button @click="updateProfile">Guardar cambios</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

// Lista de intereses disponibles
const allInterests = [
  'ART',
  'DRAW',
  'CONECTIVITY',
  'NEWS',
  'JOB MARKET',
  'SEARCHING'
]

const profile = ref({
  name: '',
  email: '',
  interests: []
})

// Extraer token y userId desde localStorage
const token = localStorage.getItem('token')
const userId = localStorage.getItem('userId')

// Configurar headers con token
const axiosConfig = {
  headers: {
    Authorization: `Bearer ${token}`
  }
}

// Obtener datos del perfil
const fetchProfile = async () => {
  try {
    const res = await axios.get(`https://localhost:44329/api/User/${userId}`, axiosConfig)
    profile.value = {
      name: res.data.name,
      email: res.data.email,
      interests: res.data.interests || []
    }
  } catch (err) {
    console.error('Error al obtener perfil:', err)
    alert('No se pudo cargar el perfil')
  }
}

// Actualizar datos del perfil
const updateProfile = async () => {
  try {
    await axios.put(`https://localhost:44329/api/User/${userId}`, {
      name: profile.value.name,
      email: profile.value.email,
      interests: profile.value.interests
    }, axiosConfig)
    alert('Perfil actualizado correctamente')
  } catch (err) {
    console.error('Error al actualizar perfil:', err)
    alert('No se pudo actualizar el perfil')
  }
}

onMounted(fetchProfile)
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

.card input,
.card textarea {
  border: 1px solid #ddd;
  border-radius: 6px;
  padding: 0.75rem;
  font-size: 1rem;
  background-color: #f9f9f9;
  transition: border-color 0.3s;
}

.card input:focus,
.card textarea:focus {
  outline: none;
  border-color: #a5a5ff;
  background-color: white;
}

.card button {
  background-color: #4f46e5;
  color: white;
  padding: 0.75rem 1rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  margin-top: 1rem;
  align-self: flex-start;
}

.card button:hover {
  background-color: #3730a3;
}

.interest-options {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 500;
}
</style>

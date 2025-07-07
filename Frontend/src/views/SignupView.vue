<script setup>
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const username = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const router = useRouter()

const isEmailValid = (email) =>
  /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)

const register = async () => {
  if (!username.value.trim() || !email.value.trim() || !password.value || !confirmPassword.value) {
    alert('Por favor completa todos los campos.')
    return
  }

  if (username.value.trim().length < 3) {
    alert('El nombre de usuario debe tener al menos 3 caracteres.')
    return
  }

  if (!isEmailValid(email.value.trim())) {
    alert('El email no es válido.')
    return
  }

  if (password.value.length < 6) {
    alert('La contraseña debe tener al menos 6 caracteres.')
    return
  }

  if (password.value !== confirmPassword.value) {
    alert('Las contraseñas no coinciden.')
    return
  }

  try {
    const response = await axios.post('https://localhost:44329/api/user', {
      name: username.value.trim(),
      email: email.value.trim(),
      password: password.value.trim(),
      role: 0
    })

    console.log('Usuario registrado:', response)
    router.push('/interests')
  } catch (error) {
    console.error('Error al registrar usuario:', error.response?.data || error.message)
    alert('Error al registrarse. Verifica los datos e intenta nuevamente.')
  }
}
</script>

<template>
  <div class="signup-container">
    <div class="signup-box">
      <div class="icon-lock">🔒</div>
      <h2>Create an account</h2>

      <div class="input-group">
        <span class="icon">👤</span>
        <input type="text" placeholder="Username" v-model="username" />
      </div>

      <div class="input-group">
        <span class="icon">📧</span>
        <input type="email" placeholder="Email" v-model="email" />
      </div>

      <div class="input-group">
        <span class="icon">🔒</span>
        <input type="password" placeholder="Password" v-model="password" />
      </div>

      <div class="input-group">
        <span class="icon">🔒</span>
        <input type="password" placeholder="Confirm Password" v-model="confirmPassword" />
      </div>

      <button class="signup-btn" @click="register">Sign Up</button>

      <p class="already-account">
        Already have an account?
        <span class="login-link" @click="$router.push('/login')">Login!</span>
      </p>
    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;600&display=swap');

.signup-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: #f3f4f6;
  font-family: 'Montserrat', sans-serif;
}

.signup-box {
  background: #2d718f;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
  color: white;
}

.icon-lock {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.input-group {
  display: flex;
  align-items: center;
  background: white;
  border: 1px solid #ccc;
  border-radius: 4px;
  margin: 0.5rem 0;
  padding: 0.5rem;
  color: black;
}

.input-group .icon {
  margin-right: 0.5rem;
  color: #2d718f;
}

.input-group input {
  border: none;
  outline: none;
  background: transparent;
  color: black;
  flex: 1;
  font-family: 'Montserrat', sans-serif;
}

.input-group input::placeholder {
  color: #888;
}

.signup-btn {
  background-color: #000000;
  color: white;
  border: none;
  padding: 0.5rem;
  width: 100%;
  border-radius: 4px;
  margin-top: 1rem;
  cursor: pointer;
  font-weight: bold;
  font-family: 'Montserrat', sans-serif;
}

.already-account {
  margin-top: 1rem;
  color: rgba(255, 255, 255, 0.8);
  font-size: 0.875rem;
}

.login-link {
  text-decoration: underline;
  cursor: pointer;
  color: white;
}
</style>

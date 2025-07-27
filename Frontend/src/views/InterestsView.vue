<script setup>
import { ref } from 'vue'
import { getUserProfile, updateUserProfile } from '@/services/userService'

const selected = ref(new Set())

const interests = [
  { icon: '🎨', label: 'ART' },
  { icon: '✏', label: 'DRAW' },
  { icon: '🌐', label: 'CONECTIVITY' },
  { icon: '📰', label: 'NEWS' },
  { icon: '💰', label: 'JOB MARKET' },
  { icon: '🔍', label: 'SEARCHING' }
]

const toggleInterest = (label) => {
  if (selected.value.has(label)) selected.value.delete(label)
  else selected.value.add(label)
}

const continueToHome = async () => {
  const token = localStorage.getItem('token')
  const userId = localStorage.getItem('userId')

  try {
    const currentProfile = await getUserProfile(userId, token)

    await updateUserProfile(userId, {
      name: currentProfile.name,
      email: currentProfile.email,
      interests: Array.from(selected.value)
    }, token)

    alert('Intereses guardados')
    window.location.href = '/home'
  } catch (e) {
    alert('Error al guardar intereses')
    console.error(e)
  }
}
</script>

<template>
  <div class="interests-container">
    <h1>Intereses</h1>
    <p>Elige que temas te gustaría ver en tu feed principal.</p>

    <div class="cards-grid">
      <div
        class="card"
        v-for="interest in interests"
        :key="interest.label"
        :class="{ selected: selected.has(interest.label) }"
        @click="toggleInterest(interest.label)"
      >
        <span class="icon">{{ interest.icon }}</span> {{ interest.label }}
      </div>
    </div>

    <button class="continue-btn" @click="continueToHome">Continuar</button>
  </div>
</template>

<style scoped>
.interests-container {
  max-width: 400px;
  margin: 2rem auto;
  font-family: 'Montserrat', sans-serif;
  text-align: center;
  color: #000;
}

h1 {
  font-weight: 600;
  margin-bottom: 0.5rem;
}

p {
  margin-bottom: 1.5rem;
  color: #333;
}

.cards-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
  margin-bottom: 2rem;
}

.card {
  background-color: black;
  color: white;
  padding: 1rem;
  border-radius: 8px;
  display: flex;
  align-items: center;
  font-weight: 600;
  cursor: pointer;
  user-select: none;
}

.card .icon {
  margin-right: 0.75rem;
  font-size: 1.5rem;
}

.continue-btn {
  background-color: black;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  font-family: 'Montserrat', sans-serif;
  width: 100%;
}

.card.selected {
  background-color: #333;
  border: 2px solid #fff;
}
</style>

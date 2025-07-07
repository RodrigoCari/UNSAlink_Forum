<template>
  <div class="interests-container">
    <h1>Intereses</h1>
    <p>Elige que temas te gustaría ver en tu feed principal.</p>

    <div class="cards-grid">
      <div class="card"><span class="icon">🎨</span> ART</div>
      <div class="card"><span class="icon">✏</span> DRAW</div>
      <div class="card"><span class="icon">🌐</span> CONECTIVITY</div>
      <div class="card"><span class="icon">📰</span> NEWS</div>
      <div class="card"><span class="icon">💰</span> JOB MARKET</div>
      <div class="card"><span class="icon">🔍</span> SEARCHING</div>
    </div>
    
    <button class="continue-btn" @click="$router.push('/home')">Continuar</button>
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
</style>

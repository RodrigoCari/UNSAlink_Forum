<template>
  <div class="search-wrapper">
    <input v-model="query"
           @input="onInput"
           placeholder="Buscar grupos…"
           class="search-input" />

    <div v-if="loading" class="status">Buscando…</div>
    <div v-else-if="error" class="status error">{{ error }}</div>

    <ul v-if="results.length" class="results-list">
      <li v-for="g in results" :key="g.id" class="result-item">
        {{ g.name }}
      </li>
    </ul>

    <div v-else-if="!loading" class="status">No se encontraron grupos.</div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { searchGroups } from '../services/groupService'

  const query = ref('')
  const results = ref([])
  const loading = ref(false)
  const error = ref('')

  // Debounce sencillo
  let timer = null
  function onInput() {
    clearTimeout(timer)
    if (!query.value.trim()) {
      results.value = []
      return
    }
    timer = setTimeout(fetch, 300)
  }

  async function fetch() {
    loading.value = true
    error.value = ''
    try {
      results.value = await searchGroups(query.value)
    } catch (e) {
      error.value = e.message
    } finally {
      loading.value = false
    }
  }
</script>

<style scoped>
  .search-wrapper {
    max-width: 400px;
    margin: 2rem auto;
  }

  .search-input {
    width: 100%;
    padding: 0.5rem;
    font-size: 1rem;
    border: 1px solid #aaa;
    border-radius: 4px;
  }

  .status {
    margin-top: 0.5rem;
    color: #555;
  }

    .status.error {
      color: #c00;
    }

  .results-list {
    margin-top: 0.75rem;
    max-height: 200px;
    overflow-y: auto;
    border: 1px solid #ddd;
    border-radius: 4px;
    padding: 0;
  }

  .result-item {
    list-style: none;
    padding: 0.5rem;
    border-bottom: 1px solid #eee;
    cursor: pointer;
  }

    .result-item:hover {
      background: #f0f8ff;
    }
</style>

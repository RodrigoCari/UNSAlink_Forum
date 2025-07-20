<!-- src/components/SearchBar.vue -->
<template>
  <div class="search-container">
    <input v-model="query"
           @input="onSearch"
           type="text"
           placeholder="Buscar en UNSAlink…"
           class="search-input" />
    <ul v-if="results.length" class="dropdown">
      <li v-for="g in results"
          :key="g.id"
          @click="onSelect(g)"
          class="dropdown-item">
        {{ g.name }}
      </li>
    </ul>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  import { searchGroups } from '@/services/groupService'

  const router = useRouter()
  const query = ref('')
  const results = ref([])

  let timer = null
  async function onSearch() {
    clearTimeout(timer)
    if (!query.value.trim()) {
      results.value = []
      return
    }
    // pequeño debounce
    timer = setTimeout(async () => {
      try {
        results.value = await searchGroups(query.value)
      } catch {
        results.value = []
      }
    }, 200)
  }

  function onSelect(group) {
    router.push(`/groups/${group.id}`)
    query.value = ''
    results.value = []
  }
</script>

<style scoped>
  .search-container {
    position: relative;
    width: 300px;
  }

  .search-input {
    width: 100%;
    padding: 0.5rem 1rem;
    border: 1px solid #d1d5db;
    border-radius: 999px;
    font-size: 0.9rem;
    outline: none;
  }

  .dropdown {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    margin-top: 0.25rem;
    background: #fff;
    border: 1px solid #ddd;
    border-radius: 0.5rem;
    max-height: 200px;
    overflow-y: auto;
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    z-index: 20;
    list-style: none;
    padding: 0;
  }

  .dropdown-item {
    padding: 0.5rem 1rem;
    cursor: pointer;
  }

    .dropdown-item:hover {
      background: #f5f5f5;
    }
</style>

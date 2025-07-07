<!-- src/views/GroupDetailView.vue -->
<template>
  <div class="group-detail">
    <h1>{{ group.name }}</h1>
    <p>{{ group.description }}</p>
    <button :disabled="joining || joined"
            @click="onJoin">
      {{
        joined
        ? 'Ya eres miembro del grupo'
        : (joining ? 'Uniéndote...' : 'Unirse al grupo')
      }}
    </button>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useGroupStore } from '../stores/group'
import { fetchGroup, joinGroup } from '../services/groupService'

const route = useRoute()
const id = route.params.id
const store = useGroupStore()
const joining = ref(false)
const group = ref({ name: '', description: '' })

onMounted(async () => {
  group.value = await fetchGroup(id)
})

async function onJoin() {
  joining.value = true
  await joinGroup(id)
  store.joined = true
  joining.value = false
}

const joined = computed(() => store.joined)
</script>



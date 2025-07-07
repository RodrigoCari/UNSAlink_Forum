import { defineStore } from 'pinia'

export const useGroupStore = defineStore('group', {
  state: () => ({
    joined: false
  }),
  actions: {
    async join(id) {
      const res = await fetch(`/api/groups/${id}/join`, { method: 'POST' })
      if (!res.ok) throw new Error('Error al unirse')
      this.joined = true
    }
  }
})

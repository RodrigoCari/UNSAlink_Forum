const API_BASE = import.meta.env.VITE_API_BASE || 'https://localhost:44329/api/Group'

export async function fetchJoinGroup(id) {
  const res = await fetch(`${API_BASE}/${id}`, { mode: 'cors' })
  if (!res.ok) {
    throw new Error(`Error ${res.status} al obtener el grupo`)
  }
  const api = await res.json()

  return {
    id: api.id,
    name: api.name || '—Sin nombre—',
    description: api.description || '—Sin descripción—',
    adminName: api.adminName || `Admin ${api.adminId || '??'}`,
    creationDate: api.creationDate || new Date().toISOString(),
    membersCount: api.membersCount ?? 0,
    onlineCount: api.onlineCount ?? 0,
    posts: api.posts || []
  }
}

export async function joinGroup(id) {
  const userId = 'F87E501A-93AC-4D75-87A9-1E1FACEEC900'
  const res = await fetch(
    `${API_BASE}/${id}/join?userId=${userId}`,
    { method: 'POST', mode: 'cors' }
  )
  if (!res.ok) {
    throw new Error(`Error ${res.status} al unirse al grupo`)
  }
  return await res.json()
}

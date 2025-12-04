import { API_BASE } from '@/config';
const GROUP_API = `${API_BASE}/Group`;

export async function fetchJoinGroup(id) {
  const res = await fetch(`${GROUP_API}/${id}`, { mode: 'cors' })
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
  const userId = localStorage.getItem('userId')
  const res = await fetch(
    `${GROUP_API}/${id}/join?userId=${userId}`,
    { method: 'POST', mode: 'cors' }
  )
  if (!res.ok) {
    throw new Error(`Error ${res.status} al unirse al grupo`)
  }
  return await res.json()
}

import { API_BASE } from '@/config';

export async function createGroup({ name, description, adminId }) {
  if (!adminId) throw new Error('adminId es requerido para crear el grupo');
  const res = await fetch(`${API_BASE}/Group`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, description, adminId })
  });
  if (!res.ok) {
    let errorMsg;
    try { errorMsg = (await res.json()).detail; }
    catch { errorMsg = `Error ${res.status} al crear grupo`; }
    throw new Error(errorMsg);
  }
  return await res.json();
}

export async function fetchGroup(id) {
  const res = await fetch(`${API_BASE}/Group/${id}`, { mode: 'cors' });
  if (!res.ok) {
    let err;
    try { err = (await res.json()).detail; }
    catch { err = `Error ${res.status} al obtener grupo`; }
    throw new Error(err);
  }
  return await res.json();
}

export async function searchGroups(name) {
  const q = encodeURIComponent(name)
  const res = await fetch(`${API_BASE}/Group/search?name=${q}`, { mode: 'cors' })
  if (!res.ok) {
    let err
    try { err = (await res.json()).detail }
    catch { err = `Error ${res.status} buscando grupos` }
    throw new Error(err)
  }
  return await res.json()
}

export async function fetchGroupsByUser(userId) {
  const res = await fetch(`${API_BASE}/Group/user/${userId}`, { mode: 'cors' })
  if (!res.ok) {
    let err
    try { err = (await res.json()).detail }
    catch { err = `Error ${res.status} al obtener grupos del usuario` }
    throw new Error(err)
  }
  return await res.json()
}

export async function fetchGroupsWithLatestPosts() {
  const res = await fetch(`${API_BASE}/Group/with-latest-posts`, { mode: 'cors' });
  if (!res.ok) {
    let err;
    try { err = (await res.json()).detail; }
    catch { err = `Error ${res.status} obteniendo grupos con últimas publicaciones`; }
    throw new Error(err);
  }
  return await res.json();
}

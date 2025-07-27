const API_BASE = import.meta.env.VITE_API_BASE || 'https://localhost:44329/api';

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

import { API_BASE } from '@/config';

export async function fetchUserById(id) {
  const res = await fetch(`${API_BASE}/User/${id}`, { mode: 'cors' });
  if (!res.ok) {
    let err;
    try { err = (await res.json()).detail; }
    catch { err = `Error ${res.status} al obtener usuario`; }
    throw new Error(err);
  }
  return await res.json();
}

export async function updateUserProfile(id, data, token) {
  const res = await fetch(`${API_BASE}/User/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(data)
  });

  if (!res.ok) throw new Error('Error al actualizar perfil');
}

export async function getUserProfile(id, token) {
  const res = await fetch(`${API_BASE}/User/${id}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${token}`
    },
    mode: 'cors'
  });

  if (!res.ok) {
    let err;
    try { err = (await res.json()).detail; }
    catch { err = `Error ${res.status} al obtener usuario`; }
    throw new Error(err);
  }

  return await res.json();
}

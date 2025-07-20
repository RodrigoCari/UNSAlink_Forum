const API_BASE = import.meta.env.VITE_API_BASE || 'https://localhost:44329/api';

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

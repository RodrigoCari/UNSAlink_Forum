const API_BASE = import.meta.env.VITE_API_BASE || 'https://localhost:44329/api';

export async function createPost(dto) {
  const res = await fetch(`${API_BASE}/Post`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    let err;
    try { err = (await res.json()).detail; }
    catch { err = `Error ${res.status} al crear publicación`; }
    throw new Error(err);
  }
  return;
}

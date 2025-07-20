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
  const res = await fetch(`${API_BASE}/Group/search?name=${encodeURIComponent(name)}`, {
    mode: 'cors'
  });
  if (!res.ok) {
    const problem = await res.json().catch(() => null);
    throw new Error(problem?.detail || `Error ${res.status} al buscar grupos`);
  }
  return /** @type {{id:string,name:string}[]} */(await res.json());
}

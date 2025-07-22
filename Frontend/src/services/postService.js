const API_BASE = import.meta.env.VITE_API_BASE || 'https://localhost:44329/api';

function handleError(res, defaultMessage) {
    return res.json().then(json => {
        throw new Error(json.detail || defaultMessage);
    }).catch(() => {
        throw new Error(`${defaultMessage} (status ${res.status})`);
    });
}

export async function createPost(dto) {
    const res = await fetch(`${API_BASE}/Post`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto)
    });
    if (!res.ok) throw await handleError(res, 'Error al crear publicación');
    return await res.json();
}

export async function fetchGroupPosts(groupId) {
    const res = await fetch(`${API_BASE}/Post/group/${groupId}`, { mode: 'cors' });
    if (!res.ok) throw await handleError(res, 'Error al obtener publicaciones');
    return await res.json();
}

export async function addComment(postId, content) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${API_BASE}/Post/${postId}/comment`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ content })
    });
    if (!res.ok) throw new Error('Error al comentar');
}
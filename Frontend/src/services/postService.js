import { API_BASE } from '@/config';

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

export async function fetchPostsByUser(userId) {
  const res = await fetch(`${API_BASE}/Post/user/${userId}`, { mode: 'cors' });
  if (!res.ok) {
    const err = await res.text();
    throw new Error(`Error fetching posts: ${err}`);
  }
  return await res.json();
}
function handleError(res, defaultMessage) {
  return res.json().then(json => {
    throw new Error(json.detail || defaultMessage);
  }).catch(() => {
    throw new Error(`${defaultMessage} (status ${res.status})`);
  });

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

export async function getPostById(postId) {
  const res = await fetch(`${API_BASE}/Post/${postId}`, {
    method: 'GET',
    headers: { 'Content-Type': 'application/json' }
  });

  if (!res.ok) {
    const errorText = await res.text();
    throw new Error(`Error al obtener post original: ${errorText}`);
  }

  return await res.json();
}

export async function sharePost({ title, groupId, originalPostId, content, type }) {
  const token = localStorage.getItem('token');
  const authorId = localStorage.getItem('userId');

  const payload = {
    title,
    groupId,
    originalPostId,
    content,
    type,
    authorId
  };

  const res = await fetch(`${API_BASE}/Post/share`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(payload)
  });

  if (!res.ok) {
    const errorText = await res.text();
    throw new Error(`Error al compartir publicación: ${errorText}`);
  }

  return await res.json();
}
import api from './api';

export async function fetchJoinGroup(id) {
  const { data: apiData } = await api.get(`/Group/${id}`);

  return {
    id: apiData.id,
    name: apiData.name || '—Sin nombre—',
    description: apiData.description || '—Sin descripción—',
    adminName: apiData.adminName || `Admin ${apiData.adminId || '??'}`,
    creationDate: apiData.creationDate || new Date().toISOString(),
    membersCount: apiData.membersCount ?? 0,
    onlineCount: apiData.onlineCount ?? 0,
    posts: apiData.posts || []
  };
}

export async function joinGroup(id) {
  const userId = localStorage.getItem('userId');
  const { data } = await api.post(`/Group/${id}/join?userId=${userId}`);
  return data;
}


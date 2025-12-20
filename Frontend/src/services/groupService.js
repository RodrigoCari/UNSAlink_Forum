import api from './api';

export async function createGroup({ name, description, adminId }) {
  if (!adminId) throw new Error('adminId es requerido para crear el grupo');
  const { data } = await api.post('/Group', { name, description, adminId });
  return data;
}

export async function fetchGroup(id) {
  const { data } = await api.get(`/Group/${id}`);
  return data;
}

export async function searchGroups(name) {
  const q = encodeURIComponent(name);
  const { data } = await api.get(`/Group/search?name=${q}`);
  return data;
}

export async function fetchGroupsByUser(userId) {
  const { data } = await api.get(`/Group/user/${userId}`);
  return data;
}

export async function fetchGroupsWithLatestPosts() {
  const { data } = await api.get('/Group/with-latest-posts');
  return data;
}


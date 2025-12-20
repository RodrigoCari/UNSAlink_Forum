import api from './api';

export async function fetchUserById(id) {
  const { data } = await api.get(`/User/${id}`);
  return data;
}

export async function updateUserProfile(id, data) {
  await api.put(`/User/${id}`, data);
}

export async function getUserProfile(id) {
  const { data } = await api.get(`/User/${id}`);
  return data;
}


import api from './api';

export async function createPost(dto) {
  const { data } = await api.post('/Post', dto);
  return data;
}

export async function fetchPostsByUser(userId) {
  const { data } = await api.get(`/Post/user/${userId}`);
  return data;
}

export async function fetchGroupPosts(groupId) {
  const { data } = await api.get(`/Post/group/${groupId}`);
  return data;
}

export async function addComment(postId, content) {
  await api.post(`/Post/${postId}/comment`, { content });
}

export async function getPostById(postId) {
  const { data } = await api.get(`/Post/${postId}`);
  return data;
}

export async function sharePost({ title, groupId, originalPostId, content, type }) {
  const authorId = localStorage.getItem('userId');

  const payload = {
    title,
    groupId,
    originalPostId,
    content,
    type,
    authorId
  };

  const { data } = await api.post('/Post/share', payload);
  return data;
}
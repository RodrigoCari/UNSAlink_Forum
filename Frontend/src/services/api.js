import axios from 'axios';
import { API_BASE } from '@/config';

const api = axios.create({
    baseURL: API_BASE,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor para agregar el token automáticamente
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Interceptor para manejar errores globalmente
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            // Opcional: Redirigir a login o limpiar token
            // localStorage.removeItem('token');
            // window.location.href = '/login';
        }
        const message = error.response?.data?.detail || error.message || 'Error desconocido';
        return Promise.reject(new Error(message));
    }
);

export default api;

import api from './api';

export const authService = {
    async login(username, password) {
        const { data } = await api.post('/User/login', {
            name: username,
            password: password
        });
        return data;
    },

    async signup(userData) {
        const { data } = await api.post('/User', userData);
        return data;
    }
}


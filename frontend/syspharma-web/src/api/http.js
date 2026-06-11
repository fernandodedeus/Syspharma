import axios from 'axios';

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:5253/api/v1'
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('syspharma.accessToken');


  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

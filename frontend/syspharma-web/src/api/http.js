import axios from 'axios';

// Se o front está rodando em HTTPS, a API também usa HTTPS
// Se está em HTTP, usa HTTP — sem precisar trocar .env manualmente

const protocol = window.location.protocol; // 'http:' ou 'https:'
const baseURL = `${protocol}//localhost:${protocol === 'https:' ? '7267' : '5253'}/api/v1`;

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? baseURL
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('syspharma.accessToken');


  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

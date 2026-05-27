import axios from 'axios';

export const api = axios.create({
  baseURL: 'https://localhost:7000/api' // pode mudar, verifique a porta do seu backend
});
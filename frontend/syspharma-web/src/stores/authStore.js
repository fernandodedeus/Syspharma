import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { api } from '../api/http';

const ACCESS_TOKEN_KEY = 'syspharma.accessToken';
const REFRESH_TOKEN_KEY = 'syspharma.refreshToken';
const USER_KEY = 'syspharma.user';

function readStoredUser() {
  const rawUser = localStorage.getItem(USER_KEY);

  if (!rawUser) {
    return null;
  }

  try {
    return JSON.parse(rawUser);
  } catch {
    localStorage.removeItem(USER_KEY);
    return null;
  }
}

function extractPhotoPath(payload) {
  if (!payload) return null;

  return payload.profilePhotoPath ?? payload.photo ?? payload.photoUrl ?? null;
}

// Monta a URL completa da foto a partir do caminho relativo ou de uma URL já completa.
// ex: "imgs/foto.jpg" → "https://localhost:7267/imgs/foto.jpg"
export function buildPhotoUrl(path) {
  if (!path) return null;

  if (/^https?:\/\//i.test(path)) {
    return path;
  }

  const base = import.meta.env.VITE_API_URL ?? 'https://localhost:7267/api/v1';
  const serverBase = base.replace(/\/api\/v\d+(\/)?$/i, '').replace(/\/$/, '');
  const normalizedPath = String(path).replace(/^\/+/, '').replace(/^api\/v\d+\//i, '');

  return `${serverBase}/${normalizedPath}`;
}

export const useAuthStore = defineStore('auth', () => {
  const accessToken = ref(localStorage.getItem(ACCESS_TOKEN_KEY));
  const refreshToken = ref(localStorage.getItem(REFRESH_TOKEN_KEY));
  const user = ref(readStoredUser());
  const loading = ref(false);
  const error = ref('');

  const isAuthenticated = computed(() => Boolean(accessToken.value));

  // Getter para a URL completa da foto do usuário logado
  const photoUrl = computed(() => buildPhotoUrl(user.value?.photo));

  function persistSession(response) {
    accessToken.value = response.accessToken;
    refreshToken.value = response.refreshToken;
    user.value = {
      id: response.userId,
      name: response.fullName,
      email: response.email ?? '',
      photo: extractPhotoPath(response)
    };

    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken.value);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken.value);
    localStorage.setItem(USER_KEY, JSON.stringify(user.value));
  }

  function clearSession() {
    accessToken.value = null;
    refreshToken.value = null;
    user.value = null;

    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
  }

  // Atualiza a foto no store e no localStorage após upload
  function updatePhoto(path) {
    if (!user.value) return;

    user.value.photo = extractPhotoPath({ profilePhotoPath: path });
    localStorage.setItem(USER_KEY, JSON.stringify(user.value));
  }

  // Busca os dados atualizados do usuário logado via /Auth/me
  async function fetchMe() {
    try {
      const { data } = await api.get('/Auth/me');

      if (user.value) {
        user.value.name = data.fullName;
        user.value.email = data.email;
        user.value.photo = extractPhotoPath(data);
        localStorage.setItem(USER_KEY, JSON.stringify(user.value));
      }
    } catch {
      // Silencioso — não bloqueia o fluxo
    }
  }

  async function login(credentials) {
    loading.value = true;
    error.value = '';

    try {
      const { data } = await api.post('/Auth/login', credentials);
      persistSession(data);

      // Após login, busca os dados completos incluindo a foto
      await fetchMe();
    } catch (requestError) {
      error.value =
        requestError.response?.data?.detail ?? 'Não foi possível entrar.';
      throw requestError;
    } finally {
      loading.value = false;
    }
  }

  async function logout() {
    const currentRefreshToken = refreshToken.value;
    clearSession();

    if (currentRefreshToken) {
      try {
        await api.post('/Auth/logout', { refreshToken: currentRefreshToken });
      } catch {
        // A sessão local já foi encerrada.
      }
    }
  }

  return {
    accessToken,
    refreshToken,
    user,
    loading,
    error,
    isAuthenticated,
    photoUrl,
    login,
    logout,
    clearSession,
    updatePhoto,
    fetchMe
  };
});
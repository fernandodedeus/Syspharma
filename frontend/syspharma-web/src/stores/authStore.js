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

// Monta a URL completa da foto a partir do caminho relativo
// ex: "imgs/foto.jpg" → "https://localhost:7267/imgs/foto.jpg"
export function buildPhotoUrl(path) {
  if (!path) return null;

  const base = import.meta.env.VITE_API_URL ?? 'https://localhost:7267/api/v1';

  // Remove o "/api/v1" do final para pegar só a base do servidor
  const serverBase = base.replace('/api/v1', '');

  return `${serverBase}/${path}`;
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
      photo: response.profilePhotoPath ?? null
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

    user.value.photo = path;
    localStorage.setItem(USER_KEY, JSON.stringify(user.value));
  }

  // Busca os dados atualizados do usuário logado via /Auth/me
  async function fetchMe() {
    try {
      const { data } = await api.get('/Auth/me');

      if (user.value) {
        user.value.name = data.fullName;
        user.value.email = data.email;
        user.value.photo = data.profilePhotoPath ?? null;
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
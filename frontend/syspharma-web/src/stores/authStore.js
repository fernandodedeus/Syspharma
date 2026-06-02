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

export const useAuthStore = defineStore('auth', () => {
  const accessToken = ref(localStorage.getItem(ACCESS_TOKEN_KEY));
  const refreshToken = ref(localStorage.getItem(REFRESH_TOKEN_KEY));
  const user = ref(readStoredUser());
  const loading = ref(false);
  const error = ref('');

  const isAuthenticated = computed(() => Boolean(accessToken.value));

  function persistSession(response) {
    accessToken.value = response.accessToken;
    refreshToken.value = response.refreshToken;
    user.value = {
      id: response.userId,
      name: response.fullName
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

  async function login(credentials) {
    loading.value = true;
    error.value = '';

    try {
      const { data } = await api.post('/Auth/login', credentials);
      persistSession(data);
    } catch (requestError) {
      error.value = requestError.response?.data?.detail ?? 'Nao foi possivel entrar.';
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
        // A sessao local ja foi encerrada.
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
    login,
    logout,
    clearSession
  };
});

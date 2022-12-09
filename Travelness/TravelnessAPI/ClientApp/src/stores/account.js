import api_service from '@/services/api-service'

const api = api_service;
//api.defaults.headers.post['Content-Type'] = 'multipart/form-data';

const state = {
    currentUser: JSON.parse(localStorage.getItem('USER')) || null,
    token: localStorage.getItem('TOKEN') || null,
};

const getters = {
    currentUser: state => {
        return state.currentUser;
    },
    token: state => {
        return state.token;
    },
    isAuthenticated: state => {
        return state.currentUser != null;
    }
};

const mutations = {
    SET_AUTHENTICATION_TOKEN: (state, data) => {
        state.token = data.access_token;
        state.currentUser = data.user;
        localStorage.setItem('TOKEN', state.token);
        localStorage.setItem('USER', JSON.stringify(state.currentUser));
    },
    UNSET_AUTHENTICATION_TOKEN: (state) => {
        state.token = null;
        state.currentUser = null;
        localStorage.removeItem('TOKEN');
        localStorage.removeItem('USER');
    },
    UPDATE_USER: (state, data) => {
        state.currentUser = data;
        localStorage.setItem('USER', JSON.stringify(state.currentUser));
    }
};

const actions = {
    Login: async (context, payload) => {
        return api.post("/api/account/token", payload).then((response) => context.commit('SET_AUTHENTICATION_TOKEN', response.data));
    },
    Logout: async (context) => {
        return context.commit('UNSET_AUTHENTICATION_TOKEN');
    },
    Register: async (context, payload) => {
        return api.post("/api/account/register", payload);
    },
    ConfirmRegistration: async (context, payload) => {
        return api.get("/api/account/confirmRegistration?username=" + payload.username + "&token=" + payload.token);
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

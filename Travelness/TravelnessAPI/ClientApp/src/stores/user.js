import api_service from '@/services/api-service'

const api = api_service;

const state = {
    user: {},
    users: []
};

const getters = {
    getUser: state => {
        return state.user;
    },
    getUsers: state => {
        return state.users;
    }
};

const mutations = {
    ReceiveUser: (state, data) => {
        state.user = data;
    },
    ReceiveUsers: (state, data) => {
        state.users = data;
    }
};

const actions = {
    GetById: async (context, id) => {
        return api.get("/api/user/" + id).then((response) => context.commit('ReceiveUser', response.data));
    },
    Update: async (context, payload) => {
        return api.put("/api/user", payload).then((response) => context.commit('account/UPDATE_USER', response.data, { root: true }));
    },
    GetAll: async (context) => {
        return api.get("/api/user").then((response) => context.commit('ReceiveUsers', response.data));
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

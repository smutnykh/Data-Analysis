import api_service from '@/services/api-service'

const api = api_service;
api.defaults.headers.post['Content-Type'] = 'multipart/form-data';

const state = {
    tour: {},
    tours: []
};

const getters = {
    getTour: state => {
        return state.tour;
    },
    getTours: state => {
        return state.tours;
    }
};

const mutations = {
    ReceiveTour: (state, data) => {
        state.tour = data;
    },
    ReceiveTours: (state, data) => {
        state.tours = data;
    }
};

const actions = {
    GetAll: async (context) => {
        return api.get("/api/tour").then((response) => context.commit('ReceiveTours', response.data));
    },
    GetById: async (context, payload) => {
        return api.get("/api/tour/" + payload).then((response) => context.commit('ReceiveTour', response.data));
    },
    Create: async (context, payload) => {
        return api.post("/api/tour", payload);
    },
    Edit: async (context, payload) => {
        return api.put("/api/tour", payload);
    },
    Delete: async (context, payload) => {
        return api.delete("/api/tour/" + payload);
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

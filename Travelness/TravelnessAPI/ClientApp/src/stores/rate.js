import api_service from '@/services/api-service'

const api = api_service;

const state = {
    averageRate: null,
    userRating: 0
};

const getters = {
    getAverageRate: state => {
        return state.averageRate;
    },
    getUserRating: state => {
        return state.userRating;
    }
};

const mutations = {
    ReceiveAverageRate: (state, data) => {
        state.averageRate = data;
    },
    ReceiveUserRating: (state, data) => {
        state.userRating = data;
    }
};

const actions = {
    GetAverage: async (context, payload) => {
        return api.get("/api/rate/average/" + payload).then((response) => context.commit('ReceiveAverageRate', response.data));
    },
    GetUserRating: async (context, payload) => {
        return api.get("/api/rate/" + payload).then((response) => {
            context.commit('ReceiveUserRating', response.data)
        });
    },
    Post: async (context, payload) => {
        if (payload.rating > 0)
            return api.post("/api/rate/" + payload.sightseeingId + "/" + payload.rating).then(() => {               
                context.dispatch("GetAverage", payload.sightseeingId);
                return context.dispatch("GetUserRating", payload.sightseeingId);
            });
    },
    Delete: async (context, payload) => {
        return api.delete("/api/rate/" + payload).then(() => {
            context.dispatch("GetAverage", payload);
            return context.dispatch("GetUserRating", payload);
        });
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

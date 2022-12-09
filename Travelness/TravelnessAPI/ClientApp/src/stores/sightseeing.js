import api_service from '@/services/api-service'

const api = api_service;

const state = {
    sightseeing: {},
    sightseeings: [],
};

const getters = {
    getSightseeing: state => {
        return state.sightseeing;
    },
    getSightseeings: state => {
        return state.sightseeings;
    }
};

const mutations = {
    ReceiveSightseeing: (state, data) => {
        state.sightseeing = data;
    },
    ReceiveSightseeings: (state, data) => {
        state.sightseeings = data;
    }
};

const actions = {
    GetAll: async (context) => {
        return api.get("/api/sightseeing").then((response) => context.commit('ReceiveSightseeings', response.data));
    },
    GetByPage: async (context, payload) => {
        var countriesUri = ""
        payload.countries.forEach(element => countriesUri += "&countries=" + element);
        countriesUri = countriesUri.substring(1);

        var areasUri = ""
        payload.areas.forEach(element => areasUri += "&areas=" + element);

        return api.get("/api/sightseeing/params?" + countriesUri + "&" + areasUri + "&search=" + payload.search + "&page=" + payload.page +
            "&pageSize=" + payload.size).then(function (response) {
                context.commit('ReceiveSightseeings', response.data.item1);
                return response.data.item2;
            });
    },
    GetById: async (context, payload) => {
        return api.get("/api/sightseeing/" + payload).then((response) => context.commit('ReceiveSightseeing', response.data));
    },
    GetByIdWithTours: async (context, payload) => {
        var companiesUri = ""
        payload.companies.forEach(element => companiesUri += "&companies=" + element);
        companiesUri = companiesUri.substring(1);

        return api.get("/api/sightseeing/details/" + payload.id + "?" + companiesUri + "&minPrice=" + payload.minPrice + "&maxPrice=" + payload.maxPrice
            + "&order=" + payload.order + "&page=" + payload.page + "&pageSize=" + payload.size).
            then(function (response) {
                context.commit('ReceiveSightseeing', response.data.item1);
                return response.data.item2;
            });
    },
    Create: async (context, payload) => {
        return api.post("/api/sightseeing", payload);
    },
    Edit: async (context, payload) => {
        return api.put("/api/sightseeing", payload);
    },
    Delete: async (context, payload) => {
        return api.delete("/api/sightseeing/" + payload);
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

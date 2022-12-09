import api_service from '@/services/api-service'

const api = api_service;

const state = {
    album: null,
    albums: null
};

const getters = {
    getAlbum: state => {
        return state.album;
    },
    getAlbums: state => {
        return state.albums;
    }
};

const mutations = {
    ReceiveAlbum: (state, data) => {
        state.album = data;
    },
    ReceiveAlbums: (state, data) => {
        state.albums = data;
    }
};

const actions = {
    GetById: async (context, payload) => {
        return api.get("/api/album/" + payload.id + "?search=" + payload.search + "&page=" + payload.page +
            "&pageSize=" + payload.size).then((response) => {
                context.commit('ReceiveAlbum', response.data.item1);
                return response.data.item2;
            });
    },
    GetUserAlbums: async (context, payload) => {
        return api.get("/api/album/user/" + payload.userId + "/" + payload.sightseeingId).then((response) => context.commit('ReceiveAlbums', response.data));
    },
    AddSightseeing: async (context, payload) => {
        return api.post("/api/album/add/" + payload.id + "/" + payload.sightseeingId);
    },
    RemoveSightseeing: async (context, payload) => {
        return api.post("/api/album/remove/" + payload.id + "/" + payload.sightseeingId);
    },
    Create: async (context, payload) => {
        return api.post("/api/album", payload);
    },
    Edit: async (context, payload) => {
        return api.put("/api/album", payload);
    },
    Delete: async (context, payload) => {
        return api.delete("/api/album/" + payload);
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};

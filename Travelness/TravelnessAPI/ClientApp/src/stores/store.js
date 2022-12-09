import Vue from 'vue';
import Vuex from 'vuex';
import account from '@/stores/account';
import user from '@/stores/user'
import sightseeing from '@/stores/sightseeing'
import tour from '@/stores/tour'
import rate from '@/stores/rate'
import album from '@/stores/album'

Vue.use(Vuex);

export const store = new Vuex.Store({
    state: {},
    getters: {},
    mutations: {},
    actions: {},
    modules: {
        account,
        user,
        sightseeing,
        tour,
        rate,
        album
    }
});

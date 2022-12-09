import Vue from 'vue'
import App from './App.vue'
import router from './router'
import Vuex from 'vuex'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import { store } from '@/stores/store'
import Vuelidate from 'vuelidate'
import VueCompositionAPI from '@vue/composition-api'

Vue.use(Vuex)
Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(Vuelidate)
Vue.use(VueCompositionAPI)

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.config.productionTip = false

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')

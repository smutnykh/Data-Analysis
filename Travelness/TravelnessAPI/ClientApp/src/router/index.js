import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Profile from '../views/Profile.vue'
import SightseeingShow from '../views/Sightseeing/SightseeingShow.vue'
import SightseeingCreate from '../views/Sightseeing/SightseeingCreate.vue'
import SightseeingEdit from '../views/Sightseeing/SightseeingEdit.vue'
import TourShow from '../views/Tour/TourShow.vue'
import TourCreate from '../views/Tour/TourCreate.vue'
import TourEdit from '../views/Tour/TourEdit.vue'
import AlbumShow from '../views/Album/AlbumShow.vue'
import Login from '../views/Auth/Login.vue'
import Register from '../views/Auth/Register.vue'
import RegisterConfirmationInfo from '../views/Auth/RegisterConfirmationInfo.vue'
import EmailConfirmation from '../views/Auth/EmailConfirmation.vue'
import Error404 from '../views/Error/Error404.vue'
import { store } from '@/stores/store';
import constants from '@/constants.js'

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/profile/:id',
        name: 'Profile',
        component: Profile
    },
    {
        path: '/sightseeing/:id',
        name: 'SightseeingShow',
        component: SightseeingShow
    },
    {
        path: '/create/sightseeing',
        name: 'SightseeingCreate',
        component: SightseeingCreate,
        meta: { requiresAdmin: true }
    },
    {
        path: '/edit/sightseeing/:id',
        name: 'SightseeingEdit',
        component: SightseeingEdit,
        meta: { requiresAdmin: true }
    },
    {
        path: '/tour/:id',
        name: 'TourShow',
        component: TourShow
    },
    {
        path: '/create/tour',
        name: 'TourCreate',
        component: TourCreate,
        meta: { requiresBusinessUser: true }
    },
    {
        path: '/edit/tour/:id',
        name: 'TourEdit',
        component: TourEdit,
        meta: { requiresBusinessUser: true }
    },
    {
        path: '/album/:id',
        name: 'AlbumShow',
        component: AlbumShow
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    },
    {
        path: '/register',
        name: 'Register',
        component: Register
    },
    {
        path: '/registerConfirmationInfo',
        name: 'RegisterConfirmationInfo',
        component: RegisterConfirmationInfo
    },
    {
        path: '/emailConfirmation',
        name: 'EmailConfirmation',
        component: EmailConfirmation
    },
    {
        path: "*",
        component: Error404
    }
]

const router = new VueRouter({
    mode: 'history',
    routes
})

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!store.getters['account/isAuthenticated']) {
            next({
                path: '/login'
            })
        }
        else {
            next();
        }
    } else {
        next();
    }
});

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAdmin)) {
        let user = store.getters['account/currentUser'];
        if (user == null || user.role != constants.ADMIN_ROLE) {
            next({
                //cant do from.fullPath due to Uncaught (in promise) Error: Redirected from "/login" to "/" via a navigation guard.
                path: '/'
            })
        }
        else {
            next();
        }
    } else {
        next();
    }
});

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresBusinessUser)) {
        let user = store.getters['account/currentUser'];        
        if (user == null || user.role != constants.BUSINESS_USER_ROLE) {
            next({
                //cant do from.fullPath due to Uncaught (in promise) Error: Redirected from "/login" to "/" via a navigation guard.
                path: '/'
            })
        }
        else {
            next();
        }
    } else {
        next();
    }
});

export default router

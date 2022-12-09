import axios from 'axios';
import store from '@/stores/account';
import router from '@/router/index'

const service = axios.create({
    baseURL: 'https://localhost:44342',
    timeout: 15000
});

service.interceptors.request.use(
    (config) => {
        const token = store.state.token;
        if (token) {
            // eslint-disable-next-line no-param-reassign
            config.headers.Authorization = "Bearer " + token;
        }
        return config;
    },
    (error) => {
        console.log(error);
        return Promise.reject(error);
    }
);

//service.interceptors.response.use(
//    (response) => {     
//        return response;
//    },
//    (error) => {
//        if (error.response.status !== 401) {
//            return new Promise((resolve, reject) => {
//                reject(error);
//            });
//        }
//        else {
//            router.push('/login');
//            return new Promise((resolve, reject) => {
//                reject(error);
//            });
//        }
//    }
//);

export default service;

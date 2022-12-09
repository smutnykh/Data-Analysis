<template>
    <div id="app">
        <nav class="navbar navbar-expand-lg">
            <a class="navbar-brand" href="/">Travel<span>ness</span></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarColor03" aria-controls="navbarColor03" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarColor03">
                <ul class="navbar-nav mr-auto">
                    
                </ul>
                <button v-if="isAdmin" class="btn btn btn-outline-light my-2 my-sm-0 ml-2" v-on:click="$router.push({path: '/create/sightseeing'})">Додати пам'ятку</button>
                <button class="btn btn-outline-light" v-if="isAuthenticated && !isAdmin" v-on:click="$router.push({ name: 'Profile', params : { id : currentUser.id}})">
                   {{currentUser.username}}
                </button>
                <button v-if="!isAuthenticated" class="btn btn btn-outline-light my-2 my-sm-0 ml-2" v-on:click="$router.push({ name: 'Login'})">Увійти</button>
                <button v-if="!isAuthenticated" class="btn btn btn-outline-light my-2 my-sm-0 ml-2" v-on:click="$router.push('Register')">Зареєструватися</button>
                <form class="form-inline my-2 my-lg-0 ml-2" v-if="isAuthenticated" novalidate @submit.prevent="logout">
                    <button class="btn btn-outline-light my-2 my-sm-0" type="submit">Вийти</button>
                </form>
            </div>
        </nav>
        <router-view />
    </div>
</template>
<script>
    import { mapGetters } from 'vuex';
    import constants from '@/constants.js'

    export default {
        computed: {
            ...mapGetters({
                isAuthenticated: 'account/isAuthenticated',
                currentUser: 'account/currentUser'
            }),
            isAdmin() {
                if (this.currentUser)
                    return this.currentUser.role == constants.ADMIN_ROLE;
                else
                    return false;
            }
        },
        methods: {
            logout: function () {
                this.$store.dispatch("account/Logout", { root: true })
                    .then(() => this.successfulLogout())
                    .catch((error) => this.unsuccessfulLogout(error));
            },
            successfulLogout: function () {
                this.errorMessage = null;
                this.$router.push("/login");
            },
            unsuccessfulLogout: function (data) {
                let str = "";
                for (let property in data.response) {
                    if (Object.prototype.hasOwnProperty.call(data.response, property) && property != "code") {
                        str += data.reponse[property] + '\n';
                    }
                }
                this.errorMessage = str;
            }
        }
    }
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }

    .navbar-brand, .navbar-brand:hover {
        font-weight: bold;
        font-style: italic;
        color: white;
    }

    .navbar-brand span {
        color: #EC1F46;
    }

    nav {
        background-color: #0a213f;
    }

    #nav {
        padding: 30px;
    }

        #nav a {
            font-weight: bold;
            color: #2c3e50;
        }

            #nav a.router-link-exact-active {
                color: #42b983;
            }


    .crud-image {
        width: 24px;
        height: 24px;
    }

        .crud-image:hover {
            cursor: pointer;
        }


    .error {
        padding: 0 6px;
        color: #dc3545;
        text-align: left;
        font-size: 0.75rem;
    }

    .btn-danger {
        padding-left: 25px;
        padding-right: 25px;
        font-size: 14px;
        background-color: #EC1F46;
    }
</style>
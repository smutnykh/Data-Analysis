<template>
    <div class="container">
        <div class="card border-dark mb-3 rounded col-6 pl-0 pr-0 mr-auto ml-auto">
            <div class="card-header text-center">Вхід у систему</div>
            <div class="card-body">
                <form novalidate @submit.prevent="login">
                    <div class="input-group mb-0 mt-2">
                        <input name="Email" type="email" class="form-control" placeholder="Email..." v-model.lazy="$v.Email.$model" v-on:keydown.enter.prevent>
                    </div>
                    <div v-if="$v.Email.$anyDirty">
                        <div class="error" v-if="!$v.Email.required">Email обо'язковий</div>
                        <div class="error" v-if="!$v.Email.email">Email невалідний</div>
                    </div>

                    <div class="form-group mb-0 mt-2">
                        <input name="Password" type="password" class="form-control" placeholder="Пароль..." v-model="$v.Password.$model" v-on:keydown.enter.prevent>
                    </div>
                    <div v-if="$v.Password.$anyDirty">
                        <div class="error" v-if="!$v.Password.required">Пароль обовя'зковий</div>
                    </div>

                    <div class="form-group">
                        <input type="hidden" class="form-control is-invalid">
                        <div class="invalid-feedback">
                            {{errorMessage}}
                        </div>
                    </div>
                    <div class="row">
                        <router-link class="col text-center" to="/">
                            <button class="w-50 btn btn-dark">На головну</button>
                        </router-link>
                        <div class="col text-center">
                            <button class="w-50 btn btn-dark" type="submit">Увійти</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    import { required, email } from "vuelidate/lib/validators";

    export default {
        name: "Login",
        data() {
            return {
                Email: null,
                Password: null,
                errorMessage: null,
            }
        },
        validations: {
            Email: { required, email },
            Password: { required },
        },
        watch: {
            Email: function (val) {
                this.errorMessage = null;
            },
            Password: function (val) {
                this.errorMessage = null;
            }
        },
        methods: {
            login: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Invalid input";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("account/Login", formData, { root: true })
                        .then(() => this.successfulLogin())
                        .catch((error) => this.unsuccessfulLogin(error));
                }
            },
            successfulLogin: function () {
                this.errorMessage = null;
                this.$router.push("/");
            },
            unsuccessfulLogin: function (error) {
                this.errorMessage = error.response.data;
            }
        }
    }
</script>

<style scoped>
    .container {
        padding-top: 100px;
        padding-bottom: 100px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 100%;
    }

    label {
        display: block;
    }

    .card {
        border-width: 2px;
        padding-right: 0;
        padding-left: 0;
    }

    .col-6 {
        flex: none;
    }
</style>

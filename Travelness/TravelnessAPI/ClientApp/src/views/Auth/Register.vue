<template>
    <div class="container">
        <div class="card border-dark mb-3 rounded col-6 pl-0 pr-0 mr-auto ml-auto">
            <div class="card-header text-center">Реєстрування у системі</div>
            <div class="card-body">
                <form novalidate @submit.prevent="register">
                    <div>
                        <div class="form-group mb-0">
                            <input required name="Email" class="form-control" type="email" v-model.lazy="$v.Email.$model" placeholder="Email..." v-on:keydown.enter.prevent />
                        </div>
                        <div v-if="$v.Email.$anyDirty">
                            <div class="error" v-if="!$v.Email.required">Email обов'язковий</div>
                            <div class="error" v-if="!$v.Email.email">Email невалідний</div>
                        </div>


                        <div class="form-group mb-0 mt-2">
                            <input required name="Username" class="form-control" v-model="$v.Username.$model" placeholder="Ім'я користувача..." v-on:keydown.enter.prevent />
                        </div>
                        <div v-if="$v.Username.$anyDirty">
                            <div class="error" v-if="!$v.Username.required">Ім'я користувача обов'язкове</div>
                        </div>


                        <div class="form-group mb-0 mt-2">
                            <input required name="Password" class="form-control" type="password" v-model="$v.Password.$model" placeholder="Пароль..." v-on:keydown.enter.prevent />
                        </div>
                        <div v-if="$v.Password.$anyDirty">
                            <div class="error" v-if="!$v.Password.required">Пароль обов'язковий</div>
                            <div class="error" v-if="!$v.Password.minLength">Мінімальна довжина поля {{$v.Password.$params.minLength.min}} символів.</div>
                        </div>


                        <div class="form-group mb-0 mt-2">
                            <input required name="RepeatPassword" class="form-control" type="password" v-model="$v.RepeatPassword.$model" placeholder="Повторіть пароль..." v-on:keydown.enter.prevent />
                        </div>
                        <div v-if="$v.RepeatPassword.$anyDirty">
                            <div class="error" v-if="!$v.RepeatPassword.required">Повторення паролю обов'язкове</div>
                            <div class="error" v-if="!$v.RepeatPassword.sameAsPassword">Паролі не співпадають</div>
                        </div>


                        <div class="form-group mb-0 mt-2">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="role" id="standartUserRadio" value="StandartUser" v-model="$v.Role.$model">
                                <label class="form-check-label" for="standartUserRadio">
                                    Звичайний акаунт
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="role" id="businessUserRadio" value="BusinessUser" v-model="$v.Role.$model">
                                <label class="form-check-label" for="businessUserRadio">
                                    Бізнес акаунт
                                </label>
                            </div>
                        </div>
                        <div v-if="$v.Role.$anyDirty">
                            <div class="error" v-if="!$v.Role.required">Вибір акаунту обов'язковий</div>
                        </div>


                        <div class="form-group">
                            <input type="hidden" class="form-control is-invalid">
                            <div class="invalid-feedback">
                                {{errorMessage}}
                            </div>
                        </div>
                        <div class="row">
                            <div class="col text-center">
                                <button class="btn btn-block btn-dark" type="submit">Зареєструватися</button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    import { required, minLength, sameAs, email } from 'vuelidate/lib/validators'

    export default {
        name: "Register",
        data: function () {
            return {
                Email: null,
                Username: null,
                Role: null,
                Password: null,
                RepeatPassword: null,
                errorMessage: null,
            }
        },
        validations: {
            Email: {
                required,
                email
            },
            Username: {
                required
            },
            Role: {
                required
            },
            Password: {
                required,
                minLength: minLength(8)
            },
            RepeatPassword: {
                required,
                minLength: minLength(8),
                sameAsPassword: sameAs('Password')
            }
        },
        watch: {
            Email: function (val) {
                this.errorMessage = null;
            },
            Username: function (val) {
                this.errorMessage = null;
            },
            Password: function (val) {
                this.errorMessage = null;
            },
            RepeatPassword: function (val) {
                this.errorMessage = null;
            }
        },
        methods: {
            register: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введені дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("account/Register", formData, { root: true })
                        .then(() => this.successfulRegister())
                        .catch((error) => this.unsuccessfulRegister(error));
                }
            },
            successfulRegister: function () {
                this.errorMessage = null;
                this.$router.push("/registerConfirmationInfo");
            },
            unsuccessfulRegister: function (error) {
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

    .form-check {
        text-align: left;
    }
</style>
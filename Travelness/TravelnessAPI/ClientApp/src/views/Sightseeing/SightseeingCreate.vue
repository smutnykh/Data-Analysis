<template>
    <div class="container">
        <h1 class="mt-3 mb-3">Створення туристичного місця</h1>
        <form novalidate @submit.prevent="create">
            <div class="input-group mb-0 mt-2">
                <input name="Name" type="text" class="form-control" placeholder="Назва..." v-model="$v.Name.$model" v-on:keydown.enter.prevent>
            </div>
            <div v-if="$v.Name.$anyDirty">
                <div class="error" v-if="!$v.Name.required">Назва обов'язкова</div>
                <div class="error" v-if="!$v.Name.maxLength">Максимальна довжина поля 100 символів</div>
            </div>

            <div class="form-group mb-0 mt-2">
                <textarea name="Description" type="text" class="form-control" placeholder="Опис..." v-model="$v.Description.$model" v-on:keydown.enter.prevent></textarea>
            </div>
            <div v-if="$v.Description.$anyDirty">
                <div class="error" v-if="!$v.Description.required">Опис обов'язковий</div>
                <div class="error" v-if="!$v.Description.maxLength">Максимальна довжина поля 500 символів</div>
            </div>

            <div class="form-group mb-0 mt-2">
                <h5 class="text-left">Країна</h5>
                <select name="Country" class="form-control" v-model="$v.Country.$model">
                    <option disabled selected>Оберіть країну</option>
                    <option v-for="item in countries">
                        {{item}}
                    </option>
                </select>
            </div>
            <div v-if="$v.Country.$anyDirty">
                <div class="error" v-if="!$v.Country.required">Країна обов'язкова</div>
            </div>

            <div class="form-group mb-0 mt-2">
                <h5 class="text-left">Локація</h5>
                <select name="Area" class="form-control" v-model="$v.Area.$model">
                    <option disabled selected>Оберіть локацію</option>
                    <option v-for="item in areas">
                        {{item}}
                    </option>
                </select>
            </div>
            <div v-if="$v.Area.$anyDirty">
                <div class="error" v-if="!$v.Area.required">Локація обов'язкова</div>
            </div>

            <img class="mt-4" :src="Src" alt="Оберіть зображення..." />
            <div class="input-group mt-3">
                <div class="custom-file">
                    <input name="Image" type="file" class="custom-file-input" @change="onFileChange">
                    <label class="custom-file-label">Оберіть зображення...</label>
                </div>
            </div>

            <div v-if="$v.Image.$anyDirty">
                <div class="error" v-if="!$v.Image.required">Зображення обов'язкове</div>
            </div>

            <div class="form-group">
                <input type="hidden" class="form-control is-invalid">
                <div class="invalid-feedback">
                    {{errorMessage}}
                </div>
            </div>
            <div class="row">
                <div class="col text-center">
                    <button class="btn btn-dark" type="submit">Готово</button>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
    import { required, maxLength } from "vuelidate/lib/validators";
    import constants from '@/constants.js'

    export default {
        name: "SightseeingCreate",
        data() {
            return {
                Name: null,
                Description: null,
                Country: null,
                Area: null,
                Image: null,
                Src: null,
                errorMessage: null,
                countries: constants.COUNTRIES,
                areas: constants.AREAS 
            }
        },
        validations: {
            Name: { required, maxLength: maxLength(100)},
            Description: { required, maxLength: maxLength(500) },
            Country: { required },
            Area: { required },
            Image: { required }
        },
        watch: {
            Name: function (val) {
                this.errorMessage = null;
            },
            Description: function (val) {
                this.errorMessage = null;
            },
            Country: function (val) {
                this.errorMessage = null;
            },
            Area: function (val) {
                this.errorMessage = null;
            }
        },
        methods: {
            onFileChange(e) {
                this.Image = e.target.files[0];
                this.Src = URL.createObjectURL(this.Image);
            },
            create: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введені дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("sightseeing/Create", formData, { root: true })
                        .then(() => this.successfulCreate())
                        .catch((error) => this.unsuccessfulCreate(error));
                }
            },
            successfulCreate: function () {
                this.errorMessage = null;
                this.$router.push("/");
            },
            unsuccessfulCreate: function (error) {
                this.errorMessage = error.response.data;
            }
        }
    }
</script>

<style scoped>
    img{
        width: 70%;
    }
</style>
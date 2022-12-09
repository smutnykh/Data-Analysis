<template>
    <div class="container">
        <h1 class="mt-3 mb-3">Редагування туристичного місця</h1>
        <form novalidate @submit.prevent="edit">
            <input type="hidden" name="Id" :value="sightseeing.id" />
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
                <b-form-select name="Country" v-model="Country" :options="countryOptions">

                </b-form-select>
            </div>

            <div class="form-group mb-0 mt-2">
                <h5 class="text-left">Локація</h5>
                <b-form-select name="Area" v-model="Area" :options="areaOptions">

                </b-form-select>
            </div>

            <img :src="Src" alt="Оберіть зображення..." class="mt-4" />
            <div class="input-group mt-3">
                <div class="custom-file">
                    <input name="Image" type="file" class="custom-file-input" @change="onFileChange">
                    <label class="custom-file-label">Оберіть зображення...</label>
                </div>
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
    import { mapGetters } from 'vuex';
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
                countryOptions: [],
                areaOptions: []
            }
        },
        validations: {
            Name: { required, maxLength: maxLength(100) },
            Description: { required, maxLength: maxLength(500) },
            Country: { required },
            Area: { required }
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
        computed: {
            ...mapGetters({
                sightseeing: 'sightseeing/getSightseeing',
            })
        },
        mounted() {
            this.$store.dispatch("sightseeing/GetById", this.$route.params.id).then(() => this.fillForm()).catch(error => {
                if (error.response && error.response.status === 400) {
                    this.$router.push("/");
                }
            });
        },
        methods: {
            onFileChange(e) {
                this.Image = e.target.files[0];
                this.Src = URL.createObjectURL(this.Image);
            },
            edit: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введені дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("sightseeing/Edit", formData, { root: true })
                        .then(() => this.successfulEdit())
                        .catch((error) => this.unsuccessfulEdit(error));
                }
            },
            successfulEdit: function () {
                this.errorMessage = null;
                this.$router.push({ name: 'SightseeingShow', params: { id: this.sightseeing.id } })
            },
            unsuccessfulEdit: function (error) {
                this.errorMessage = error.response.data;
            },
            fillForm() {
                this.Name = this.sightseeing.name;
                this.Description = this.sightseeing.description;
                this.Country = this.sightseeing.country;
                this.Area = this.sightseeing.area;
                this.Src = constants.SIGHTSEEING_IMAGE_PATH + this.sightseeing.image;
                constants.COUNTRIES.forEach(element => this.countryOptions.push({ value: element, text: element }))
                constants.AREAS.forEach(element => this.areaOptions.push({ value: element, text: element }))
            }
        }
    }
</script>

<style scoped>
    img {
        width: 70%;
    }
</style>
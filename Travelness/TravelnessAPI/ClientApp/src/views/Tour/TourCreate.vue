<template>
    <div class="container">
        <h1 class="mt-3 mb-3">Створення туру</h1>
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
                <input name="Price" type="number" class="form-control" placeholder="Ціна..." v-model="$v.Price.$model" v-on:keydown.enter.prevent>
            </div>
            <div v-if="$v.Price.$anyDirty">
                <div class="error" v-if="!$v.Price.required">Ціна обов'язкова</div>
                <div class="error" v-if="!$v.Price.minValue">Ціна має бути додатнім числом</div>
            </div>

            <div class="form-group mb-0 mt-2">
                <h5 class="text-left">Туристичні місця</h5>
                <b-form-select name="SightseeingsIds" v-model="SightseeingsIds" :options="options"
                               multiple :select-size="5">
                </b-form-select>
            </div>
            <div v-if="$v.SightseeingsIds.$anyDirty">
                <div class="error" v-if="!$v.SightseeingsIds.required">Виберіть хоча б одне туристичне місце</div>
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
    import { required, minValue, maxLength } from "vuelidate/lib/validators";
    import { mapGetters } from 'vuex';

    export default {
        name: "TourCreate",
        data() {
            return {
                Name: null,
                Description: null,
                Price: null,
                SightseeingsIds: [],
                options: [],
                errorMessage: null
            }
        },
        validations: {
            Name: { required, maxLength: maxLength(100) },
            Description: { required, maxLength: maxLength(500) },
            Price: {
                required,
                minValue: minValue(0)
            },
            SightseeingsIds: { required }
        },
        watch: {
            Name: function (val) {
                this.errorMessage = null;
            },
            Description: function (val) {
                this.errorMessage = null;
            },
            Price: function (val) {
                this.errorMessage = null;
            },
            SightseeingsIds: function (val) {
                this.errorMessage = null;
            }
        },
        computed: {
            ...mapGetters({
                sightseeings: 'sightseeing/getSightseeings',
                currentUser: 'account/currentUser'
            })
        },
        mounted() {
            this.$store.dispatch("sightseeing/GetAll", this.$route.params.id).then(() =>
                this.sightseeings.forEach(element => this.options.push({ value: element.id, text: element.name }))
            );
        },
        methods: {
            create: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введенні дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("tour/Create", formData, { root: true })
                        .then(() => this.successfulCreate())
                        .catch((error) => this.unsuccessfulCreate(error));
                }
            },
            successfulCreate: function () {
                this.errorMessage = null;
                this.$router.push({ name: 'Profile', params: { id: this.currentUser.id } });
            },
            unsuccessfulCreate: function (error) {
                this.errorMessage = error.response.data;
            }
        }
    }
</script>

<style scoped>
</style>
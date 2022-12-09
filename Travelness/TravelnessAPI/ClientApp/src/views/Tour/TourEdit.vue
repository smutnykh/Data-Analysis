<template>
    <div class="container">
        <h1 class="mt-3 mb-3">Редагування туру</h1>
        <form novalidate @submit.prevent="edit">
            <input type="hidden" name="Id" :value="tour.id" />
            <div class="input-group mb-0 mt-2">
                <input name="Name" type="text" class="form-control" placeholder="Назва..." 
                       v-model="$v.Name.$model" v-on:keydown.enter.prevent>
            </div>
            <div v-if="$v.Name.$anyDirty">
                <div class="error" v-if="!$v.Name.required">Назва обов'язкова</div>
                <div class="error" v-if="!$v.Name.maxLength">Максимальна довжина поля 100 символів</div>
            </div>

            <div class="form-group mb-0 mt-2">
                <textarea name="Description" type="text" class="form-control" placeholder="Опис..." 
                       v-model="$v.Description.$model" v-on:keydown.enter.prevent></textarea>
            </div>
            <div v-if="$v.Description.$anyDirty">
                <div class="error" v-if="!$v.Description.required">Опис обов'язковий</div>
                <div class="error" v-if="!$v.Description.maxLength">Максимальна довжина поля 500 символів</div>
            </div>

            <div class="form-group mb-0 mt-2">
                <input name="Price" type="number" class="form-control" placeholder="Ціна..." 
                       v-model="$v.Price.$model" v-on:keydown.enter.prevent>
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
        name: "SightseeingCreate",
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
                tour: 'tour/getTour'
            })
        },
        mounted() {
            this.$store.dispatch("sightseeing/GetAll", this.$route.params.id)
                .then(() => this.$store.dispatch("tour/GetById", this.$route.params.id)
                    .then(() => this.fillForm())).catch(error => {
                        if (error.response && error.response.status === 400) {
                            this.$router.push("/");
                        }
                    });       
        },
        methods: {
            edit: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введені дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("tour/Edit", formData, { root: true })
                        .then(() => this.successfulEdit())
                        .catch((error) => this.unsuccessfulEdit(error));
                }
            },  
            successfulEdit: function () {
                this.errorMessage = null;
                this.$router.push({ name: 'TourShow', params: { id: this.tour.id } });
            },
            unsuccessfulEdit: function (error) {
                this.errorMessage = error.response.data;
            },
            fillForm() {
                this.Name = this.tour.name;
                this.Description = this.tour.description;
                this.Price = this.tour.price;
                this.SightseeingsIds = [];
                this.tour.sightseeings.forEach(element => this.SightseeingsIds.push(element.id));
                this.sightseeings.forEach(element => this.options.push({ value: element.id, text: element.name }))
            }
        }
    }
</script>

<style scoped>
</style>
<template>
    <div class="container">
        <div class="row">
            <div class="col-2">
                <h3 class="mt-3 text-left">Фільтри</h3><hr>
                <div class="form-group">
                    <h4 class="mt-3 text-left" v-b-toggle.collapse-country>Країна</h4>
                    <b-collapse visible id="collapse-country">
                        <div class="form-check pl-0 text-left" v-for="item in countries" @change="search">
                            <b-form-checkbox size="sm" :id="item" v-model="selectedCountries" :value="item">
                                {{item}}
                            </b-form-checkbox>
                        </div>
                    </b-collapse>
                </div><hr>
                <div class="form-group">
                    <h4 class="mt-3 text-left" v-b-toggle.collapse-area>Тип</h4>
                    <b-collapse visible id="collapse-area">
                        <div class="form-check pl-0 text-left" v-for="item in areas" @change="search">
                            <b-form-checkbox size="sm" :id="item" v-model="selectedAreas" :value="item">
                                {{item}}
                            </b-form-checkbox>
                        </div>
                    </b-collapse>
                </div><hr>
            </div>
            <div class="col-10">
                <div class="input-group mb-3 mt-3">
                    <input type="text" class="form-control" placeholder="Пошук за назвою..." v-model="searchTitle" />
                    <div class="input-group-append">
                        <button class="btn btn-outline-danger" type="button" @click="search"> Пошук </button>
                    </div>
                </div>
                <div v-if="isNoRecords">
                    <h1 class="mt-5">На жаль, нічого не знайдено</h1>
                </div>
                <div v-else>
                    <div class="row mt-3">
                        <div class="col-4" v-for="item in sightseeings" :key="item.id">
                            <sightseeingCard v-bind:sightseeing="item">

                            </sightseeingCard>
                        </div>
                    </div>
                    <b-pagination v-model="page" :total-rows="count" :per-page="pageSize" prev-text="‹"
                                  next-text="›" @change="onPageChange">
                    </b-pagination>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import sightseeingCard from '@/components/SightseeingCard'
    import constants from '@/constants.js';

    export default {
        name: 'Home',
        components: {
            sightseeingCard
        },
        data() {
            return {
                page: 1,
                count: null,
                pageSize: 6,
                searchTitle: "",
                countries: constants.COUNTRIES,
                areas: constants.AREAS,
                selectedCountries: [],
                selectedAreas: []
            }
        },
        computed: {
            ...mapGetters({
                sightseeings: 'sightseeing/getSightseeings',
            }),
            isNoRecords() {
                return this.count < 1;
            }
        },
        mounted() {
            this.getByPage(1);
        },
        methods: {
            getByPage(page) {
                this.$store.dispatch("sightseeing/GetByPage", {
                    countries: this.selectedCountries, areas: this.selectedAreas,
                    search: this.searchTitle, page, size: this.pageSize
                }).then(
                    (response) => this.setCount(response)
                );
            },

            onPageChange(value) {
                this.getByPage(value);
            },

            search() {
                this.getByPage(1);
                this.page = 1;
            },

            setCount(count) {
                this.count = count;
            }
        }
    }
</script>

<style scoped>
    .pagination {
        justify-content: center
    }

    .form-check-input {
        width: 15px;
        height: 15px;
    }
</style>

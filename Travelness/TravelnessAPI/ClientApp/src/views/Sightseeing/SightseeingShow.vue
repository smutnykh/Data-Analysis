<template>
    <div class="container mt-4 mb-4">
        <div class="row">
            <div class="col-10 text-left">
                <h3 class="mb-0">
                    {{sightseeing.name.toUpperCase()}}
                    <img v-if="isAdmin" class="crud-image ml-2 mb-1" v-on:click="$router.push({ name: 'SightseeingEdit', params : { id : sightseeing.id}})" src="@/assets/edit.png">
                    <img v-if="isAdmin" class="crud-image ml-2 mb-1" v-on:click="remove" src="@/assets/delete.png">
                    <img v-if="isStandartUser" class="crud-image mb-1" v-on:click="showAddSightseeingModal" src="@/assets/add.png">                  
                </h3>
                 <b-modal size="sm" id="addSightseeingModal" hide-footer title="Додання пам'ятки">
                        <div v-if="isAnyAlbum">
                            <h5>Додати до альбому: </h5>
                            <hr class="mt-0 mb-0" />
                            <div class="row" v-for="item in albums" :key="item.id">
                                <div class="col-7">
                                    <p class="mt-2">{{item.name}}</p>
                                </div>
                                <div class="col-5 text-right">
                                    <button v-if="item.sightseeings.length" class="btn btn-outline-danger mt-1" v-on:click="removeSightseeing(item.id)">Видалити</button>
                                    <button v-else class="btn btn-danger mt-1" v-on:click="addSightseeing(item.id)">Додати</button>
                                </div>
                                <div class="col-12">
                                    <hr class="mt-0 mb-0" />
                                </div>
                            </div>
                        </div>
                        <div v-else>
                            <h5>Альбоми відсутні</h5>
                        </div>
                    </b-modal>
                <p class="mb-3">{{sightseeing.country}}</p>
            </div>
            <div class="col-2 text-right pr-0">
                <button v-if="isAnyRate" class="btn btn-danger" v-b-modal.ratingModal>
                    Середня оцінка <br /> <h4 class="mb-0">{{averageRate}}/<small>5</small></h4>
                </button>
                <button v-else class="btn btn-danger" v-b-modal.ratingModal>
                    Відсутні оцінки
                </button>
                <b-modal size="sm" id="ratingModal" hide-footer title="Оцінювання">
                    <div v-if="isStandartUser">
                        <h5>Ваша оцінка: </h5>
                        <image-rating :item-size="30" v-model="rating" src="/images/default/star.png">

                        </image-rating>
                        <button class="btn btn-danger mt-3" v-on:click="removeRating">Прибрати оцінку</button>
                    </div>
                    <div v-else>
                        <h5>Для оцінювання увійдіть як звичайний користувач</h5>
                    </div>
                </b-modal>
            </div>
            <div class="col-8">
                <img class="side-image" :src="sightseeingImagePath" alt="...">
            </div>
            <div class="card col-4">
                <div class="card-body text-left">
                    <h4 class="card-title">
                        {{sightseeing.name}}
                    </h4>
                    <p class="card-text">
                        {{sightseeing.description}}
                    </p>
                    <div class="row">
                        <div class="col-6">
                            <small>Тури починаються з</small>
                            <h3>$ {{slider.min}}</h3>
                        </div>
                        <div class="col-6">
                            <img src="@/assets/mountain.png" class="icon" />
                            {{sightseeing.area}}
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <h3 class="text-left mt-4 mb-4">Тури</h3><hr />
        <div class="row">
            <div class="col-2">
                <div class="form-group">
                    <h5 class="text-left mb-5">Ціна на тур</h5>
                    <slider class="text-center slider mr-auto ml-auto" v-model="slider.value" :format="slider.format" :min="slider.min"
                            :max="slider.max" :disabled="slider.disabled" @change="loadData(1)"></slider>
                </div><hr />
                <div class="form-group">
                    <h5 class="mt-3 text-left" v-b-toggle.collapse-company>Компанія</h5>
                    <b-collapse visible id="collapse-company">
                        <div class="form-check pl-0 text-left" v-for="item in companies" @change="loadData(1)">
                            <b-form-checkbox size="sm" :id="item" v-model="selectedCompanies" :value="item">
                                {{item}}
                            </b-form-checkbox>
                        </div>
                    </b-collapse>
                </div><hr />
            </div>
            <div class="col-10" v-if="isNoRecords">
                <h3>На жаль, нічого не знайдено</h3>
            </div>
            <div class="col-10" v-else>
                <div class="row">
                    <div class="col-9 pr-0 mt-1 text-right">
                        Сортування:
                    </div>
                    <div class="col-3 mb-3">
                        <b-form-select v-model="order" :options="orderOptions" @change="loadData(1)"></b-form-select>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6" v-for="item in sightseeing.tours" :key="item.id">
                        <tourCard v-bind:tour="item">

                        </tourCard>
                    </div>
                </div>
                <b-pagination v-model="page" :total-rows="count" :per-page="pageSize" prev-text="‹"
                              next-text="›" @change="onPageChange">
                </b-pagination>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import constants from '@/constants.js'
    import tourCard from '@/components/TourCard'
    import slider from '@vueform/slider/dist/slider.vue2.js'
    import { ImageRating } from 'vue-rate-it';

    export default {
        name: 'SightseeingShow',
        components: {
            tourCard,
            slider,
            'image-rating': ImageRating
        },
        data() {
            return {
                slider: {
                    min: 0,
                    max: 100000,
                    value: [0, 100000],
                    format: function (value) {
                        return `$${value}`
                    },
                    disabled: false
                },
                page: 1,
                pageSize: 2,
                count: null,
                companies: [],
                selectedCompanies: [],
                orderOptions: [
                    { value: "none", text: "Ніякого" },
                    { value: "ascending", text: "За зростанням ціни" },
                    { value: "descending", text: "За спаданням ціни" },
                ],
                order: "none",
                rating: 0
            }
        },
        computed: {
            ...mapGetters({
                currentUser: 'account/currentUser',
                sightseeing: 'sightseeing/getSightseeing',
                averageRate: 'rate/getAverageRate',
                userRating: 'rate/getUserRating',
                albums: 'album/getAlbums'
            }),
            sightseeingImagePath() {
                return constants.SIGHTSEEING_IMAGE_PATH + this.sightseeing.image;
            },
            isCurrentUser() {
                return this.$store.state.account.currentUser.id == this.$route.params.id;
            },
            isAdmin() {
                if (this.currentUser)
                    return this.currentUser.role == constants.ADMIN_ROLE;
                else
                    return false;
            },
            isStandartUser() {
                if (this.currentUser)
                    return this.currentUser.role == constants.STANDART_USER_ROLE;
                else
                    return false;
            },
            isNoRecords() {
                return this.count < 1;
            },
            isAnyRate() {
                return this.averageRate != 0;
            },
            isAnyAlbum() {
                if (this.albums)
                    return this.albums.length != 0;
            }
        },
        mounted() {
            this.$store.dispatch("rate/GetAverage", this.$route.params.id, { root: true });
            if (this.isStandartUser)
                this.$store.dispatch("rate/GetUserRating", this.$route.params.id, { root: true }).then(() => this.rating = this.userRating);
            this.loadData(this.page).then((response) => this.setRangeAndCompanies(response.min, response.max, response.allCompanies))
                .catch(error => {
                    if (error.response && error.response.status === 400) {
                        this.$router.push("/");
                    }
                });
        },
        watch: {
            '$route.path': {
                handler(oldUrl, newUrl) {
                    this.setFiltersToDeafault();
                    this.$store.dispatch("rate/GetAverage", this.$route.params.id, { root: true });
                    if (this.isStandartUser)
                        this.$store.dispatch("rate/GetUserRating", this.$route.params.id, { root: true }).then(() => this.rating = this.userRating);
                    this.loadData(this.page).then((response) => this.setRangeAndCompanies(response.min, response.max, response.allCompanies))
                        .catch(error => {
                            if (error.response && error.response.status === 400) {
                                this.$router.push("/");
                            }
                        });
                }
            },
            rating: function () {
                this.$store.dispatch("rate/Post", { sightseeingId: this.$route.params.id, rating: this.rating }, { root: true });
                this.$bvModal.hide('ratingModal');
            }
        },
        methods: {
            remove: function () {
                this.$store.dispatch("sightseeing/Delete", this.sightseeing.id, { root: true })
                    .then(() => this.successfulDelete())
                    .catch((error) => this.unsuccessfulDelete(error));
            },
            successfulDelete: function () {
                this.$router.push("/");
            },
            unsuccessfulDelete: function (error) {
                console.log(error.response.data);
            },
            setCount(count) {
                this.count = count;
            },
            setRangeAndCompanies(min, max, allCompanies) {
                this.slider.disabled = false;
                this.slider.min = min;
                if (min == max) {
                    max++;
                    this.slider.disabled = true;
                }
                this.slider.max = max;

                this.companies = [];
                allCompanies.forEach(element =>
                    this.companies.push(element)
                );
                this.companies = this.companies.filter((x, i, a) => a.indexOf(x) == i);
            },
            loadData(page) {
                return this.$store.dispatch("sightseeing/GetByIdWithTours", {
                    id: this.$route.params.id, companies: this.selectedCompanies,
                    minPrice: this.slider.value[0], maxPrice: this.slider.value[1], order: this.order,
                    page, size: this.pageSize
                }).then(
                    (response) => this.loadDataResponse(response, page))
            },
            loadDataResponse(response, page) {
                this.page = page;
                this.setCount(response.count)
                return response;
            },
            onPageChange(value) {
                this.loadData(value);
            },
            setFiltersToDeafault() {
                this.slider.value = [0, 10000];
                this.slider.min = 0;
                this.slider.max = 10000;
                this.selectedCompanies = [];
                this.order = "none";
            },
            removeRating() {
                this.$store.dispatch("rate/Delete", this.$route.params.id, { root: true }).then(() => this.rating = this.userRating);
                this.$bvModal.hide('ratingModal');
            },
            showAddSightseeingModal() {
                if (this.isStandartUser) {
                    this.$store.dispatch("album/GetUserAlbums", { userId: this.currentUser.id, sightseeingId: this.$route.params.id }, { root: true });
                }
                this.$bvModal.show('addSightseeingModal');
            },
            addSightseeing(albumId) {
                this.$store.dispatch("album/AddSightseeing", { id: albumId, sightseeingId: this.$route.params.id }, { root: true });
                this.$bvModal.hide('addSightseeingModal');
            },
            removeSightseeing(albumId) {
                this.$store.dispatch("album/RemoveSightseeing", { id: albumId, sightseeingId: this.$route.params.id }, { root: true });
                this.$bvModal.hide('addSightseeingModal');
            }

        }
    }
</script>

<style scoped>
    .side-image {
        object-fit: cover;
        height: 100%;
        width: 100%;
        border-radius: 0.25rem;
    }

    .card {
        background-color: #0a213f;
        color: white;
    }

    .icon {
        width: 32px;
        height: 32px;
    }

    .pagination {
        justify-content: center
    }

    .form-check-input {
        width: 15px;
        height: 15px;
    }

    .slider {
        width: 80%;
    }

    .btn-danger {
        background-color: #EC1F46;
    }
</style>

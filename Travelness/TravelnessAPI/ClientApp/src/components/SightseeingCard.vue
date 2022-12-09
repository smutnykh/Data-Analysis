<template>
    <div class="card mb-3">
        <div class="row no-gutters">
            <div class="col-12">
                <img class="image" :src="sightseeingImagePath" alt="...">
            </div>
            <div class="col-12">
                <div class="card-body text-left">
                    <div class="row pl-4">
                        <image-rating :item-size="10" :show-rating="false" :read-only="true" v-model="rate"
                                      :increment="0.01" src="/images/default/star.png">

                        </image-rating>
                    </div>
                    <div class="row pl-4">                        
                        <h5 class="mb-0">
                            <a class="card-text" v-on:click="$router.push({ name: 'SightseeingShow', params : { id : sightseeing.id}})">
                                {{sightseeing.name}}
                            </a>
                        </h5>
                    </div>
                    <div class="row mt-4 pl-4">
                        <img src="@/assets/location.png" class="icon mr-2" />
                        <span>{{sightseeing.country}}</span>
                    </div>
                    <div class="row mt-2 pl-4">
                        <img src="@/assets/mountain.png" class="icon mr-2" />
                        <span>{{sightseeing.area}}</span>
                    </div>
                    <div class="col-12 mt-4 pl-2">
                        <button class="btn btn-danger" v-on:click="$router.push({ name: 'SightseeingShow', params : { id : sightseeing.id}})">Переглянути</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
    import constants from '@/constants.js'
    import { mapGetters } from 'vuex';
    import { ImageRating } from 'vue-rate-it';

    export default {
        name: 'SightseeingCard',       
        props: {
            sightseeing: Object
        },
        components: {
            'image-rating': ImageRating
        },
        data() {
            return {
                rate: 0
            }
        },
        computed: {
            ...mapGetters({
                averageRate: 'rate/getAverageRate',
            }),
            sightseeingImagePath() {
                return constants.SIGHTSEEING_IMAGE_PATH + this.sightseeing.image
            },
        },
        mounted() {
            this.$store.dispatch("rate/GetAverage", this.sightseeing.id, { root: true }).then(() => this.rate = this.averageRate);
        }
    }

</script>

<style scoped>
    .image {
        object-fit: cover;
        width: 100%;
        border-top-right-radius: 0.25rem;
        border-top-left-radius: 0.25rem;
    }

    .card {
        background-color: #0a213f;
        color: white;
    }

    a {
        color: white;
    }

        a:hover {
            color: inherit;
            text-decoration: none;
            cursor: pointer;
        }

    .icon {
        width: 24px;
        height: 24px;
    }

    .btn-danger {
        padding-left: 25px;
        padding-right: 25px;
        font-size: 14px;
        background-color: #EC1F46;
    }

    span {
        font-size: 14px;
    }

    .card{
        border: none;
    }
</style>
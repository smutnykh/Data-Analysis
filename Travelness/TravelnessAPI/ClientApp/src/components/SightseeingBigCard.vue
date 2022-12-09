<template>
    <div class="card mb-3 text-left">
        <div class="row no-gutters">
            <div class="col-4">
                <img class="side-image" :src="sightseeingImagePath" alt="...">
            </div>
            <div class="col-8">
                <div class="card-body pl-4">
                    <image-rating :item-size="10" :show-rating="false" :read-only="true" v-model="rate"
                                   :increment="0.01" src="/images/default/star.png">

                    </image-rating>
                    <h4 class="card-title" v-on:click="$router.push({ name: 'SightseeingShow', params : { id : sightseeing.id}})">
                        <span>{{sightseeing.name}}</span>
                    </h4>
                    <p class="card-text">
                        <small>{{sightseeing.description}}</small>
                    </p>
                    <div class="row">
                        <div class="col-12">
                            <img src="@/assets/location.png" class="icon mr-2" />
                            {{sightseeing.country}}
                            <img src="@/assets/mountain.png" class="icon ml-4 mr-2" />
                            {{sightseeing.area}}
                        </div>
                        <div class="col-12 mt-4 pl-2">
                            <button class="btn btn-danger" v-on:click="$router.push({ name: 'SightseeingShow', params : { id : sightseeing.id}})">Переглянути</button>
                        </div>
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
            }
        },
        mounted() {
            this.$store.dispatch("rate/GetAverage", this.sightseeing.id, { root: true }).then(() => this.rate = this.averageRate);
        }
    }

</script>

<style scoped>
    .side-image {
        object-fit: cover;
        height: 100%;
        width: 100%;
    }

    h4.card-title span:hover {
        cursor: pointer;
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
        }

    .icon {
        width: 24px;
        height: 24px;
    }

    .card{
        border: none;
    }
</style>
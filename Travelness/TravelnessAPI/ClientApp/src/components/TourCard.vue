<template>
    <div class="card mb-3">
        <div class="row">
            <div class="col-12">
                <div class="card-header">
                    <div class="row">
                        <div class="col-8 text-left">
                            <a v-on:click="$router.push({ name: 'TourShow', params : { id : tour.id}})">
                                <span class="tourTitle">{{tour.name}}</span>
                            </a>
                        </div>
                        <div class="col-4 text-right">
                            <span class="tourTitle">$ {{tour.price}}</span>
                        </div>
                    </div>                       
                </div>
                <div class="card-body">                   
                    <div class="row">
                        <div class="col-6 text-left" v-for="item in tour.sightseeings" :key="item.id">
                            <a v-on:click="$router.push({ name: 'SightseeingShow', params : { id : item.id}})">
                                &#8226; {{item.name}}
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
    import constants from '@/constants.js'

    export default {
        name: 'TourCard',
        props: {
            tour: Object
        },
        computed: {
            authorImagePath() {
                if (this.tour.user.profileImage == null)
                    return constants.DEFAULT_PROFILE_IMAGE_PATH;
                else
                    return constants.PROFILE_IMAGE_PATH + this.tour.user.profileImage;
            }
        }
    }

</script>

<style scoped>
    a {
        color: #2c3e50;
        text-decoration: none;
    }

        a:hover {
            cursor: pointer;
        }

    .card {
        color: #2c3e50 !important;
        background-color: white !important;
    }

    .card-header {
        background-color: #0a213f;
        color: white;
    }

    .tourTitle {
        font-size: 20px;
    }

    img {
        margin-bottom: 10px;
        height: 35px;
        width: 35px;
        border-radius: 50%;
        border: 2px solid #fff;
    }
</style>
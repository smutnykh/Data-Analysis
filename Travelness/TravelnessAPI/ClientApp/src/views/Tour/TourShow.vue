<template>
    <div class="container mt-4 mb-4">
        <div class="row">
            <div class="col-12 text-left">
                <h3 class="card-title">
                    {{tour.name.toUpperCase()}}
                    <img v-if="isAuthor" v-on:click="$router.push({ name: 'TourEdit', params : { id : tour.id}})" class="crud-image mr-2 mb-1" src="@/assets/edit.png">
                    <img v-if="isAuthor" v-on:click="remove" class="crud-image mr-2 mb-1" src="@/assets/delete.png">
                </h3>
            </div>          
        </div>
        <div class="row">
            <div class="col-6 text-left">
                <h3 class="card-title">
                    ${{tour.price}}
                </h3>
            </div>
            <div class="col-6 text-right">
                <img class="profile mr-2" :src="authorImagePath" />
                <a v-on:click="$router.push({ name: 'Profile', params : { id : tour.user.id}})">{{tour.user.username}}</a>
            </div>
        </div>
        <div class="card mb-3">
            <div class="row no-gutters">
                <div class="col-12">
                    <div class="card-body">
                        <p class="card-text">
                            {{tour.description}}
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <h3 class="text-left mt-4 mb-4">Туристичні місця</h3>
        <div class="row">
            <div class="col-12" v-for="item in tour.sightseeings" :key="item.id">
                <sightseeingBigCard v-bind:sightseeing="item">

                </sightseeingBigCard>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import sightseeingBigCard from '@/components/SightseeingBigCard'
    import constants from '@/constants.js'

    export default {
        name: 'TourShow',
        components: {
            sightseeingBigCard
        },
        mounted() {
            this.$store.dispatch("tour/GetById", this.$route.params.id).catch(error => {
                if (error.response && error.response.status === 400) {
                    this.$router.push("/");
                }
            });
        },
        computed: {
            ...mapGetters({
                tour: 'tour/getTour',
            }),
            authorImagePath() {
                if (this.tour.user.profileImage == null)
                    return constants.DEFAULT_PROFILE_IMAGE_PATH;
                else
                    return constants.PROFILE_IMAGE_PATH + this.tour.user.profileImage;
            },
            isAuthor() {
                if (this.$store.state.account.currentUser)
                    return this.$store.state.account.currentUser.id == this.tour.user.id;
                else
                    return false;
            }
        },
        watch: {
            '$route.path': {
                handler(oldUrl, newUrl) {
                    this.$store.dispatch("tour/GetById", this.$route.params.id);
                }
            }
        },
        methods: {
            remove: function () {
                this.$store.dispatch("tour/Delete", this.tour.id, { root: true })
                    .then(() => this.successfulDelete())
                    .catch((error) => this.unsuccessfulDelete(error));
            },
            successfulDelete: function () {
                this.$router.push({ name: 'Profile', params: { id: this.tour.user.id } });
            },
            unsuccessfulDelete: function (error) {
                console.log(error.response.data);
            }
        }
    }
</script>

<style scoped>
    img.profile {
        margin-bottom: 10px;
        height: 35px;
        width: 35px;
        border-radius: 50%;
        border: 2px solid #0a213f;
    }

    .card {
        background-color: #0a213f;
        color: white;
    }

    a {
        font-size: 20px;
        color: white;
        text-decoration: none;
    }

        a:hover {
            color: white;
            cursor: pointer;
        }
</style>
<style src="@vueform/slider/themes/default.css"></style>


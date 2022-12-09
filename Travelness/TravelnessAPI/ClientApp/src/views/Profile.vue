<template>
    <div class="container mb-4">
        <div class="row">
            <div class="col-4">
                <header class="header card-section p-4">
                    <div class="details">
                        <img :src="profileImagePath" alt="John Doe" class="profile-pic">
                        <h5 class="text-left mt-4 mb-3">{{user.username}}</h5>
                        <button v-if="isCurrentUser" class="btn btn-outline-success" v-on:click="showModal()">Редагувати</button>
                        <button v-if="isCurrentUser && isBussinessUser" class="btn btn-outline-primary ml-3" v-on:click="$router.push({path: '/create/tour'})">Створити тур</button>
                        <button v-if="isCurrentUser && !isBussinessUser" class="btn btn-outline-primary ml-3" v-b-modal.createAlbumModal>Створити альбом</button>
                        <p class="text-left mt-3">{{user.profileInfo}}</p>

                        <b-modal id="createAlbumModal" hide-footer title="Створення альбому">
                            <form novalidate @submit.prevent="create">
                                <div class="input-group mb-0 mt-2">
                                    <input name="AlbumName" type="text" class="form-control" placeholder="Назва альбому..." v-model="$v.AlbumName.$model" v-on:keydown.enter.prevent>
                                </div>
                                <div v-if="$v.AlbumName.$anyDirty">
                                    <div class="error" v-if="!$v.AlbumName.required">Назва обов'язкова</div>
                                    <div class="error" v-if="!$v.AlbumName.maxLength">Максимальна довжина поля 100 символів</div>
                                </div>
                                <div class="form-group">
                                    <input type="hidden" class="form-control is-invalid">
                                    <div class="invalid-feedback text-center">
                                        {{errorMessage}}
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col text-center">
                                        <button class="btn btn-dark" type="submit">Готово</button>
                                    </div>
                                </div>
                            </form>
                        </b-modal>
                        <profileEditModal v-show="isModalVisible" v-if="isCurrentUser" @closeModal="closeModal"></profileEditModal>
                    </div>
                </header>
            </div>
            <div class="col-8">
                <div class="mt-4 pt-2">
                    <div v-if="isBussinessUser">
                        <h3 class="text-left mt-4 mb-4">Тури</h3>
                        <div v-if="isAnyTour" class="row">
                            <div class="col-6" v-for="item in user.tours" :key="item.id">
                                <tourCard v-bind:tour="item">

                                </tourCard>
                            </div>
                        </div>
                        <div v-else class="row">
                            <div class="col-12">
                                <h2>На жаль, нічого не знайдено</h2>
                            </div>
                        </div>
                    </div>
                    <div v-else>
                        <h3 class="text-left mt-4 mb-4">Альбоми</h3>
                        <div v-if="isAnyAlbum" class="row">
                            <div class="col-6 pb-4" v-for="item in user.albums" :key="item.id">
                                <albumCard v-bind:album="item">

                                </albumCard>
                            </div>
                        </div>
                        <div v-else class="row">
                            <div class="col-12">
                                <h2>На жаль, нічого не знайдено</h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import { required, maxLength } from "vuelidate/lib/validators";
    import profileEditModal from '@/components/ProfileEditModal'
    import tourCard from '@/components/TourCard'
    import albumCard from '@/components/AlbumCard'
    import constants from '@/constants.js'

    export default {
        name: 'Profile',
        components: {
            profileEditModal,
            tourCard,
            albumCard
        },
        data() {
            return {
                AlbumName: null,
                isModalVisible: false,
                errorMessage: null
            }
        },
        validations: {
            AlbumName: { required, maxLength: maxLength(100) }
        },
        computed: {
            ...mapGetters({
                user: 'user/getUser',
                currentUser: 'account/currentUser'
            }),
            profileImagePath() {
                if (this.user.profileImage == null)
                    return constants.DEFAULT_PROFILE_IMAGE_PATH;
                else
                    return constants.PROFILE_IMAGE_PATH + this.user.profileImage;
            },
            isCurrentUser() {
                if (this.$store.state.account.currentUser)
                    return this.$store.state.account.currentUser.id == this.$route.params.id;
                else
                    return false;
            },
            isBussinessUser() {
                return this.user.role == constants.BUSINESS_USER_ROLE;
            },
            isAnyAlbum() {
                return this.user.albums.length != 0;
            },
            isAnyTour() {
                return this.user.tours.length != 0;
            }
        },
        mounted() {
            this.$store.dispatch("user/GetById", this.$route.params.id).catch(error => {
                if (error.response && error.response.status === 400) {
                    this.$router.push("/");
                }
            });
        },
        watch: {
            '$route.path': {
                handler(oldUrl, newUrl) {
                    this.$store.dispatch("user/GetById", this.$route.params.id);
                }
            },
            Name: function (val) {
                this.errorMessage = null;
            }
        },
        methods: {
            closeModal() {
                this.isModalVisible = false;
            },
            showModal() {
                this.isModalVisible = true;
            },
            create: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введені дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("album/Create", formData, { root: true })
                        .then(() => this.successfulCreate())
                        .catch((error) => this.unsuccessfulCreate(error));
                }
            },
            successfulCreate: function () {
                this.$store.dispatch("user/GetById", this.$route.params.id);
                this.AlbumName = "";
                this.$bvModal.hide('createAlbumModal');
            },
            unsuccessfulCreate: function (error) {
                this.errorMessage = error.response.data;
            }
        }
    }
</script>

<style scoped>
    .header {
        padding: 50px 0px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .details {
        text-align: left;
    }

    .profile-pic {
        width: 50%;
        border-radius: 50%;
        border: 3px solid #2c3e50;
    }

    .card-section {
        padding-left: 30px;
        padding-right: 30px;
        margin-top: 50px;
        border-radius: 1rem;
        background-color: #f5f5f5;
    }
</style>

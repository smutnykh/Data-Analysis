<template>
    <div class="container mt-4 mb-4">
        <div class="row">
            <div class="col-12 text-left">
                <h3 class="mb-0">
                    {{album.name.toUpperCase()}}
                    <img v-if="isAuthor" v-b-modal.editAlbumModal class="crud-image mr-2 mb-1" src="@/assets/edit.png">
                    <img v-if="isAuthor" v-on:click="remove" class="crud-image mr-2 mb-1" src="@/assets/delete.png">
                </h3>
                <b-modal id="editAlbumModal" hide-footer title="Редагування альбому">
                    <form novalidate @submit.prevent="edit">
                        <div class="input-group mb-0 mt-2">
                            <input type="hidden" name="Id" :value="album.id" />
                            <input name="Name" type="text" class="form-control" placeholder="Назва альбому..." v-model="$v.Name.$model" v-on:keydown.enter.prevent>
                        </div>
                        <div v-if="$v.Name.$anyDirty">
                            <div class="error" v-if="!$v.Name.required">Назва обов'язкова</div>
                            <div class="error" v-if="!$v.Name.maxLength">Максимальна довжина поля 100 символів</div>
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
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="input-group mb-3 mt-3">
                    <input type="text" class="form-control" placeholder="Пошук за назвою туристичного місця..." v-model="searchTitle" />
                    <div class="input-group-append">
                        <button class="btn btn-outline-danger" type="button" @click="search">Пошук</button>
                    </div>
                </div>
                <div v-if="isNoRecords">
                    <h1 class="mt-5 display-3">На жаль, нічого не знайдено</h1>
                </div>
                <div v-else>
                    <div class="row mt-1">
                        <div class="col-4 text-right" v-for="item in album.sightseeings" :key="item.id">
                            <img v-if="isAuthor" class="crud-image mb-1 remove-sightseeing" v-on:click="removeSightseeing(item.id)" src="@/assets/delete.png">
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
    import { required, maxLength } from "vuelidate/lib/validators";
    import sightseeingCard from '@/components/SightseeingCard'
    import constants from '@/constants.js'

    export default {
        name: 'AlbumShow',
        components: {
            sightseeingCard
        },
        data() {
            return {
                page: 1,
                count: null,
                pageSize: 3,
                searchTitle: "",
                Name: null,
                errorMessage: null
            }
        },
        validations: {
            Name: { required, maxLength: maxLength(100) }
        },
        computed: {
            ...mapGetters({
                album: 'album/getAlbum',
            }),
            authorImagePath() {
                if (this.album.user.profileImage == null)
                    return constants.DEFAULT_PROFILE_IMAGE_PATH;
                else
                    return constants.PROFILE_IMAGE_PATH + this.album.user.profileImage;
            },
            isAuthor() {
                if (this.$store.state.account.currentUser)
                    return this.$store.state.account.currentUser.id == this.album.user.id;
                else
                    return false;
            },
            isNoRecords() {
                return this.count < 1;
            }
        },
        mounted() {
            this.loadData(1);

        },
        watch: {
            '$route.path': {
                handler(oldUrl, newUrl) {
                    this.loadData(1);
                }
            },
            Name: function (val) {
                this.errorMessage = null;
            }
        },
        methods: {
            remove: function () {
                this.$store.dispatch("album/Delete", this.album.id, { root: true })
                    .then(() => this.successfulDelete())
                    .catch((error) => this.unsuccessfulDelete(error));
            },
            successfulDelete: function () {
                this.$router.push({ name: 'Profile', params: { id: this.album.user.id } });
            },
            unsuccessfulDelete: function (error) {
                console.log(error.response.data);
            },
            edit: function () {
                if (this.$v.$invalid) {
                    this.errorMessage = "Неправильно введені дані";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("album/Edit", formData, { root: true })
                        .then(() => this.successfulEdit(formData.get('Name')))
                        .catch((error) => this.unsuccessfulEdit(error));
                }
            },
            successfulEdit: function (name) {
                this.errorMessage = null;
                this.loadData(this.page);
                this.$bvModal.hide('editAlbumModal');
            },
            unsuccessfulEdit: function (error) {
                this.errorMessage = error.response.data;
            },
            loadData(page) {
                this.$store.dispatch("album/GetById", {
                    id: this.$route.params.id, search: this.searchTitle, page,
                    size: this.pageSize
                })
                    .then((response) => this.loadDataOnResponse(response))
                    .catch(error => {
                        if (error.response && error.response.status === 400) {
                            this.$router.push("/");
                        }
                    });
            },
            loadDataOnResponse(response) {
                this.count = response;
                this.Name = this.album.name;
            },
            onPageChange(value) {
                this.loadData(value);
            },
            search() {
                this.loadData(1);
                this.page = 1;
            },
            removeSightseeing(sightseeingId) {
                this.$store.dispatch("album/RemoveSightseeing", { id: this.album.id, sightseeingId }).then(() => this.loadData(1));
            }
        }
    }
</script>

<style scoped>
    .remove-sightseeing {
        position: relative;
        margin: 0 8px -60px 0 !important;
        z-index: 1;
    }
   
    .pagination {
        justify-content: center
    }
</style>

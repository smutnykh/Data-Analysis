<template>
    <div class="modal" aria-labelledby="exampleModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Редагування профілю</h5>
                    <button type="button" class="close" v-on:click="closeModal">
                        <span>&times;</span>
                    </button>
                </div>
                <form novalidate @submit.prevent="update">
                    <div class="modal-body row">
                        <div class="col-3">
                            <img :src="this.$store.state.account.currentUser.profileImage == null ? DefaultSrc : Src" alt="Оберіть зображення" />
                            <div class="input-group mt-3">
                                <div class="custom-file">
                                    <input name="ProfileImage" type="file" class="custom-file-input" @change="onFileChange">
                                    <label class="custom-file-label">Оберіть зображення</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-9 text-left">
                            <div class="mb-3">
                                <label for="username" class="form-label">Ім'я користувача</label>
                                <input name="Username" type="text" v-model="$v.Username.$model" class="form-control" id="username" v-on:keydown.enter.prevent>
                                <div v-if="$v.Username.$anyDirty">
                                    <div class="error" v-if="!$v.Username.required">Ім'я користувача обов'язкове</div>
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="profileInfo" class="form-label">Детальна інформація</label>
                                <textarea name="ProfileInfo" type="text" v-model="$v.ProfileInfo.$model" class="form-control" id="profileInfo" v-on:keydown.enter.prevent></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="invalid-feedback mb-3">
                        {{errorMessage}}
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" v-on:click="closeModal">Закрити</button>
                        <button type="submit" class="btn btn-primary">Зберегти</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>


<script>
    import { required, maxLength } from "vuelidate/lib/validators";
    import constants from '@/constants.js'

    export default {
        name: 'ProfileEditModal',
        data() {
            return {
                Username: this.$store.state.account.currentUser.username,
                ProfileInfo: this.$store.state.account.currentUser.profileInfo,
                Src: constants.PROFILE_IMAGE_PATH + this.$store.state.account.currentUser.profileImage,
                DefaultSrc: constants.DEFAULT_PROFILE_IMAGE_PATH,
                ProfileImage: null,
                errorMessage: null
            }
        },
        validations: {
            Username: { required },
            ProfileInfo: { maxLength: maxLength(1000) }
        },
        watch: {
            Username: function (val) {
                this.errorMessage = null;
            },
            ProfileInfo: function (val) {
                this.errorMessage = null;
            }
        },
        methods: {
            closeModal() {
                this.$emit('closeModal');
                this.Username = this.$store.state.account.currentUser.username;
                this.ProfileInfo = this.$store.state.account.currentUser.profileInfo;
                this.Src = constants.PROFILE_IMAGE_PATH + this.$store.state.account.currentUser.profileImage;
                this.DefaultSrc = constants.DEFAULT_PROFILE_IMAGE_PATH
            },
            onFileChange(e) {
                this.ProfileImage = e.target.files[0];
                this.Src = URL.createObjectURL(this.ProfileImage);
                this.DefaultSrc = URL.createObjectURL(this.ProfileImage);
            },
            update: function (event) {
                if (this.$v.$invalid) {
                    this.errorMessage = "Invalid input";
                }
                else {
                    let formData = new FormData(event.target);
                    this.$store.dispatch("user/Update", formData, { root: true })
                        .then(() => this.successfulUpdate())
                        .catch((error) => this.unsuccessfulUpdate(error));
                }
            },
            successfulUpdate: function () {
                this.$store.dispatch("user/GetById", this.$route.params.id);
                this.$emit('closeModal');
            },
            unsuccessfulUpdate: function (error) {
                this.errorMessage = error.response.data;
            }
        },
    }
</script>

<style scoped>
    .modal {
        display: block;
        top: 100px;
    }

    .modal-dialog {
        min-width: 850px;
        max-width: 850px;
    }

    label {
        font-size: 14px;
    }

    img {
        object-fit: cover;
        width: 181px;
        height: 181px;
        border-radius: 50%;
    }

    .error {
        padding: 0 6px;
        color: red;
        text-align: left;
        font-size: 0.75rem;
    }

    .invalid-feedback {
        color: red;
        display: block;
        font-size: 0.75rem;
    }
</style>
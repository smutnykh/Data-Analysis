<template>
    <div class="card bg-dark text-white text-left">
        <img :src="albumImagePath" class="card-img"  alt="Card image">
        <div class="card-img-overlay">
            <h5 class="card-title"><span>{{album.name}}</span></h5>
            <button class="btn btn-danger mt-2" v-on:click="$router.push({ name: 'AlbumShow', params : { id : album.id}})">Переглянути</button>
        </div>
    </div>
</template>


<script>
    import constants from '@/constants.js'

    export default {
        name: 'AlbumCard',
        props: {
            album: Object
        },
        computed: {
            albumImagePath() {
                if (this.album.image != null)
                    return constants.SIGHTSEEING_IMAGE_PATH + this.album.image;
                else
                    return constants.DEFAULT_ALBUM_IMAGE_PATH;
            }
        }
    }
</script>

<style scoped>
    .card-img-overlay{
        top: auto;
        bottom: 0 !important;
        padding: 15px;
    }

    span {
        border-radius: 0.25rem;
        background-color: #0a213f;
        padding: 10px;
    }

    .card {
        border: none;
    }
</style>
<template>
    <div></div>
</template>

<script>
    export default {
        name: "EmailConformation",
        created: function () {
            var token = this.$route.query.token;
            var username = this.$route.query.username;
            this.$store.dispatch("account/ConfirmRegistration", { token, username }, { root: true }).
                then(() => this.emailConfirmed());
        },
        methods: {
            emailConfirmed: function () {
                console.log("true");
                this.$router.push('/login');
            }
        }
    }
</script>

<style scoped>
    
</style>
<template>
    <v-card>
        <v-img :src="comic.image.mediumUrl"></v-img>
        <h6 class="text-center">{{comic.name}}</h6>
        <p class="text-center">Issue Number: {{comic.issueNumber}}</p>
    </v-card>
</template>

<script>

export default {
    
    props: ["comic"],
    data() {
        return {

        }
    }
}

</script>

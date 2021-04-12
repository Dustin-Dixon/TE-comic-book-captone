<template>
    <v-card>
        <v-skeleton-loader type="image" boilerplate></v-skeleton-loader>
        <div>{{comic.name}}</div>
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

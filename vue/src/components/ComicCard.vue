<template>
  <v-card>
    <v-img :src="comic.image.mediumUrl" :height="height"></v-img>
    <h6 class="text-center">{{ comic.name }}</h6>
    <p class="text-center">Issue Number: {{ comic.issueNumber }}</p>
  </v-card>
</template>

<script>
export default {
  props: ["comic", "height"],
};
</script>

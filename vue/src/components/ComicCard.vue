<template>
  <v-card>
    <v-img :src="comic.image.mediumUrl" :height="`${imageHeight}px`"></v-img>
    <h6 class="text-center">{{ comic.name }}</h6>
    <p class="text-center">Issue Number: {{ comic.issueNumber }}</p>
  </v-card>
</template>

<script>
export default {
  props: ["comic"],
  computed: {
    imageHeight() {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return 150;
        case "sm":
          return 200;
        case "md":
          return 300;
        case "lg":
          return 350;
        default:
          return 400;
      }
    },
  },
};
</script>

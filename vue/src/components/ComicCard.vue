<template>
  <v-card>
    <v-img :src="comic.image.mediumUrl" :height="`${imageHeight}px`"></v-img>
    <!-- <v-card-text>
    <div class="text-center">{{ comic.name }} #{{comic.issueNumber}}</div>
    </v-card-text> -->
    <v-card-actions>
      <comic-detail :comic="comic" />
    </v-card-actions>
  </v-card>
</template>

<script>

import ComicDetail from "./ComicDetail.vue"

export default {
  props: ["comic"],
  components: {ComicDetail},
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

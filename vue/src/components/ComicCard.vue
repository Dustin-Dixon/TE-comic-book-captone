<template>
<v-container>
  <v-card>
    <v-img :src="comic.image.mediumUrl" :height="height"></v-img>
    <!-- <v-card-text>
    <div class="text-center">{{ comic.name }} #{{comic.issueNumber}}</div>
    </v-card-text> -->

      <comic-detail :comic="comic" />

  </v-card>
</v-container>
</template>

<script>

import ComicDetail from "./ComicDetail.vue"

export default {
  props: ["comic", "height"],
  components: {ComicDetail},
};
</script>

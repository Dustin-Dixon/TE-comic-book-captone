<template>
  <v-card @click="$emit('click')">
    <v-img :src="comic.image.mediumUrl" :height="height"></v-img>
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
  props: ["comic", "height"],
  components: {ComicDetail},
};
</script>

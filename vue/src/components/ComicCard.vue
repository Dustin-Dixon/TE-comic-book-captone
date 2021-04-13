<template>
  <v-card @click="$emit('click')">
    <v-img :src="comic.image.mediumUrl" :height="height"></v-img>
    <!-- <v-card-text>
    <div class="text-center">{{ comic.name }} #{{comic.issueNumber}}</div>
    </v-card-text> -->

    <comic-detail :comic="comic" v-if="showInfo" />
  </v-card>
</template>

<script>
import ComicDetail from "./ComicDetail.vue";

export default {
  props: ["comic", "height", "showInfo"],
  components: { ComicDetail },
};
</script>

<template>
  <v-card @click="$emit('click')">
    <v-img :src="comic.image.mediumUrl" :height="height"></v-img>
    <!-- <v-card-text>
    <div class="text-center">{{ comic.name }} #{{comic.issueNumber}}</div>
    </v-card-text> -->

    <comic-detail @add-tag="$emit('add-tag', $event)" @delete="$emit('delete', $event)" :comic="comic" v-if="showInfo" :showRemove="showRemove" />
  </v-card>
</template>

<script>
import ComicDetail from "./ComicDetail.vue";

export default {
  props: ["comic", "height", "showInfo", "showRemove"],
  components: { ComicDetail },
};
</script>

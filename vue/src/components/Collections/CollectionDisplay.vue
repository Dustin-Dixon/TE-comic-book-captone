<template>
  <div>
    <v-row v-if="addComic"
      ><v-col align="center"
        ><v-btn @click="addComic">Add Comic</v-btn></v-col
      ></v-row
    >
    <v-row>
      <v-col cols="3" v-for="comic in comics" :key="comic.id">
        <comic-card :comic="comic" />
      </v-col>
    </v-row>
  </div>
</template>

<script>
import ComicCard from "../ComicCard.vue";

export default {
  components: {
    ComicCard,
  },
  props: {
    comics: {
      type: Array,
      default: () => [],
    },
    addComic: {
      type: Function,
    },
  },
};
</script>
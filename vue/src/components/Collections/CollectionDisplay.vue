<template>
  <v-container>
    <v-row v-if="addComic"
      ><v-col align="center"
        ><v-btn @click="addComic">Add Comic</v-btn></v-col
      ></v-row
    >
    <v-row>
      <v-col cols="3" v-for="comic in comics" :key="comic.id">
        <comic-card :comic="comic" :height="cardHeight" />
      </v-col>
    </v-row>
  </v-container>
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
  computed: {
    cardHeight() {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return "150px";
        case "sm":
          return "200px";
        case "md":
          return "300px";
        case "lg":
          return "350px";
        default:
          return "400px";
      }
    },
  },
};
</script>
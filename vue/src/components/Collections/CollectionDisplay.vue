<template>
  <v-container>
    <v-row>
      <v-col cols="4" v-for="comic in comics" :key="comic.id">
        <comic-card
          @delete="$emit('delete', $event)"
          @add-tag="comic.tags.push($event)"
          :comic="comic"
          :height="cardHeight"
          :showInfo="showInfo"
          :showRemove="showRemove"
        />
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
    showInfo: {
      type: Boolean,
      default: true,
    },
    showRemove: {
      type: Boolean,
      default: true,
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
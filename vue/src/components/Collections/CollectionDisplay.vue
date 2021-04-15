<template>
  <v-container>
    <v-row v-if="addComic" align="center">
      <v-col cols="9">
        <v-divider />
      </v-col>
      <v-col cols="2">
        <v-btn @click="addComic">Add Comic</v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="9">
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
      </v-col>
      <v-col cols="3">
        <collection-statistics :collection="selectedCollection" :key="selectedCollection.collectionID"/>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import ComicCard from "../ComicCard.vue";
import CollectionStatistics from "../Collections/CollectionStatistics.vue";
export default {
  components: {
    ComicCard,
    CollectionStatistics,
  },
  props: {
    comics: {
      type: Array,
      default: () => [],
    },
    addComic: {
      type: Function,
    },
    showInfo: {
      type: Boolean,
      default: true,
    },
    showRemove: {
      type: Boolean,
      default: true,
    },
    selectedCollection: {
      type: Object,
      default: () => {},
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
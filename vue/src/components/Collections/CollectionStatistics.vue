<template>
  <v-container>
    <h3 class="text-center">Collection Stats</h3>
    <template v-if="hasCollection">
      <h6>Total Comics In Collection: {{ stats.comicCount }}</h6>
    </template>
  </v-container>
</template>

<script>
import CollectionService from "../../services/CollectionService.js";

export default {
  props: ["collection"],
  data() {
    return {
      stats: [],
    };
  },
  computed: {
    hasCollection() {
      return this.collection.collectionID !== undefined;
    },
  },
  watch: {
    collection: function (val) {
      if (val.collectionID !== undefined) {
        CollectionService.getCollectionStats(val.collectionID).then(
          (response) => {
            if (response.status === 200) {
              this.stats = response.data;
            }
          }
        );
      }
    },
  },
};
</script>
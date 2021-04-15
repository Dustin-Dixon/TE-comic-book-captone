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
import { mapState } from "vuex";

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
    ...mapState(["user"]),
    comicCount() {
      return this.user.comicCount;
    },
  },
  methods: {
    loadStats() {
      if (this.collection.collectionID !== undefined) {
        CollectionService.getCollectionStats(this.collection.collectionID).then(
          (response) => {
            if (response.status === 200) {
              this.stats = response.data;
            }
          }
        );
      }
    },
  },
  watch: {
    collection: function () {
      this.loadStats();
    },
    comicCount: function() {
      this.loadStats();
    }
  },
};
</script>
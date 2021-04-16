<template>
  <v-container>
    <h3 class="text-center text-h6">Collection Stats</h3>
    <template v-if="hasCollection">
      <h6 class="text-nav">
        Total Comics In Collection: {{ stats.comicCount }}
      </h6>
      <v-divider class="my-1"></v-divider>
      <h3 class="text-center text-h6">Top 5 Characters</h3>
      <div
        v-for="characterCount in topFiveCharacters"
        :key="characterCount.character.id"
      >
        <h6 class="text-nav">
          {{ characterCount.character.name }}: {{ characterCount.count }}
        </h6>
      </div>
      <v-divider class="my-1"></v-divider>
      <h3 class="text-center text-h6">Top 5 Creators</h3>
      <div
        v-for="creatorCount in topFiveCreators"
        :key="creatorCount.creator.id"
      >
        <h6 class="text-nav">
          {{ creatorCount.creator.name }}: {{ creatorCount.count }}
        </h6>
      </div>
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
      return (this.collection !== undefined && this.collection.collectionID !== undefined);
    },
    ...mapState(["user"]),
    comicCount() {
      return this.user.comicCount;
    },
    topFiveCharacters() {
      return this.stats.characters.slice(0, 5);
    },
    topFiveCreators() {
      return this.stats.creators.slice(0, 5);
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
      if (this.collection !== undefined) {
        this.loadStats();
      }
    },
    comicCount: function () {
      this.loadStats();
    },
  },
};
</script>
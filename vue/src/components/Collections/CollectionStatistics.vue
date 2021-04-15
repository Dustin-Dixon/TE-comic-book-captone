<template>
  <v-container> 
      <h3 class="text-center">Collection Stats</h3>
      <h6>Total Comics In Collection: {{stats.comicCount}}</h6>
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
  created() {
    CollectionService.getPublicCollectionStats(this.collection.id).then(
      (response) => {
        if (response.status === 200) {
          this.stats = response.data;
        }
      }
    );
  },
};
</script>
<template>
  <v-container>
    <v-row>
      <v-col>
        <h3 class="text-h3 text-center">{{ collection.name }}</h3>
        <v-divider class="my-2" />
        <h5 class="text-h4 text-center mb-2">By: {{ collection.username }}</h5>
      </v-col>
    </v-row>
    <collection-display :comics="comics" />
  </v-container>
</template>

<script>
import CollectionDisplay from "../components/Collections/CollectionDisplay.vue";
import CollectionService from "../services/CollectionService.js";
export default {
  components: { CollectionDisplay, CollectionService },
  data() {
    return {
      comics: [],
      collection: [],
    };
  },
  computed: {
    collectionID() {
      return this.$route.params.id;
    },
  },
  created() {
    CollectionService.getPubliCollectionFromID(this.collectionID).then(
      (response) => {
        if (response.status === 200) {
          this.collection = response.data;
        }
      }
    );
    CollectionService.getComicsInPublicCollection(this.collectionID).then(
      (response) => {
        if (response.status === 200) {
          this.comics = response.data;
        }
      }
    );
  },
};
</script>
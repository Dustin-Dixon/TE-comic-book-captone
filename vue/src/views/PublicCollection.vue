<template>
  <v-container>
    <v-row>
      <v-col>
        <h3 class="text-h3 text-center">{{ collection.name }}</h3>
        <v-divider class="my-2" />
        <h5 class="text-h4 text-center mb-2">By: {{ collection.username }}</h5>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="9">
        <collection-display
          :selectedCollection="collection"
          :comics="comics"
          :showRemove="false"
        />
      </v-col>
      <v-col>
        <collection-statistics :collection="collection" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import CollectionDisplay from "../components/Collections/CollectionDisplay.vue";
import CollectionStatistics from "../components/Collections/CollectionStatistics.vue";
import CollectionService from "../services/CollectionService.js";

export default {
  components: { CollectionDisplay, CollectionStatistics},
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
    CollectionService.getPublicCollectionFromID(this.collectionID).then(
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
<template>
  <div>
    <v-carousel v-if="!loading">
      <v-carousel-item
        v-for="collection in collections"
        :key="collection.collectionID"
      >
        <collection-preview :collection="collection" />
      </v-carousel-item>
    </v-carousel>
    <v-row v-else>
      <v-col align="center">
        <v-skeleton-loader class="carousel" type="card-heading, image" />
      </v-col>
    </v-row>
  </div>
</template>

<script>
import CollectionPreview from "./CollectionPreview";
import CollectionService from "@/services/CollectionService";

export default {
  components: {
    CollectionPreview,
  },
  data() {
    return {
      collections: [],
      loading: true,
    };
  },
  created() {
    CollectionService.getPublicCollections().then((response) => {
      if (response.status === 200) {
        this.collections = response.data;
        this.loading = false;
      }
    });
  },
};
</script>

<style scoped>
.v-skeleton-loader__image.carousel {
  height: 500px;
}
</style>
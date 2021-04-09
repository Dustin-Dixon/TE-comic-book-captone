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
        <v-skeleton-loader class="carousel" type="card-heading, image"/>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import CollectionPreview from "./CollectionPreview";

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
  created() {},
};
</script>

<style scoped>
.v-skeleton-loader__image.carousel {
  height: 500px;
}
</style>
<template>
  <v-container>
    <v-img :src="require('@/assets/power_comics_logo.png')" max-height="400px" contain></v-img>
    <v-divider class="my-4" />
    <v-row>
      <v-col>
        <h3 class="text-h3 text-center">PUBLIC COLLECTIONS</h3>
      </v-col>
    </v-row>
    <v-divider class="my-4" />
    <collection-carousel class="mt-1" />
  </v-container>
</template>

<script>
import CollectionCarousel from "../components/Collections/CollectionCarousel.vue";

export default {
  components: {
    CollectionCarousel,
  },
};
</script>
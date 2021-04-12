<template>
  <v-container>
    <v-img :src="require('@/assets/power_comics_logo.png')" max-height="400px" contain></v-img>
    <v-divider />
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
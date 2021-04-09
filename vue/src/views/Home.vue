<template>
  <v-container>
    <h1 class="text-h1 text-center">
      Welcome to <span class="red--text">POW</span>er Comics
    </h1>
    <v-divider />
    <collection-carousel class="mt-1"/>
  </v-container>
</template>

<script>
import CollectionCarousel from '../components/Collections/CollectionCarousel.vue';

export default {
  components: {
    CollectionCarousel
  },
};
</script>
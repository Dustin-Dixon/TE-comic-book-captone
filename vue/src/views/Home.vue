<template>
  <v-container>
    <v-img :src="require('@/assets/power_comics_logo.png')" max-height="400px" contain></v-img>
    <v-divider class="my-4" />
      <v-col>
        <h4 class="text-h4 text-center">Welcome to Power Comics</h4>
        <p class="text-h6 text-center" id="welcome">Where you can browse users' public collections, or build your own collections.<br>
        Sign up to build your own collections and show them to others by setting them to public!</p>
      </v-col>
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
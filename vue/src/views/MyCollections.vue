<template>
  <v-container>
    <v-row>
      <v-col align="center">
        <h1>My Collections</h1>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="3">
        <collection-list />
      </v-col>
      <v-col>
        <collection-display />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import CollectionList from "../components/CollectionList.vue";
import CollectionDisplay from "../components/CollectionDisplay.vue";

export default {
  name: "MyCollections",
  components: { CollectionList, CollectionDisplay },
  data() {
    return {
      error: "",
    };
  },

};
</script>
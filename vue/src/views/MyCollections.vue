<template>
  <v-container>
    <v-row>
      <v-col align="center">
        <h1>My Collections</h1>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="3">
        <collection-list
          :collections="collections"
          :saveCollection="saveCollection"
          :changeSelected="selectCollection"
        />
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

import CollectionService from "../services/CollectionService";

export default {
  name: "MyCollections",
  components: { CollectionList, CollectionDisplay },
  data() {
    return {
      error: "",
      collections: [],
    };
  },
  methods: {
    saveCollection(newCollection) {
      CollectionService.addCollection(newCollection).then((response) => {
        if (response.status === 201) {
          this.collections.push(response.data);
        }
      });
    },
    selectCollection(selectedCollection) {
      console.log(selectedCollection);
    }
  },
  created() {
    return CollectionService.getUserCollections()
      .then((response) => {
        if (response.status === 200) {
          this.collections = response.data;
        }
      })
      .catch((error) => {
        console.log(error);
        this.error = error;
      });
  },
};
</script>
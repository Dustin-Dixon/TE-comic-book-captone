<template>
  <v-container fluid>
    <v-row>
      <v-col align="center">
        <h1 class="text-h1">My Collections</h1>
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
        <collection-display :addComic="addComicToCollection" :comics="comics" />
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
      comics: [],
      selectedCollection: {},
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
      this.selectedCollection = selectedCollection;
      CollectionService.getComicsInCollection(selectedCollection).then(
        (response) => {
          if (response.status === 200) {
            this.comics = response.data;
          }
        }
      );
    },
    addComicToCollection(newComic) {
      CollectionService.addComicToCollection(
        this.selectedCollection,
        newComic
      ).then((response) => {
        if (response.status === 201) {
          this.comics.push(response.data);
        }
      });
    },
  },
  created() {
    return CollectionService.getUserCollections()
      .then((response) => {
        if (response.status === 200) {
          this.collections = response.data;
          this.selectCollection(this.collections[0]);
        }
      })
      .catch((error) => {
        console.log(error);
        this.error = error;
      });
  },
};
</script>
<template>
  <v-sheet height="100%" tile>
    <v-container>
      <v-row>
        <v-col>
          <h3 class="text-h3 text-center">{{ collection.name }}</h3>
          <h4 class="text-h4 text-center">By: {{collection.username}}</h4>
        </v-col>
      </v-row>
      <v-divider class="my-5"/>
      <v-row>
        <collection-display :comics="firstFourComics" />
      </v-row>
    </v-container>
  </v-sheet>
</template>

<script>
import CollectionDisplay from "./CollectionDisplay.vue"
import CollectionService from "../../services/CollectionService.js"
export default {
  components: { CollectionDisplay, CollectionService},
  props: ["collection"],
  data() {
    return {
      comics: [],
    }
  },
  computed: {
    firstFourComics() {
      return this.comics.slice(0, 4);
    }
  },
  created() {
    CollectionService.getComicsInPublicCollection(this.collection.collectionID).then(
      (response) => {
        if (response.status === 200){
          this.comics = response.data;
        }
      }
    )
  }
};
</script>
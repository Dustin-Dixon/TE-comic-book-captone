<template>
    <v-container>
        <collection-display :comics="comics" />
    </v-container>
</template>

<script>

import CollectionDisplay from "../components/Collections/CollectionDisplay.vue"
import CollectionService from "../services/CollectionService.js"
export default {
    components: {CollectionDisplay, CollectionService},
    data () {
        return {
            comics: [],
        }
    },
    computed: {
        collectionID() {
            return this.$route.params.id;
        }
    },
    created () {
        CollectionService.getComicsInPublicCollection(this.collectionID).then(
        (response) => {
          if (response.status === 200) {
            this.comics = response.data;
          }
        }
      );
    }
}
</script>
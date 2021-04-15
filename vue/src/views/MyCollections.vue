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
        <collection-display
          v-if="collections.length > 0"
          @delete="deleteComic"
          :addComic="() => (addDialog = !addDialog)"
          :comics="comics"
          :selectedCollection="selectedCollection"
        />
      </v-col>
    </v-row>
    <add-comic
      :visible="addDialog"
      @close="addDialog = false"
      :saveComic="addComicToCollection"
    />
    <ConfirmDlg ref="confirm" />
  </v-container>
</template>

<script>
import CollectionList from "../components/Collections/CollectionList.vue";
import CollectionDisplay from "../components/Collections/CollectionDisplay.vue";
import AddComic from "../components/Dialogs/AddComic.vue";
import CollectionService from "../services/CollectionService";
import ConfirmDlg from "../components/Dialogs/Confirm.vue";

export default {
  name: "MyCollections",
  components: { CollectionList, CollectionDisplay, AddComic, ConfirmDlg },
  data() {
    return {
      error: "",
      collections: [],
      comics: [],
      selectedCollection: {},
      addDialog: false,
    };
  },
  methods: {
    saveCollection(newCollection) {
      CollectionService.addCollection(newCollection).then((response) => {
        if (response.status === 201) {
          this.collections.push(response.data);
          if (this.collections.length === 1) {
            this.selectedCollection = response.data;
          }
        }
      });
    },
    selectCollection(selectedCollection) {
      this.selectedCollection = selectedCollection;
      CollectionService.getComicsInCollection(
        selectedCollection.collectionID
      ).then((response) => {
        if (response.status === 200) {
          this.comics = response.data;
        }
      });
    },
    addComicToCollection(newComic) {
      CollectionService.addComicToCollection(
        this.selectedCollection,
        newComic
      ).then((response) => {
        if (response.status === 201) {
          this.comics.push(response.data);
          this.selectedCollection.comicCount += 1;
          this.$store.dispatch("ADD_COMIC");
        }
      });
    },
    async deleteComic(comic) {
      if (
          await this.$refs.confirm.open(
            "Confirm",
            "Are you sure you want to remove this comic?"
          )
        ) {
          CollectionService.deleteComicFromCollection(
          this.selectedCollection.collectionID,
          comic.id
        ).then((response) => {
          if (response.status === 200) {
            this.selectCollection(this.selectedCollection);
            this.$store.dispatch("REMOVE_COMIC");
          }
        });
        }
        
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
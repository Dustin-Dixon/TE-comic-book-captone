<template>
  <div>
    <v-list nav dense>
      <v-list-item-group
        v-model="selectedItem"
        color="primary"
        @change="changeSelected"
      >
        <v-list-item v-for="collection in collections" :key="collection.id">
          <v-list-item-content>
            <v-list-item-title v-text="collection.name" />
          </v-list-item-content>
        </v-list-item>
      </v-list-item-group>
      <v-btn fab color="primary" bottom left absolute @click="dialog = !dialog">
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-list>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-text>
          <v-text-field label="Collection Name" v-model="newCollection.name" />
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="saveCollection"> Submit </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import CollectionService from "../services/CollectionService";

export default {
  data() {
    return {
      dialog: false,
      selectedItem: 0,
      newCollection: { name: "" },
      collections: [],
    };
  },
  methods: {
    saveCollection() {
      CollectionService.addCollection(this.newCollection).then((response) => {
        if (response.status === 201) {
          this.refreshCollections();
        }
      });
      this.dialog = false;
      this.newCollection = { name: "" };
    },
    changeSelected() {
      this.$store.commit(
        "SELECT_COLLECTION",
        this.collections[this.selectedItem]
      );
    },
    refreshCollections() {
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
  },
  name: "CollectionList",
  created() {
    this.refreshCollections().then(() => {
      this.changeSelected();
    });
  },
};
</script>
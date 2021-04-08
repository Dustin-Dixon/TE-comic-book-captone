<template>
  <div>
    <v-list nav dense>
      <v-list-item-group v-model="selectedCollection" color="primary">
        <v-list-item
          v-for="collection in collections"
          :key="collection.id"
          @change="changeSelected(collection)"
        >
          <v-list-item-content>
            <v-list-item-title v-text="collection.name" />
          </v-list-item-content>
          <v-list-item-action>
            <settings-menu :collection="collection"/>
          </v-list-item-action>
        </v-list-item>
      </v-list-item-group>
      <v-btn
        fab
        color="primary"
        bottom
        left
        absolute
        @click="showAddDialog = !showAddDialog"
      >
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-list>
    <v-dialog v-model="showAddDialog" max-width="500px">
      <v-card>
        <v-card-text>
          <v-text-field label="Collection Name" v-model="newCollection.name" />
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="onSaveCollection"> Submit </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

import SettingsMenu from "./Collections/SettingsMenu.vue"

export default {
  name: "CollectionList",
  components: {SettingsMenu},
  data() {
    return {
      showAddDialog: false,
      newCollection: { name: "" },
      selectedCollection: 0,
    };
  },

  props: {
    collections: {
      type: Array,
      default: () => [],
    },
    saveCollection: {
      type: Function,
      required: true,
    },
    changeSelected: {
      type: Function,
      required: true,
    },
  },

  methods: {
    onSaveCollection() {
      this.saveCollection(this.newCollection);
      this.showAddDialog = false;
      this.newCollection = { name: "" };
    }
  },
};
</script>
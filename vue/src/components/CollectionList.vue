<template>
    <v-list
        nav
        dense
      >
        <v-list-item-group
          v-model="selectedItem"
          color="primary"
        >
          <v-list-item
            v-for="collection in collections"
            :key="collection.id"
          >
            <v-list-item-content>
              <v-list-item-title v-text="collection.name"/>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
</template>

<script>
export default {
  data() {
    return {
      selectedItem: ''
    }
  },
  name: "CollectionList",
  props: ["collections"],
};
</script>
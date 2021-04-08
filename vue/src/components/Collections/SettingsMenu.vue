<template>
  <v-menu :close-on-content-click="false" :nudge-width="200" offset-x>
    <template v-slot:activator="{ on, attrs }">
      <v-btn
        icon
        v-on="on"
        v-bind="attrs"
        @click="newCollection = { ...collection }"
      >
        <v-icon>mdi-cog</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-card-text>
        <v-text-field label="Name" v-model="newCollection.name" />
        <v-checkbox label="Public" v-model="newCollection.public"></v-checkbox>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text color="primary" @click="updateCollection"> Submit </v-btn>
      </v-card-actions>
    </v-card>
  </v-menu>
</template>

<script>

import CollectionService from "@/services/CollectionService.js"

export default {
    props: ["collection"],
    data () {
        return {
            newCollection: {name: '', collectionID: 0, public: false}
        }
    },
    methods: {
        updateCollection() {
            CollectionService.updateCollectionSettings(this.newCollection).then((response) => {
                if (response.status === 200){
                    
                }
            })
        }
    }
}
</script>

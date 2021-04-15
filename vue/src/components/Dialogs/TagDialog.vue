<template>
  <v-menu
    v-model="showMenu"
    :close-on-content-click="false"
    top
    :offset-y="offset"
  >
    <template v-slot:activator="{ on, attrs }">
      <v-btn text v-on="on" v-bind="attrs"> Add A Tag </v-btn>
    </template>
    <v-card>
      <v-card-text>
        <v-text-field label="Tag Name" v-model="newTag.description" autofocus />
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text color="primary" @click="addTagToComic"> Submit </v-btn>
      </v-card-actions>
    </v-card>
  </v-menu>
</template>

<script>
import ComicService from "../../services/ComicService.js";

export default {
  props: ["comic"],
  data() {
    return {
      newTag: {
        description: "",
      },
      showMenu: false,
      offset: true,
    };
  },
  methods: {
    addTagToComic() {
      ComicService.addTagToComic(this.comic.id, this.newTag).then(
        (response) => {
          console.log(response);
          if (response.status === 201) {
              this.$emit("add-tag", response.data);
          }
        }
      );
      this.newTag = { description: "" };
      this.showMenu = false;
    },
  },
};
</script>
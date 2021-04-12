<template>
  <div>
    <v-row v-if="addComic"
      ><v-col align="center"
        ><v-btn @click="dialog = !dialog">Add Comic</v-btn></v-col
      ></v-row
    >

    <v-row>
      <v-col cols="3" v-for="comic in comics" :key="comic.comicID">
        <comic-card :comic="comic" />
      </v-col>
    </v-row>

    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-text>
          <v-text-field label="Search for a Comic" v-model="searchTerms" />
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="saveComic"> Submit </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import ComicCard from "../ComicCard.vue";

export default {
  components: {
    ComicCard,
  },
  data() {
    return {
      dialog: false,
      menu: false,
      searchTerms: "",
    };
  },
  methods: {
    saveComic() {
      this.addComic(this.newComic);
      this.newComic = { name: "", author: "", releaseDate: "" };
      this.dialog = false;
    },
  },
  props: {
    comics: {
      type: Array,
      default: () => [],
    },
    addComic: {
      type: Function,
    },
  },
};
</script>
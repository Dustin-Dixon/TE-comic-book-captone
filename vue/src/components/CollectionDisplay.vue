<template>
  <div>
    <v-row v-if="addComic"
      ><v-col align="center"
        ><v-btn @click="dialog = !dialog">Add Comic</v-btn></v-col
      ></v-row
    >

    <div v-for="comic in comics" :key="comic.comicID">{{ comic.name }}</div>

    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-text>
          <v-text-field label="Comic Name" v-model="newComic.name" />
          <v-text-field label="Author" v-model="newComic.author" />
          <v-menu
            ref="menu"
            v-model="menu"
            :close-on-content-click="false"
            :return-value.sync="newComic.releaseDate"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                v-model="newComic.releaseDate"
                label="Release Date"
                prepend-icon="mdi-calendar"
                readonly
                v-bind="attrs"
                v-on="on"
              ></v-text-field>
            </template>
            <v-date-picker v-model="newComic.releaseDate" no-title scrollable>
              <v-spacer></v-spacer>
              <v-btn text color="primary" @click="menu = false"> Cancel </v-btn>
              <v-btn
                text
                color="primary"
                @click="$refs.menu.save(newComic.releaseDate)"
              >
                OK
              </v-btn>
            </v-date-picker>
          </v-menu>
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
import CollectionService from "../services/CollectionService";

export default {
  data() {
    return {
      dialog: false,
      menu: false,
      newComic: { name: "", author: "", releaseDate: "" },
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
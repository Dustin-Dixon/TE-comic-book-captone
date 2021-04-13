<template>
  <v-dialog v-model="show" max-width="500px">
    <v-card>
      <v-card-text>
        <v-text-field
          class="pt-5"
          label="Search for a Comic"
          v-model="searchTerms"
          @input="onChangeSearch"
        />
        <v-divider />
      </v-card-text>
      <v-card-actions>
        <v-row>
          <v-col cols="2" v-for="comic in searchResults" :key="comic.comicID">
            <comic-card :comic="comic" />
          </v-col>
        </v-row>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
const debounce = require("lodash.debounce");

export default {
  data() {
    return {
      searchTerms: "",
      searchResults: [],
    };
  },
  created() {
    this.debouncedSearch = debounce(this.doLocalSearch, 500);
  },
  methods: {
    onChangeSearch() {
      console.log("Key Pressed");
      this.debouncedSearch();
    },
    doLocalSearch() {
      console.log("Search");
    },
  },
  computed: {
    show: {
      get() {
        return this.visible;
      },
      set(value) {
        if (!value) {
          this.$emit("close");
        }
      },
    },
  },
  props: ["visible"],
};
</script>
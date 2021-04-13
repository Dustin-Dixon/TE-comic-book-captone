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
        <v-container>
          <v-row>
            <v-col cols="3" v-for="comic in searchResults" :key="comic.comicID">
              <comic-card :comic="comic" height="100px" />
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import ComicCard from "../ComicCard";

import debounce from "lodash.debounce";
import ComicService from "@/services/ComicService";

export default {
  data() {
    return {
      searchTerms: "",
      searchResults: [],
    };
  },
  created() {
    this.debouncedSearch = debounce(this.doLocalSearch, 500);
    this.doLocalSearch();
  },
  methods: {
    onChangeSearch() {
      this.debouncedSearch();
    },
    doLocalSearch() {
      ComicService.searchLocalComics(this.searchTerms).then((response) => {
        if (response.status === 200) {
          this.searchResults = response.data;
        }
      });
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
          this.searchTerms = "";
          this.doLocalSearch();
        }
      },
    },
  },
  props: ["visible"],
  components: {
    ComicCard,
  },
};
</script>
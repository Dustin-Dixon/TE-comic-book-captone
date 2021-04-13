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
              <comic-card
                :comic="comic"
                height="100px"
                @click="saveClickedComic(comic)"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col align="center">
              Can't find your comic?
              <v-btn
                class="ml-2"
                @click="searchOnline"
                :disabled="disableOnlineSearch"
                >Search Online</v-btn
              >
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
      disableOnlineSearch: false,
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
    saveClickedComic(comic) {
      this.saveComic(comic);
      this.show = false;
    },
    searchOnline() {
      this.disableOnlineSearch = true;
      ComicService.searchOnline(this.searchTerms)
        .then((response) => {
          if (response.status === 200) {
            this.searchResults = response.data.results;
          }
        })
        .finally(() => {
          this.disableOnlineSearch = true;
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
  props: ["visible", "saveComic"],
  components: {
    ComicCard,
  },
};
</script>
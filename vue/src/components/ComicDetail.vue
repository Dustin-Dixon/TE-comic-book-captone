<template>
  <v-menu v-model="showDetail" :nudge-width="200" offset-x elevation="2">
    <template v-slot:activator="{ on, attrs }">
      <v-btn icon v-on="on" v-bind="attrs" @click="comicDetails = { ...comic }">
        <v-icon>mdi-information-outline</v-icon>
      </v-btn>
    </template>
    <v-card max-width="400px">
      <v-img
        :src="comic.image.mediumUrl"
        :height="`${imageHeight}px`"
        contain
      ></v-img>
      <v-card-title align="center">{{ comic.name }}</v-card-title>
      <v-card-text>
        <div>Issue Number: {{ comic.issueNumber }}</div>
        <div>Volume: {{ comic.volume.name }}</div>
        <div v-if="comic.volume.publisher !== null">Publisher: {{ comic.volume.publisher.name }} </div>
        <div>Cover Date: {{ comic.coverDate }}</div>
        <div v-if="Array.isArray(comic.creators) && comic.creators.length > 0">Creators: {{ creatorList }}</div>
        <div v-if="Array.isArray(comic.characters) && comic.characters.length > 0">Characters: {{ characterList }}</div>
        <v-row class="mt-1" v-if="Array.isArray(comic.tags) && comic.tags.length > 0">
          <div v-for="tag in comic.tags" :key="tag.id">
            <v-chip color="primary" class="ma-2">{{ tag.description }}</v-chip>
          </div>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-btn text :href="comic.siteDetailUrl" target="_blank">Get More Info</v-btn>
        <v-btn text v-if="showRemove" @click="$emit('delete', comic)"
          >Remove Comic</v-btn
        >
        <tag-dialog
          @add-tag="$emit('add-tag', $event)"
          v-if="showRemove"
          :comic="comic"
        />
      </v-card-actions>
    </v-card>
  </v-menu>
</template>

<script>
import TagDialog from "./Dialogs/TagDialog.vue";

export default {
  components: { TagDialog },
  props: ["comic", "showRemove"],
  data() {
    return {
      showDetail: false,
      comicDetails: {
        name: "",
        comicID: "",
        issueNumber: "",
        image: { mediumUrl: "" },
        coverDate: "",
      },
    };
  },
  computed: {
    imageHeight() {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return 200;
        case "sm":
          return 250;
        case "md":
          return 350;
        case "lg":
          return 400;
        default:
          return 450;
      }
    },
    creatorList() {
      return this.makeStringOfNames(this.comic.creators);
    },
    characterList() {
      return this.makeStringOfNames(this.comic.characters);
    },
  },
  methods: {
    makeStringOfNames(arrayToFilter) {
      return arrayToFilter.reduce((acc, object, i) => {
        if (i === 0) {
          return object.name;
        } else {
          return `${acc}, ${object.name}`;
        }
      }, "");
    },
  },
};
</script>
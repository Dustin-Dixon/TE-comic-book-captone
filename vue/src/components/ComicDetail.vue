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
        <div>Cover Date: {{ comic.coverDate }}</div>
        <div v-if="comic.creators">Creators: {{ creatorList }}</div>
      </v-card-text>
      <v-card-actions>
        <v-btn v-if="showRemove" @click="$emit('delete', comic)"
          >Remove Comic</v-btn
        >
        <tag-dialog v-if="showRemove" :comic="comic" />
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
      return this.comic.creators.reduce((acc, creator, i) => {
        if (i === 0) {
          return creator.name;
        } else {
          return `${acc}, ${creator.name}`;
        }
      }, "");
    },
  },
};
</script>
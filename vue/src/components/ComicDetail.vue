<template>
  <v-card
    v-model="showDetail"
    :close-on-content-click="false"
    :nudge-width="200"
    offset-x
    elevation="2"
  >
    <template v-slot:activator="{ on, attrs }">
      <v-btn
        icon
        v-on="on"
        v-bind="attrs"
        @click="newCollection = { ...comic }"
      >
        <v-icon>mdi-info-outline</v-icon>
      </v-btn>
    </template>
    <v-img :src="comic.image.mediumUrl"></v-img>
    <v-card-title align="center">{{ comic.name }}</v-card-title>
    <v-card-text>
      <div>Issue Number: {{ comic.issueNumber }}</div>
      <div>Cover Date: {{ comic.coverDate }}</div>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  props: ["comic"],
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
};
</script>
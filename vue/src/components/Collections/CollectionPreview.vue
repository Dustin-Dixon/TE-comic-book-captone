<template>
  <v-sheet height="100%" tile>
    <v-container>
      <v-row>
        <v-col>
          <h2 class="text-h3 text-center">{{ collection.name }}</h2>
        </v-col>
      </v-row>
    </v-container>
  </v-sheet>
</template>

<script>
export default {
  props: ["collection"],
};
</script>
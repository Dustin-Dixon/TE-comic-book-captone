<template>
  <v-sheet height="100%" tile>
    <v-row align="center" justify="center">
      {{ collection.name }}
    </v-row>
  </v-sheet>
</template>

<script>
export default {
  props: ["collection"],
};
</script>
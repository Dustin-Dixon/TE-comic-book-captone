<template>
  <v-sheet height="100%" tile>
    <v-container>
      <v-row>
        <v-col>
          <h3 class="text-h3 text-center">{{ collection.name }}</h3>
          <h4 class="text-h4 text-center">By: {{collection.username}}</h4>
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
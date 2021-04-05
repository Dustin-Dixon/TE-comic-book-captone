<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <v-btn :to="{ name: 'home' }" text exact>Home</v-btn>
      <v-btn :to="{ name: 'my-collections' }" text v-if="loggedIn"
        >Collections</v-btn
      >
      <v-spacer />
      <v-btn :to="{ name: 'login' }" text v-if="!loggedIn">Log In</v-btn>
      <v-btn :to="{ name: 'logout' }" text v-if="loggedIn">Log Out</v-btn>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script>
export default {
  name: "App",
  computed: {
    loggedIn() {
      return this.$store.getters.loggedIn;
    },
  },
};
</script>

<template>
  <v-app>
    <v-app-bar app color="primary">
      <v-btn
        id="home-button"
        :ripple="false"
        class="ml-1 mr-2"
        :to="{ name: 'home' }"
        icon
        exact
        large
        ><v-img src="../public/power_comics_icon.png" height="40px" contain
      /></v-btn>
      <v-btn
        :to="{ name: 'my-collections' }"
        class="text-nav"
        text
        v-if="loggedIn"
        >Collections</v-btn
      >
      <v-btn
        :to="{ name: 'about-us' }"
        class="text-nav"
        text
        >About Us</v-btn
        >
      <v-spacer />
      <v-btn :to="{ name: 'register' }" class="text-nav" text v-if="!loggedIn">
        Sign Up
      </v-btn>
      <v-btn :to="{ name: 'login' }" class="text-nav" text v-if="!loggedIn">
        Log In
      </v-btn>
      <v-btn :to="{ name: 'logout' }" class="text-nav" text v-if="loggedIn">
        Log Out
      </v-btn>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
    <v-footer>POWer Comics &copy;2021</v-footer>
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

<style scoped>
#home-button::before {
  background-color: transparent;
}
</style>

<template>
  <div id="app">
    <div id="nav">
      <router-link :to="{ name: 'home' }">Home</router-link>
      <router-link :to="{ name: 'my-collections'}" v-if="loggedIn">Collections</router-link>
      <router-link :to="{ name: 'login' }" v-if="!loggedIn">Log In</router-link>
      <router-link :to="{ name: 'logout' }" v-if="loggedIn">Logout</router-link>
    </div>
    <router-view />
  </div>
</template>

<script>

export default {
  computed: {
    loggedIn() {
      return this.$store.state.token != ''
    }
  }
}
</script>

<style scoped>
  #nav a {
    border-right: 2px solid black;
    padding: 0 5px;
  }

  #nav a:last-of-type {
    border-right: none;
    padding-right: none;
  }
</style>
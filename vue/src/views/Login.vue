<template>
  <v-container>
    <v-form @submit.prevent="login">
      <v-row>
        <v-col align="center">
          <h1>Please Sign In</h1>
        </v-col>
      </v-row>
      <v-row v-if="invalidCredentials">
        <v-col>
          <v-alert type="error">Invalid username and password!</v-alert>
        </v-col>
      </v-row>
      <v-row v-if="this.$route.query.registration">
        <v-col>
          <v-alert type="success">
            Thank you for registering, please sign in.
          </v-alert>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="5" offset="1">
          <v-text-field autofocus label="Username" v-model="user.username" />
        </v-col>
        <v-col cols="5">
          <v-text-field
            label="Password"
            v-model="user.password"
            type="password"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="5" offset="1" align="center">
          <v-btn type="submit">Submit</v-btn>
        </v-col>
        <v-col cols="5" align="center">
          <v-btn :to="{ name: 'register' }">Sign Up</v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-container>
</template>

<script>
import authService from "../services/AuthService";

export default {
  name: "login",
  components: {},
  data() {
    return {
      user: {
        username: "",
        password: "",
      },
      invalidCredentials: false,
    };
  },
  methods: {
    login() {
      authService
        .login(this.user)
        .then((response) => {
          if (response.status == 200) {
            this.$store.commit("SET_AUTH_TOKEN", response.data.token);
            this.$store.commit("SET_USER", response.data.user);
            this.$router.push("/");
          }
        })
        .catch((error) => {
          const response = error.response;

          if (response.status === 401) {
            this.invalidCredentials = true;
          }
        });
    },
  },
};
</script>

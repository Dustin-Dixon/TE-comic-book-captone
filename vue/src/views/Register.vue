<template>
  <v-container>
    <v-form @submit.prevent="register">
      <v-row>
        <v-col align="center">
          <h1 class="text-h1">Create Account</h1>
        </v-col>
      </v-row>
      <v-row v-if="registrationErrors">
        <v-col>
          <v-alert type="error">{{ registrationErrorMsg }}</v-alert>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="5" offset="1">
          <v-text-field autofocus label="Username" v-model="user.username" />
        </v-col>
        <v-col cols="5">
          <v-select :items="roles" v-model="user.role" />
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="5" offset="1">
          <v-text-field
            label="Password"
            v-model="user.password"
            type="password"
          />
        </v-col>
        <v-col cols="5">
          <v-text-field
            label="Confirm Password"
            v-model="user.confirmPassword"
            type="password"
          />
        </v-col>
      </v-row>

      <v-row>
        <v-col cols="5" offset="1" align="center">
          <v-btn type="submit">Submit</v-btn>
        </v-col>
        <v-col cols="5" align="center">
          <v-btn :to="{ name: 'login' }">Have an account?</v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-container>
</template>

<script>
import authService from "../services/AuthService";

export default {
  name: "register",
  data() {
    return {
      user: {
        username: "",
        password: "",
        confirmPassword: "",
        role: "standard",
      },
      roles: [
        {
          text: "Standard",
          value: "standard",
        },
        {
          text: "Premium",
          value: "premium",
        },
      ],
      registrationErrors: false,
      registrationErrorMsg: "There were problems registering this user.",
    };
  },
  methods: {
    register() {
      if (this.user.password != this.user.confirmPassword) {
        this.registrationErrors = true;
        this.registrationErrorMsg = "Password & Confirm Password do not match.";
      } else {
        authService
          .register(this.user)
          .then((response) => {
            if (response.status == 201) {
              this.$router.push({
                path: "/login",
                query: { registration: "success" },
              });
            }
          })
          .catch((error) => {
            const response = error.response;
            this.registrationErrors = true;
            if (response.status === 400) {
              this.registrationErrorMsg = "Bad Request: Validation Errors";
            }
          });
      }
    },
    clearErrors() {
      this.registrationErrors = false;
      this.registrationErrorMsg = "There were problems registering this user.";
    },
  },
};
</script>
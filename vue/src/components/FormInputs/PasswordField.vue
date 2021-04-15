<template>
  <v-text-field
    class="password-field"
    :label="label"
    :value="value"
    @input="$emit('update:value', $event)"
    :type="visible ? 'text' : 'password'"
    :append-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
    @click:append="visible = !visible"
  />
</template>

<script>
export default {
  props: ["label", "value"],
  data() {
    return {
      visible: false,
    };
  },
};
</script>
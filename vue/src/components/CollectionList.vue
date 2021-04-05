<template>
  <div>
    <div
      class="collection"
      v-for="collection in collections"
      :key="collection.id"
    >
      {{ collection.name }}
    </div>
  </div>
</template>

<script>
export default {
  name: "CollectionList",
  props: ["collections"],
};
</script>
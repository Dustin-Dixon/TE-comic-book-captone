<template>
  <div>
    <h1>My Collections</h1>
    <collection-list :collections="collections" />
  </div>
</template>

<script>
import CollectionList from '../components/CollectionList.vue';
import CollectionService from "../services/CollectionService";

export default {
  name: 'MyCollections',
  components: { CollectionList },
  data() {
    return {
      collections: [],
      error: '',
    };
  },
  created() {
    CollectionService.getUserCollections().then((response) => {
      if (response.status === 200) {
        this.collections = response.data;
      }
    }).catch(error => {
      console.log(error);
      this.error = error;
    });
  },
};
</script>
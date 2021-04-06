import axios from 'axios';

export default {
    getUserCollections() {
        return axios.get("/user/collection");
    },

    addCollection(collection) {
        return axios.post("/user/collection", collection);
    },

    addComicToCollection(collection, comic) {
        return axios.post(`/user/collection/${collection.collectionID}`, comic);
    }
}
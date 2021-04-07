import axios from 'axios';

export default {
    getUserCollections() {
        return axios.get("/user/collection");
    },

    addCollection(collection) {
        return axios.post("/user/collection", collection);
    },

    getComicsInCollection(collection) {
        return axios.get(`/user/collection/${collection.collectionID}`);
    },

    addComicToCollection(collection, comic) {
        return axios.post(`/user/collection/${collection.collectionID}`, comic);
    }
}
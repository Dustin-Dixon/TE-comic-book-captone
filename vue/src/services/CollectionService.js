import axios from 'axios';

export default {
    getUserCollections() {
        return axios.get("/user/collection");
    },

    addCollection(collection) {
        return axios.post("/user/collection", collection);
    },

    getComicsInCollection(collectionID) {
        return axios.get(`/user/collection/${collectionID}`);
    },

    addComicToCollection(collection, comic) {
        return axios.post(`/user/collection/${collection.collectionID}`, comic);
    },

    updateCollectionSettings(collection) {
        return axios.put(`/user/collection/${collection.collectionID}`, collection);
    },

    getPublicCollections() {
        return axios.get("/anonymous/collection");
    },

    getComicsInPublicCollection(collectionID) {
        return axios.get(`anonymous/collection/${collectionID}/comic`);
    },

    getPublicCollectionFromID(collectionID) {
        return axios.get(`anonymous/collection/${collectionID}`);
    },

    deleteComicFromCollection(collectionID,comicID) {
        return axios.delete(`user/collection/${collectionID}/comic/${comicID}`)
    },

    getCollectionStats(collectionId) {
        return axios.get(`/collection/${collectionId}/stats`);
    },
}
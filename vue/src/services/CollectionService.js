import axios from 'axios';

export default {
    getUserCollections() {
        return axios.get("/user/collections");
    }
}
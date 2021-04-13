import axios from 'axios';

export default {
    searchLocalComics(searchTerms) {
        return axios.get(`/search/local?searchTerm=${searchTerms}`);
    },
    searchOnline(name) {
        return axios.get(`/search/issues?name=${name}`);
    }
}
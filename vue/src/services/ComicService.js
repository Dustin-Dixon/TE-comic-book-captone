import axios from 'axios';

export default {
    searchLocalComics(searchTerms) {
        return axios.get(`/search/local?searchTerm=${searchTerms}`);
    },
    searchOnline(name, description) {
        return axios.get(`/search/issues?name=${name}&description=${description}`);
    }
}
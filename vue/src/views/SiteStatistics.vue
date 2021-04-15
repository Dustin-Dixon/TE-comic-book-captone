<template>
  <v-container>
    <v-row>
      <v-col align="center">
        <v-content class="text-h3"> Total Comics by Tags </v-content>
      </v-col>
    </v-row>
    <v-row>
        <v-col align="center">
            <v-content class="text-h6" v-for="tagCount in firstHalfTags" :key="tagCount.description">
                {{tagCount.count}} {{tagCount.description}} {{PluralOrNot(tagCount.count)}}
            </v-content>
        </v-col>
        <v-col align="center">
            <v-content class="text-h6" v-for="tagCount in secondHalfTags" :key="tagCount.description">
                {{tagCount.count}} {{tagCount.description}} {{PluralOrNot(tagCount.count)}}
            </v-content>
        </v-col>
    </v-row>
  </v-container>
</template>

<script>
import ComicService from "../services/ComicService.js";

export default {
    data() {
        return {
            error: "",
            tagList: [],
            firstHalfTags: [],
            secondHalfTags: [],
        }
    },
    methods: {
        FirstHalf(tagList) {
            var half = tagList.length / 2;
            this.firstHalfTags = this.tagList.splice(0, half);
            this.secondHalfTags = this.tagList.splice(0, tagList.length);
        },
        PluralOrNot(count){
            if (count === 1){
                return 'Comic';
            }
            else return 'Comics'
        }
    },
    created() {
        return ComicService.getAllTags()
            .then((response) => {
                if (response.status === 200) {
                    this.tagList = response.data;
                    this.FirstHalf(this.tagList);
                }
            })
            .catch((error) => {
                console.log(error);
                this.error = error;
            });
    },
};
</script>
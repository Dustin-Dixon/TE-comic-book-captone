<template>
  <v-container fluid>
    <v-row>
      <v-col align="center">
        <h1 class="text-h3"> Total Comics by Tag </h1>
      </v-col>
    </v-row>
    <v-divider class="my-4" />
    <v-row>
        <v-col cols="4" offset="2" align="center">
            <div v-for="tagCount in firstHalfTags" :key="tagCount.description">
                <p class="text-h6">{{tagCount.description}} {{PluralOrNot(tagCount.count)}}</p>
                <p class="text-nav">{{tagCount.count}}</p>
            </div>
        </v-col>
        <v-col cols="4" align="center">
            <div v-for="tagCount in secondHalfTags" :key="tagCount.description">
                <p class="text-h6">{{tagCount.description}} {{PluralOrNot(tagCount.count)}}</p>
                <p class="text-nav">{{tagCount.count}}</p>
            </div>
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
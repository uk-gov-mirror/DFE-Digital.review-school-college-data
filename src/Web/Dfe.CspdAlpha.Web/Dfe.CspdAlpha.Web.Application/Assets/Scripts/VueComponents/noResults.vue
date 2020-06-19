<template>
  <div v-if="!hasResults">
    <p>No records matched. <a href="#" v-on:click.prevent="clearSearch">Clear search</a></p>
  </div>
</template>

<script>
  export default {
    name: "noResults",
    props: {
      searchText: String,
      hasResults: Boolean,
    },
    methods: {
      clearSearch: function() {
        this.$root.clearSearch();
      }
    }
  }
</script>

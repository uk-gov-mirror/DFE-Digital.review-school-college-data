import Vue from 'vue';

import pupilSearch from '../VueComponents/pupilSearch';
import pupilTable from '../VueComponents/pupilTable';
import noResults from '../VueComponents/noResults';

const pupilListApp = new Vue({
  el: '#pupil-list-app',
  components: {
    'pupil-search': pupilSearch,
    'pupil-table': pupilTable,
    'no-results': noResults,
  },
  data: {
    pupils: [],
    searchText: '',
    urn: '',
    columns: ['First name', 'Last name', 'UPN', 'View pupil'],
    isLoading: false,
  },
  mounted: function() {
    this.pupils = window.pupilsJson.map((pupil) => {
      pupil.fullname = pupil.FirstName.toLowerCase() + ' ' + pupil.LastName.toLowerCase();
      return pupil;
    });

    this.urn = window.urn;
  },
  methods: {
    filterPupils: function(){
      const searchText = this.searchText;
      this.filteredPupils = this.pupils.filter((pupil) => {
        return pupil.fullname.indexOf(searchText) > -1;
      });
    },
    clearSearch: function(){
      this.searchText = '';
      document.getElementById('pupil-search').focus();
    }

  },
  computed: {
    filteredPupils: function(){
      const searchText = this.searchText.toLowerCase();
      return this.pupils.filter((pupil) => {
        return pupil.fullname.indexOf(searchText) > -1;
      });
    },
    isFiltering: function(){
      return this.searchText.length > 0;
    },
    hasResults: function(){
      return this.isFiltering ? this.filteredPupils.length > 0 : true;
    }
  }
});


export default pupilListApp;

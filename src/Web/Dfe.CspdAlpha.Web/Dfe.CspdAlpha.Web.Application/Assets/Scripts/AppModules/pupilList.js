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
    idBinding: '',
    searchText: '',
    urn: '',
    columns: [],
    isLoading: false
  },
  mounted: function() {
    this.pupils = window.pupilsJson.map((pupil) => {
      if (pupil.ULN){
        pupil.searchCriteria = pupil.FirstName.toLowerCase() + ' ' + pupil.LastName.toLowerCase() + ' ' + pupil.ULN.toLowerCase();
      }
      else if (pupil.UPN){
        pupil.searchCriteria = pupil.FirstName.toLowerCase() + ' ' + pupil.LastName.toLowerCase() + ' ' + pupil.UPN.toLowerCase();
      } else {
        pupil.searchCriteria = pupil.FirstName.toLowerCase() + ' ' + pupil.LastName.toLowerCase();
      }
      return pupil;
    });
    if (window.checkingWindowURL.indexOf('ks5') > -1) {
      this.columns = ['First name', 'Last name', 'ULN', 'View student'];
      this.idBinding = 'ULN';
    } else {
      this.columns = ['First name', 'Last name', 'UPN', 'View pupil'];
      this.idBinding = 'UPN';
    }
    this.urn = window.urn;
  },
  methods: {
    filterPupils: function(){
      const searchText = this.searchText;
      this.filteredPupils = this.pupils.filter((pupil) => {
        return pupil.searchCriteria.indexOf(searchText) > -1;
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
        return pupil.searchCriteria.indexOf(searchText) > -1;
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

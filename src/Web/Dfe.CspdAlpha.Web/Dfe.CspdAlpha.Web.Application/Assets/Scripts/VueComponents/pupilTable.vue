<template>
  <table class="govuk-table app-pupil-list-table" v-if="pupils.length > 0">
    <thead class="govuk-table__head">
      <tr class="govuk-table__row">
        <th class="govuk-table__header" v-for="col in columns">
          {{col}}
        </th>
      </tr>
    </thead>
    <tbody class="govuk-table__body">
      <tr class="govuk-table__row" v-for="pupil in pupils">
        <td class="govuk-table__cell" data-label="First name">{{pupil.FirstName}}</td>
        <td class="govuk-table__cell" data-label="Last name">{{pupil.LastName}}</td>
        <td class="govuk-table__cell" data-label="pupil ID">{{pupil.PupilId}}</td>
        <td class="govuk-table__cell" v-html="viewLink(pupil.PupilId)"></td>
        <td class="govuk-table__cell" v-html="amendLink(pupil.PupilId)"></td>
      </tr>
    </tbody>
  </table>

</template>

<script>
  export default {
    name: "pupilTable",
    props: {
      pupils: Array,
      columns: Array,
      urn: String,
    },
    methods: {
      viewLink: function(pupilId){
        const href = '/school/' + this.urn + '/Pupil/View/' + pupilId
        return `<a href="${href}">View pupil</a>`
      },
      amendLink: function(pupilId) {
        const href = '/school/' + this.urn + '/Results/View/' + pupilId;
        return `<a href="${href}">View/amend results</a>`;
      }
    }
  }
</script>


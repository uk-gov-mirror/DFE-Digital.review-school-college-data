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
        <td class="govuk-table__cell" data-label="UPN">{{pupil.UPN}}</td>
        <td class="govuk-table__cell" v-html="viewLink(pupil.PupilId)"></td>
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
      checkingWindowURL: String,
    },
    methods: {
      viewLink: function(pupilId){
        const href = '/' + checkingWindowURL + '/' + this.urn + '/Pupil/View/' + pupilId
        return `<a href="${href}">${this.columns[3]}</a>`
      },
    }
  }
</script>


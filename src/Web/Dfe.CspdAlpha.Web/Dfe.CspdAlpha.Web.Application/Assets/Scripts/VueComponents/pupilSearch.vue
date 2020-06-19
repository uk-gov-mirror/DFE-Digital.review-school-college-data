<template>
  <div class="govuk-form-group">
    <label class="govuk-label" for="pupil-search">
      Search for a pupil
    </label>
    <input v-bind:value="value" v-on:input="$emit('input', $event.target.value)"
           class="govuk-input govuk-!-width-one-half" id="pupil-search" type="text">
  </div>
</template>

<script>
  export default {
    name: 'pupilSearch',
    props: ['value'],
  }
</script>


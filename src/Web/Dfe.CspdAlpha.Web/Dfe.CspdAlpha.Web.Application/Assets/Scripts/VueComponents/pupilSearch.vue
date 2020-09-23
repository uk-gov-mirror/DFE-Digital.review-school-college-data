<template>
    <input v-bind:value="value" v-on:input="$emit('input', $event.target.value)"
           class="govuk-input govuk-!-width-one-half" id="pupil-search" type="text">
</template>

<script>
  export default {
    name: 'pupilSearch',
    props: ['value'],
  }
</script>


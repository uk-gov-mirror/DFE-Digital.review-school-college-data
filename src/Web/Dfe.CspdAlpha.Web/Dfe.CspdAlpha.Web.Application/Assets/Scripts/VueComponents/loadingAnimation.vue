<template>
  <div class="dot-pulse" v-if="shouldShow"></div>
</template>

<script>
  export default {
    name: "loadingAnimation",
    props: {
      shouldShow: Boolean
    }
  }
</script>

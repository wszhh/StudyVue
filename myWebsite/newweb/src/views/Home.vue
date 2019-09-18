<template>
  <div id="Home">
    <NowTime></NowTime>
  </div>
</template>

<script>
import NowTime from "@/components/NowTime.vue";
export default {
  name: "home",
  data() {
    return {};
  },
  components: {
    NowTime
  }
};
</script>

<style>

</style>
<template>
  <div class="dashboard-container zdy-border">
    <div class="dashboard-text">用户名: {{ name }} 时间：{{ timeaa }}</div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { getTime } from "@/api/user";

export default {
  name: "dashboard",
  data() {
    return {
      timeaa: "时间获取失败"
    };
  },
  computed: {
    ...mapGetters(["name"])
  },
  created: function() {
    getTime().then(Response => (this.timeaa = Response.data));
  }
};
</script>

<style lang="scss" scoped>
.dashboard {
  &-container {
    margin: 30px;
  }
  &-text {
    font-size: 30px;
    line-height: 46px;
  }
}
</style>

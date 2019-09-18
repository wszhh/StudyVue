<template>
  <div>
    <p>{{time}}</p>
    <!-- <el-button @click="getTime" type="warning">获取当前时间</el-button> -->
  </div>
</template>

<script>
//import { METHODS } from "http";
export default {
  name: "Time",
  data() {
    return {
      time: "时间获取失败"
    };
  },
  methods: {},
  created: function() {
    var _this = this;
    window.setInterval(() => {
      _this.$axios
        .get("/Home")
        .then(response => {
          _this.time = response.data;
        })
        .catch(error => {
          _this.time = error;
          _this.serrored = true;
        });
    }, 3000);
  }
};
</script>

<style>
</style>
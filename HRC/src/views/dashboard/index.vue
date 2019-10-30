<template>
  <div class="dashboard-container zdy-border">
    <div class="dashboard-text">用户名: {{ name }} 时间：{{ timeaa }}</div>
    <!-- 做个实验 2019-10-30 20:57:22 -->
    <div v-if="false">
      <el-input v-model="user"></el-input>
      <el-input v-model="message"></el-input>
      <el-button @click="send">发送</el-button>
      <el-button @click="testt">test</el-button>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { getTime, test } from "@/api/user";
import { getToken } from "@/utils/auth";
const state = {
  token: getToken()
};
const signalR = require("@aspnet/signalr");
export default {
  name: "dashboard",
  data() {
    return {
      timeaa: "时间获取失败",
      user: "",
      message: "",
      url: process.env.VUE_APP_BASE_API + "/signalrHubs"
    };
  },
  computed: {
    ...mapGetters(["name"])
  },
  methods: {
    async testt() {
      this.creatSend();
      await test({ message: this.user });
    },
    send() {
      var _this = this;
      let connection = new signalR.HubConnectionBuilder()
        .withUrl(this.url, {
          accessTokenFactory: () => state.token
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();
      /////////////////////////////
      connection.on("ReceiveMessage", function(user, message) {
        console.log(user + ":" + message);
      });
      //////////////////////////////
      connection
        .start()
        .then(res => {
          connection
            .invoke("SendMessage", this.user, this.message)
            .catch(function(err) {
              console.log("发送失败");
              return console.error(err.toString());
            });
        })
        .catch(err => {
          console.log("开始失败！");
        });
    },
    creatSend() {
      let connection = new signalR.HubConnectionBuilder()
        .withUrl(this.url, {
          accessTokenFactory: () => state.token
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();
      /////////////////////////////
      connection.on("ReceiveMessage", function(user, message) {
        console.log(user + message);
      });
      //////////////////////////////
      connection
        .start()
        .then(res => {
          connection
            .invoke("SendMessage", this.user, this.message)
            .catch(function(err) {
              console.log("发送失败");
              return console.error(err.toString());
            });
        })
        .catch(err => {
          console.log("开始失败！");
        });
    }
  },
  created() {
    getTime().then(Response => (this.timeaa = Response.data));
    // this.creatSend();
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

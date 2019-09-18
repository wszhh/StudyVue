<template>
  <div class="Register">
    <div class="Login">
      <el-row :gutter="0">
        <el-col :span="14" :offset="8">
          <div class="grid-content bg-purple">
            <UserRegister></UserRegister>
          </div>
        </el-col>
      </el-row>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
import UserRegister from "@/components/UserRegister.vue";
export default {
  name: "register",
  components: {
    UserRegister
  },
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
.el-header,
.el-footer {
  background-color: #b3c0d1;
  color: #333;
  text-align: center;
  line-height: 60px;
}

.el-aside {
  background-color: #d3dce6;
  color: #333;
  text-align: center;
  line-height: 200px;
}

.el-main {
  background-color: #e9eef3;
  color: #333;
  text-align: center;
  line-height: 160px;
}

body > .el-container {
  margin-bottom: 40px;
}

.el-container:nth-child(5) .el-aside,
.el-container:nth-child(6) .el-aside {
  line-height: 260px;
}

.el-container:nth-child(7) .el-aside {
  line-height: 320px;
}
</style>

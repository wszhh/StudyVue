<template>
  <div class="Login">
    <el-row :gutter="0">
      <el-col :span="14" :offset="8">
        <div class="grid-content bg-purple">
          <UserLogin></UserLogin>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
// @ is an alias to /src
import UserLogin from "@/components/UserLogin.vue";
export default {
  name: "login",
  components: {
    UserLogin
  },
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
</style>

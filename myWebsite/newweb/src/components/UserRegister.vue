<template>
  <!--  -->

  <div
    id="login"
    style="box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);padding:20px;width:410px;margin-top:20%"
  >
    <el-form
      :model=" registerForm"
      :rules="registerrulers"
      ref="registerForm"
      label-width="100px"
      class="el-form demo-ruleForm login-container el-form--label-left"
    >
      <el-row :gutter="20">
        <el-col :span="12" :offset="10">
          <div class="grid-content bg-purple">
            <h3 class="title">注册</h3>
          </div>
        </el-col>
      </el-row>
      <div class="el-form-item is-required el-form-item--medium">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <div class="el-input el-input--medium">
            <el-form-item
              label="用户名"
              :error="error.UserNameErrorMsg"
              prop="username"
              class="el-form-item is-required el-form-item--medium"
            >
              <el-input v-model="registerForm.username" name="username"></el-input>
            </el-form-item>
          </div>
        </div>
      </div>
      <div class="el-form-item is-required el-form-item--medium">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <div class="el-input el-input--medium">
            <el-form-item
              label="邮箱"
              prop="email"
              class="el-form-item is-required el-form-item--medium"
            >
              <el-input v-model="registerForm.email" name="email"></el-input>
            </el-form-item>
          </div>
        </div>
      </div>
      <div class="el-form-item is-required el-form-item--medium">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <div class="el-input el-input--medium el-input--suffix">
            <el-form-item
              label="密码"
              prop="password"
              class="el-form-item is-required el-form-item--medium"
            >
              <el-input
                v-model="registerForm.password"
                type="password"
                name="password"
                autocomplete="off"
              ></el-input>
            </el-form-item>
            <span class="el-input__suffix">
              <span class="el-input__suffix-inner">
                <i class="el-input__icon el-icon-view el-input__clear"></i>
              </span>
            </span>
          </div>
        </div>
      </div>
      <div class="el-form-item is-required el-form-item--medium">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <div class="el-input el-input--medium el-input--suffix">
            <el-form-item
              label="确认密码"
              prop="checkPassword"
              class="el-form-item is-required el-form-item--medium"
            >
              <el-input
                v-model="registerForm.checkPassword"
                type="password"
                name="checkPassword"
                autocomplete="off"
              ></el-input>
            </el-form-item>
            <span class="el-input__suffix">
              <span class="el-input__suffix-inner">
                <i class="el-input__icon el-icon-view el-input__clear"></i>
              </span>
            </span>
          </div>
        </div>
      </div>
      <div class="el-form-item el-form-item--medium" style="width: 100%;">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <el-button
            type="primary"
            @click="register"
            class="el-button el-button--primary el-button--medium"
            style="width: 100%;"
          >注册</el-button>
        </div>
      </div>
      <div class="el-form-item el-form-item--medium" style="width: 100%;">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <router-link to="/">
            <el-button
              type="success"
              class="el-button el-button--primary el-button--medium"
              style="width: 100%;"
            >已经注册？前往登录</el-button>
          </router-link>
        </div>
      </div>
    </el-form>
  </div>
</template>

<script>
export default {
  data() {
    var validatePass = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        if (this.registerForm.checkPassword !== "") {
          this.$refs.registerForm.validateField("checkPassword");
        }
        callback();
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.registerForm.password) {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };
    return {
      registerForm: {
        username: "",
        password: "",
        checkPassword: "",
        email: ""
      },
      error: {
        UserNameErrorMsg: ""
      },
      registerrulers: {
        username: [
          { required: true, message: "请输入用户名", trigger: "blur" },
          { min: 3, max: 10, message: "长度在 3 到 10 个字符", trigger: "blur" }
        ],
        password: [{ validator: validatePass, trigger: "blur" }],
        checkPassword: [{ validator: validatePass2, trigger: "blur" }],
        email: [
          { required: true, message: "请输入邮箱地址", trigger: "blur" },
          {
            type: "email",
            message: "请输入正确的邮箱地址",
            trigger: ["blur", "change"]
          }
        ]
      }
    };
  },
  methods: {
    register() {
      this.$refs.registerForm.validate(valid => {
        if (valid) {
          this.$axios
            .post("/Account/Regiseter", {
              UserName: this.registerForm.username,
              Password: this.registerForm.checkPassword,
              Email: this.registerForm.email
            })
            .then(successResponse => {
              this.responseResult = JSON.stringify(successResponse.data);
              if (successResponse.data === 1) {
                this.$router.replace({ path: "/" });
              } else if (successResponse.data === 0) {
                this.error.UserNameErrorMsg = Math.random().toString();
                this.$nextTick(() => {
                  this.error.UserNameErrorMsg = "该用户名已被使用";
                });
                this.$notify.error({
                  title: "错误",
                  message: "注册失败"
                });
              }
            })
            .catch(failResponse => {});
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    }
  }
};
</script>

<style>
</style>
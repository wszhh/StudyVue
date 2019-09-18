<template>
  <div
    id="login"
    style="box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);padding:20px;width:410px;margin-top:20%"
  >
    <el-form
      :model="loginForm"
      :rules="loginrules"
      ref="loginForm"
      label-width="100px"
      class="el-form demo-ruleForm login-container el-form--label-left"
    >
      <el-row :gutter="20">
        <el-col :span="12" :offset="9">
          <div class="grid-content bg-purple">
            <h3 class="title">系统登录</h3>
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
              <el-input v-model="loginForm.username" name="username"></el-input>
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
              <el-input v-model="loginForm.password" type="password" name="password"></el-input>
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
            @click.native.prevent="login"
            :loading="state.LoginBtnIsLoading"
            class="el-button el-button--primary el-button--medium"
            style="width: 100%;"
          >登录</el-button>
        </div>
      </div>
      <div class="el-form-item el-form-item--medium" style="width: 100%;">
        <div class="el-form-item__content" style="margin-left: 0px;">
          <router-link to="/Register">
            <el-button
              type="success"
              class="el-button el-button--primary el-button--medium"
              style="width: 100%;"
            >没有账号？前往注册</el-button>
          </router-link>
        </div>
      </div>
    </el-form>
  </div>
</template>

<script>
export default {
  name: "Login",
  data() {
    return {
      loginForm: {
        username: "",
        password: ""
      },
      error: {
        UserNameErrorMsg: ""
      },
      state: {
        LoginBtnIsLoading: false
      },
      loginrules: {
        username: [
          { required: true, message: "请输入用户名", trigger: "blur" },
          { min: 3, max: 10, message: "长度在 3 到 10 个字符", trigger: "blur" }
        ],
        password: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { min: 6, max: 18, message: "长度在 6 到 18 个字符", trigger: "blur" }
        ]
      }
    };
  },
  methods: {
    login(ev) {
      var that = this;
      that.$store.commit("saveToken", "");
      this.$refs.loginForm.validate(valid => {
        if (valid) {
          this.state.LoginBtnIsLoading = true;
          this.$axios
            .post("/Account/Login", {
              UserName: this.loginForm.username,
              Password: this.loginForm.password
            })
            .then(successResponse => {
              if (successResponse.data.code === 1) {
                ///
                var token = successResponse.data.token.token;
                var expires_in = successResponse.data.token.expires_in;
                that.$store.commit("saveToken", token); //保存 token
                //console.log("expires_in:" + expires_in);
                var curTime = new Date();
                var expiredate = new Date(
                  curTime.setSeconds(curTime.getSeconds() + expires_in)
                );
                //console.log("expiredate:" + expiredate);
                //console.log("NowTime" + new Date());
                window.localStorage.refreshtime = expiredate;
                //console.log("refreshtime:" + window.localStorage.refreshtime);
                that.$store.commit("saveTokenExpire", expiredate);
                window.localStorage.expires_in = expires_in;
                //console.log(that.$store.state.token);
                // this.$notify.success({
                //   title: "登录成功",
                //   message: "欢迎你 " + this.loginForm.username + " !"
                // });
                that.getUserInfoByToken(token);
                // this.$router.push({ name: "Home" });
              } else if (successResponse.data === 0) {
                this.error.UserNameErrorMsg = Math.random().toString();
                this.$nextTick(() => {
                  this.error.UserNameErrorMsg = "用户名或密码错误";
                });
                this.state.LoginBtnIsLoading = false;
                this.$notify.error({
                  title: "错误",
                  message: "登录失败"
                });
              } else if (successResponse.data === 2) {
                this.state.LoginBtnIsLoading = false;
                this.error.UserNameErrorMsg = Math.random().toString();
                this.$nextTick(() => {
                  this.error.UserNameErrorMsg = "错误次数超过限制，将锁定5分钟";
                });
                this.$notify.error({
                  title: "错误",
                  message: "登录失败"
                });
              }
            })
            .catch(failResponse => {});
        } else {
          console.log("error submit!!");
          return false;
        }
      });
      ///
    },
    // 获取用户数据
    getUserInfoByToken(token) {
      var _this = this;
      var loginParams = { token: token };
      this.$axios
        .get("/Account/getInfoByToken", { params: loginParams })
        .then(res => {
          var data = res.data;
          if (!data.success) {
            _this.$message({
              message: data.message,
              type: "error"
            });
          } else {
            // _this.closeAlert()
            // _this.openAlert("接收到用户数据，开始初始化路由树...")
            _this.loginStr = "接收到用户数据，开始初始化路由树...";
            window.localStorage.user = JSON.stringify(data.response);
            //      this.$router.replace({ path: "/Home" });
            _this.$router.replace(
              _this.$route.query.redirect ? _this.$route.query.redirect : "/Home"
            );
            if (data.response.uID > 0) {
              _this.GetNavigationBar(data.response.uID);
            }
          }
        });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    }
  }
};
</script>


<style>
</style>



<template>
  <div class="app-container zdy-border">
    <el-form ref="UserInfo" :model="UserInfo" label-width="120px">
      <el-row>
        <el-col :span="12">
          <el-row>
            <el-col :span="12">
              <!-- 本人照片 -->
              <el-form-item label="照片">
                <el-image
                  style="width: 100px; height: 135px;"
                  :src="UserInfo.photo"
                  :preview-src-list="PhotoArray"
                ></el-image>
              </el-form-item>
              <div class="grid-content bg-purple"></div>
            </el-col>
            <el-col :span="12">
              <!-- 照片上传 -->
              <el-upload
                class="avatar-uploader"
                :action="ActionUrl"
                :show-file-list="false"
                :on-success="handleAvatarSuccess"
                :before-upload="beforeAvatarUpload"
                :headers="{Authorization:Authorization}"
                :disabled="!IsSetTrue"
              >
                <img v-if="imageUrl" :src="imageUrl" class="avatar" />
                <i v-else class="el-icon-plus avatar-uploader-icon"></i>
              </el-upload>
              <div class="grid-content bg-purple-light"></div>
            </el-col>
          </el-row>
        </el-col>
        <el-col :span="12">
          <!-- 部门 -->
          <el-form-item label="部门">
            <el-select v-model="UserInfo.departmentId" :disabled="true">
              <el-option
                v-for="item in options"
                :key="item.departmentId"
                :label="item.departmentName"
                :value="item.departmentId"
              ></el-option>
            </el-select>
          </el-form-item>
          <!-- 入职日期 -->
          <el-form-item label="入职日期">
            <el-date-picker
              v-model="UserInfo.joinTime"
              type="date"
              placeholder="暂无数据"
              style="width: 80%;"
              :disabled="true"
              format="yyyy 年 MM 月 dd 日"
            />
          </el-form-item>
          <el-form-item label="薪资">
            <el-input
              v-model="UserInfo.salary"
              placeholder="暂无数据"
              style="width: 80%"
              :disabled="true"
            >
              <template slot="prepend">￥</template>
            </el-input>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="12">
          <!-- 姓名 -->
          <el-form-item label="姓名">
            <el-input
              v-model="UserInfo.realName"
              placeholder="请输入内容"
              style="width: 80%"
              :disabled="!IsSetTrue"
            ></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <!-- 出生日期 -->
          <el-form-item label="出生日期">
            <el-date-picker
              v-model="UserInfo.birthday"
              type="date"
              value-format="yyyy-MM-dd"
              placeholder="暂无数据"
              style="width: 80%;"
              format="yyyy 年 MM 月 dd 日"
              :disabled="!IsSetTrue"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="12">
          <!-- 性别 -->
          <el-form-item label="性别">
            <!-- <el-input v-model="UserInfo.Sex" placeholder="请输入内容" style="width: 80%"></el-input> -->
            <el-select v-model="UserInfo.sex" :disabled="!IsSetTrue">
              <el-option label="男" value="1"></el-option>
              <el-option label="女" value="0"></el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <!-- 电话 -->
          <el-form-item label="电话" prop="phoneNumber">
            <el-input
              v-model.number="UserInfo.phoneNumber"
              show-word-limit
              maxlength="11"
              style="width: 80%"
              :disabled="!IsSetTrue"
            ></el-input>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="12">
          <!-- 地址 -->
          <el-form-item label="地址">
            <el-input
              v-model="UserInfo.address"
              placeholder="请输入内容"
              style="width: 80%"
              :disabled="!IsSetTrue"
            ></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <!-- 个人介绍 -->
          <el-form-item label="个人介绍">
            <el-input
              v-model="UserInfo.introduction"
              type="textarea"
              placeholder="这人个很懒，什么都没写!"
              style="width: 80%"
              :disabled="!IsSetTrue"
            ></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-form-item>
        <el-button type="primary" v-permission="['MyInfo_Set']" @click="onSubmit">保存</el-button>
        <el-button type="danger" @click="dialogVisible=true" v-permission="['MyInfo_SetPwd']">修改密码</el-button>
      </el-form-item>
    </el-form>
    <!-- 修改密码 -->
    <el-dialog title="更改密码" :visible.sync="dialogVisible" width="450px">
      <el-form
        :model="ChangePasswordForm"
        :rules="ChangeRules"
        ref="ChangePasswordForm"
        label-width="120px"
      >
        <el-col>
          <el-form-item label="原密码" prop="oldPassword">
            <el-input
              placeholder="请输入原密码"
              v-model="ChangePasswordForm.oldPassword"
              show-password
              type="text"
              tabindex="1"
              ref="oldPassword"
              class="pwdWidth"
            ></el-input>
          </el-form-item>
        </el-col>
        <el-col>
          <el-form-item label="新密码" prop="newPassword">
            <el-tooltip class="item" effect="light" placement="right">
              <div slot="content">
                不能有空格
                <br />长度应在6-15位字符之间
                <br />建议使用数字、字母、符号的组合形式
              </div>
              <el-input
                placeholder="请输入新密码"
                v-model="ChangePasswordForm.newPassword"
                show-password
                type="text"
                tabindex="2"
                ref="newPassword"
                class="pwdWidth"
              ></el-input>
            </el-tooltip>
          </el-form-item>
        </el-col>
        <el-col>
          <el-form-item label="确认密码" prop="checkPassword">
            <el-tooltip class="item" effect="light" placement="right">
              <div slot="content">
                不能有空格
                <br />长度应在6-15位字符之间
                <br />建议使用数字、字母、符号的组合形式
              </div>
              <el-input
                placeholder="请再次输入新密码"
                v-model="ChangePasswordForm.checkPassword"
                show-password
                type="text"
                tabindex="3"
                ref="checkPassword"
                class="pwdWidth"
              ></el-input>
            </el-tooltip>
          </el-form-item>
        </el-col>
      </el-form>

      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="ChangePWD">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { getToken } from "@/utils/auth";
import { GetUserinfo, SetUserInfo } from "@/api/StaffInfoManage";
import { GetAllDepartments } from "@/api/department";
import { ChangePassword } from "@/api/user";
import checkPermission from "@/utils/permission"; // 权限判断函数
const state = {
  token: getToken()
};
export default {
  name: "MyInfo",
  data() {
    var validatePass = (rule, value, callback) => {
      var nameTest = /^(?![0-9]+$)(?![a-zA-Z]+$)(?![.`~!@#$%^&*]+$)[0-9A-Za-z]{6,15}$/; //校验密码6-15位
      if (value === "") {
        callback(new Error("请输入新密码"));
      } else if (nameTest.test(value)) {
        callback(new Error("密码须包含数字、字母、特殊符号，长度6-15个字符"));
      } else {
        if (this.ChangePasswordForm.checkPassword !== "") {
          this.$refs.ChangePasswordForm.validateField("checkPassword");
        }
        callback();
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value !== this.ChangePasswordForm.newPassword || value === "") {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };
    var validatePass3 = (rule, value, callback) => {
      var nameTest = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,15}$/; //校验密码6-15位
      if (value === "") {
        callback(new Error("请输入原密码"));
      } else if (nameTest.test(value)) {
        callback(new Error("密码须包含数字、字母、特殊符号，长度6-15个字符"));
      } else {
        callback();
      }
    };
    return {
      UserInfo: {
        realName: "",
        departmentId: "",
        phoneNumber: "",
        address: "",
        salary: "",
        birthday: "",
        sex: "",
        introduction: "",
        joinTime: "",
        photo: ""
      },
      IsSetTrue: null,
      Authorization: "Bearer " + state.token,
      dialogVisible: false,
      options: "",
      PhotoArray: [],
      imageUrl: "",
      ActionUrl: process.env.VUE_APP_BASE_API + "/user/setphoto",
      ChangePasswordForm: {
        oldPassword: "",
        newPassword: "",
        checkPassword: ""
      },
      ChangeRules: {
        oldPassword: [
          {
            min: 6,
            max: 15,
            message: "长度在 6 到 15 个字符",
            trigger: "blur"
          },
          { validator: validatePass3, trigger: "blur" }
        ],
        newPassword: [
          {
            min: 6,
            max: 15,
            message: "长度在 6 到 15 个字符",
            trigger: "blur"
          },
          { validator: validatePass, trigger: "blur" }
        ],
        checkPassword: [
          {
            min: 6,
            max: 15,
            message: "长度在 6 到 15 个字符",
            trigger: "blur"
          },
          { validator: validatePass2, trigger: "blur" }
        ]
      }
    };
  },
  methods: {
    checkPermission,
    ChangePWD() {
      this.$refs.ChangePasswordForm.validate(valid => {
        if (valid) {
          ChangePassword(this.ChangePasswordForm).then(res => {
            if (res.code == 20000) {
              //this.$message.success("密码修改成功");
              dialogVisible = false;
            } else {
              //this.$message.error("密码修改失败，请检查原密码");
            }
          });
        }
      });
    },
    async onSubmit() {
      //照片就不传了,没用
      delete this.UserInfo.Photo;
      await SetUserInfo(this.UserInfo).then(res => {
        if (res.code == 20000) {
          //this.$message.success("个人信息保存成功");
        } else {
          //this.$message.error("个人信息保存失败");
        }
      });
    },
    onCancel() {
      this.$message({
        message: this.UserInfo.Sex,
        type: "warning"
      });
    },
    async GetUserInfo() {
      await GetUserinfo(state.token).then(Response => {
        var res = Response.data;
        this.UserInfo = res;
        this.PhotoArray = new Array(res.photo);
        this.UserInfo.sex = res.sex.toString();
      });
    },
    async GetAllDepartments() {
      const { data } = await GetAllDepartments();
      this.options = data;
    },
    handleAvatarSuccess(res, file) {
      this.$message.success("照片更新成功");
      //this.imageUrl = URL.createObjectURL(file.raw);
      this.GetUserInfo();
    },
    beforeAvatarUpload(file) {
      const isJPG = file.type === "image/jpeg";
      const isLt2M = file.size < 102400;

      if (!isJPG) {
        this.$message.error("上传头像图片只能是 JPG 格式!");
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 100Kb!");
      }
      return isJPG && isLt2M;
    }
  },
  created() {
    this.GetUserInfo();
    this.GetAllDepartments();
    this.IsSetTrue = this.checkPermission(["MyInfo_Set"]);
  }
};
</script>

<style scoped>
.pwdWidth {
  width: 250px;
}

.line {
  text-align: center;
}

.avatar-uploader .el-upload {
  border: 1px dashed #8c939d;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 100;
  height: 135px;
  line-height: 135px;
  text-align: center;
}
.avatar {
  width: 100px;
  height: 135px;
  display: block;
}
</style>


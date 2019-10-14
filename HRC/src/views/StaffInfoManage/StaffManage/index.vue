<template>
  <div id="fengye" class="zdy-border">
    <!-- 顶端搜索 -->
    <div style="width:450px;">
      <!-- 添加员工的dialog -->
      <el-dialog title="添加员工" :visible.sync="AdddialogFormVisible" width="70%" top="5vh">
        <!-- 表单开始 -->
        <el-form ref="UserInfo" :model="UserInfo" label-width="120px">
          <el-row>
            <el-col :span="12">
              <el-form-item label="照片">
                <el-upload
                  ref="upload"
                  :action="ActionUrl"
                  :before-upload="isHaveImg"
                  :on-change="checkImg"
                  :on-success="onSuccess"
                  :file-list="fileList"
                  :auto-upload="false"
                  list-type="picture"
                  accept="image/jpeg"
                  :multiple="false"
                  :limit="1"
                  :headers="{Authorization:Authorization}"
                  :data="{UserName:UserInfo.userName}"
                >
                  <el-button slot="trigger" size="small" type="primary">选取文件</el-button>
                  <div slot="tip" style="color:#F56C6C;font-size:10px;">{{error.UploadError}}</div>
                </el-upload>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 部门 -->
              <el-form-item
                label="部门"
                :rules="[{required:true,message:'不得留空'}]"
                prop="departmentId"
              >
                <el-select v-model="UserInfo.departmentId">
                  <el-option
                    v-for="item in options"
                    :key="item.departmentId"
                    :label="item.departmentName"
                    :value="item.departmentId"
                  ></el-option>
                </el-select>
              </el-form-item>
              <!-- 入职日期 -->
              <el-form-item label="入职日期" :rules="[{required:true,message:'不得留空'}]" prop="joinTime">
                <el-date-picker
                  v-model="UserInfo.joinTime"
                  type="date"
                  placeholder="暂无数据"
                  style="width: 80%;"
                  format="yyyy 年 MM 月 dd 日"
                  :default-value="new Date()"
                />
              </el-form-item>
              <el-form-item label="薪资" prop="salary" :rules="[{required:true,message:'不得留空'}]">
                <el-input v-model="UserInfo.salary" placeholder="请输入薪资" style="width: 80%">
                  <template slot="prepend">￥</template>
                </el-input>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <!-- 用户名 -->
              <el-form-item
                label="用户名"
                prop="userName"
                :error="error.nameError"
                :rules="[{required:true,message:'用户名不能为空'},{min:5,max:15,message:'用户名长度介于5-15'}]"
              >
                <el-tooltip class="item" effect="light" content="密码为 Qq123." placement="right">
                  <el-input
                    v-model="UserInfo.userName"
                    placeholder="请输入内容"
                    style="width: 80%"
                    maxlength="15"
                    show-word-limit
                    @change="checThiskUserName"
                  ></el-input>
                </el-tooltip>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 出生日期 -->
              <el-form-item label="出生日期" :rules="[{required:true,message:'不得留空'}]" prop="birthday">
                <el-date-picker
                  v-model="UserInfo.birthday"
                  type="date"
                  value-format="yyyy-MM-dd"
                  placeholder="请选择出生日期"
                  style="width: 80%;"
                  format="yyyy 年 MM 月 dd 日"
                />
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <!-- 姓名 -->
              <el-form-item label="姓名" prop="realName" :rules="[{required:true,message:'不能留空'}]">
                <el-input v-model="UserInfo.realName" placeholder="请输入内容" style="width: 80%"></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 电话 -->
              <el-form-item
                label="电话"
                prop="phoneNumber"
                :rules="[{required:true,message:'不能留空'},{type:'number',message:'只能输入数字'}]"
              >
                <el-input
                  v-model="UserInfo.phoneNumber"
                  show-word-limit
                  maxlength="11"
                  placeholder="请输入电话"
                  style="width: 80%"
                ></el-input>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <!-- 性别 -->
              <el-form-item label="性别" :rules="[{required:true,message:'不能留空'}]" prop="sex">
                <!-- <el-input v-model="UserInfo.Sex" placeholder="请输入内容" style="width: 80%"></el-input> -->
                <el-select v-model="UserInfo.sex">
                  <el-option label="男" value="1"></el-option>
                  <el-option label="女" value="0"></el-option>
                </el-select>
              </el-form-item>
              <!-- 地址 -->
              <el-form-item label="地址" :rules="[{required:true,message:'不能留空'}]" prop="address">
                <el-input v-model="UserInfo.address" placeholder="请输入内容" style="width: 80%"></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 个人介绍 -->
              <el-form-item
                label="个人介绍"
                :rules="[{required:true,message:'不能留空'}]"
                prop="introduction"
              >
                <el-input
                  v-model="UserInfo.introduction"
                  type="textarea"
                  placeholder="这人个很懒，什么都没写!"
                  style="width: 80%"
                ></el-input>
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item>
            <!-- <el-button type="primary" @click="onAddSubmit">保存</el-button> -->
          </el-form-item>
        </el-form>
        <!-- 表单结束 -->
        <div slot="footer" class="dialog-footer">
          <el-button @click="AdddialogFormVisible = false">取 消</el-button>
          <el-button type="primary" @click="onAddSubmit">确 定</el-button>
        </div>
      </el-dialog>
      <!-- 编辑员工的dialog -->
      <el-dialog title="编辑员工" :visible.sync="ChangedialogFormVisible" width="70%" top="5vh">
        <!-- 表单开始 -->
        <el-form ref="ChangeUserInfo" :model="UserInfo" label-width="120px">
          <el-row>
            <el-col :span="12">
              <el-form-item label="照片">
                <el-upload
                  ref="upload"
                  :action="ActionUrl"
                  :before-upload="isHaveImg"
                  :on-change="checkImg"
                  :on-success="onSuccess"
                  :file-list="fileList"
                  :auto-upload="false"
                  list-type="picture"
                  accept="image/jpeg"
                  :multiple="false"
                  :limit="1"
                  :headers="{Authorization:Authorization}"
                  :data="{UserName:UserInfo.userName}"
                >
                  <el-button slot="trigger" size="small" type="primary">选取文件</el-button>
                  <div slot="tip" style="color:#F56C6C;font-size:10px;">{{error.UploadError}}</div>
                </el-upload>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 部门 -->
              <el-form-item
                label="部门"
                :rules="[{required:true,message:'不得留空'}]"
                prop="departmentId"
              >
                <el-select v-model="UserInfo.departmentId">
                  <el-option
                    v-for="item in options"
                    :key="item.departmentId"
                    :label="item.departmentName"
                    :value="item.departmentId"
                  ></el-option>
                </el-select>
              </el-form-item>
              <!-- 入职日期 -->
              <el-form-item label="入职日期" :rules="[{required:true,message:'不得留空'}]" prop="joinTime">
                <el-date-picker
                  v-model="UserInfo.joinTime"
                  type="date"
                  placeholder="暂无数据"
                  style="width: 80%;"
                  format="yyyy 年 MM 月 dd 日"
                  :default-value="new Date()"
                />
              </el-form-item>
              <el-form-item label="薪资" prop="salary" :rules="[{required:true,message:'不得留空'}]">
                <el-input v-model="UserInfo.salary" placeholder="请输入薪资" style="width: 80%">
                  <template slot="prepend">￥</template>
                </el-input>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <!-- 姓名 -->
              <el-form-item label="姓名" prop="realName" :rules="[{required:true,message:'不能留空'}]">
                <el-input v-model="UserInfo.realName" placeholder="请输入内容" style="width: 80%"></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 出生日期 -->
              <el-form-item label="出生日期" :rules="[{required:true,message:'不得留空'}]" prop="birthday">
                <el-date-picker
                  v-model="UserInfo.birthday"
                  type="date"
                  value-format="yyyy-MM-dd"
                  placeholder="请选择出生日期"
                  style="width: 80%;"
                  format="yyyy 年 MM 月 dd 日"
                />
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <!-- 性别 -->
              <el-form-item label="性别" :rules="[{required:true,message:'不能留空'}]" prop="sex">
                <el-select v-model="UserInfo.sex">
                  <el-option label="男" value="1"></el-option>
                  <el-option label="女" value="0"></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 电话 -->
              <el-form-item
                label="电话"
                prop="phoneNumber"
                :rules="[{required:true,message:'不能留空'},{type:'number',message:'只能输入数字'}]"
              >
                <el-input
                  v-model.number="UserInfo.phoneNumber"
                  show-word-limit
                  maxlength="11"
                  placeholder="请输入电话"
                  style="width: 80%"
                ></el-input>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <!-- 地址 -->
              <el-form-item label="地址" :rules="[{required:true,message:'不能留空'}]" prop="address">
                <el-input v-model="UserInfo.address" placeholder="请输入内容" style="width: 80%"></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <!-- 个人介绍 -->
              <el-form-item
                label="个人介绍"
                :rules="[{required:true,message:'不能留空'}]"
                prop="introduction"
              >
                <el-input
                  v-model="UserInfo.introduction"
                  type="textarea"
                  placeholder="这人个很懒，什么都没写!"
                  style="width: 80%"
                ></el-input>
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item>
            <!-- <el-button type="primary" @click="onAddSubmit">保存</el-button> -->
          </el-form-item>
        </el-form>
        <!-- 表单结束 -->
        <div slot="footer" class="dialog-footer">
          <el-button @click="ChangedialogFormVisible = false">取 消</el-button>
          <el-button type="primary" @click="OnChangeSubmit()">确 定</el-button>
        </div>
      </el-dialog>
      <!-- 其它选项 -->
      <el-button
        type="primary"
        v-permission="['Staff_Add']"
        @click="AdddialogFormVisible = true"
      >添加员工</el-button>

      <el-input
        v-permission="['Staff_Find']"
        placeholder="请输入员工姓名"
        class="input-with-select"
        clearable
      >
        <!-- <el-select v-model="select" slot="prepend" placeholder="请选择">
          <el-option label="餐厅名" value="1"></el-option>
          <el-option label="订单号" value="2"></el-option>
          <el-option label="用户电话" value="3"></el-option>
        </el-select>-->
        <el-button
          slot="append"
          icon="el-icon-search"
          @click="FindColleagueByName(FindByname)"
          @keyup.enter="FindColleagueByName(FindByname)"
        ></el-button>
      </el-input>
    </div>
    <!-- 表格 -->
    <el-table
      v-loading="listLoading"
      :data="list"
      element-loading-text="加载中.."
      empty-text="查无此人"
      border
      fit
      highlight-current-row
    >
      <el-table-column align="center" label="编号" width="95">
        <template slot-scope="scope">{{ scope.row.id}}</template>
      </el-table-column>
      <el-table-column align="center" label="姓名" width="95">
        <template slot-scope="scope">{{ scope.row.realName}}</template>
      </el-table-column>

      <el-table-column label="出生日期" align="center" width="200">
        <el-date-picker
          v-model="scope.row.birthday"
          slot-scope="scope"
          value-format="yyyy-MM-dd"
          type="date"
          placeholder="暂无数据"
          style="width: 80%;"
          :default-value="new Date()"
          :disabled="true"
        />
      </el-table-column>

      <el-table-column label="性别" width="55" align="center">
        <template slot-scope="scope">{{ SexFormat(scope.row.sex) }}</template>
      </el-table-column>

      <el-table-column label="部门" align="center">
        <template slot-scope="scope">{{ DepartmengFormat( scope.row.departmentId )}}</template>
      </el-table-column>

      <el-table-column label="电话" align="center">
        <template slot-scope="scope">{{ scope.row.phoneNumber }}</template>
      </el-table-column>

      <el-table-column label="薪资" width="80" align="center">
        <template slot-scope="scope">{{ scope.row.salary }}</template>
      </el-table-column>

      <el-table-column label="地址" min-width="166px" align="center">
        <template slot-scope="scope">{{ scope.row.address }}</template>
      </el-table-column>

      <el-table-column label="入职日期" align="center" width="200">
        <el-date-picker
          v-model="scope.row.joinTime"
          slot-scope="scope"
          value-format="yyyy-MM-dd"
          type="date"
          placeholder="暂无数据"
          style="width: 80%;"
          :disabled="true"
        />
      </el-table-column>

      <el-table-column
        align="center"
        label="操作"
        width="80"
        v-permission="['Staff_Set']"
        fixed="right"
      >
        <template slot-scope="scope">
          <el-button
            @click="ChangeUserInfo(scope.row),ChangedialogFormVisible=true"
            type="primary"
            size="small"
            v-permission="['Staff_Set']"
          >编辑</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      :total="total"
      :page.sync="listQuery.page"
      :limit.sync="listQuery.limit"
      @pagination="getList"
    />
  </div>
</template>

<script>
import Pagination from "@/components/Pagination";
import { GetAllDepartments } from "@/api/department";
import {
  getStaffList,
  SetStaffInfo,
  FindColleagueByName,
  GetUserinfo
} from "@/api/StaffInfoManage";
import { getToken } from "@/utils/auth";
import { CheckUserName, AddStaff } from "@/api/user";
import permission from "@/directive/permission/index.js"; // 权限判断指令
const state = {
  token: getToken()
};
export default {
  directives: { permission },
  components: { Pagination },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: "success",
        draft: "info",
        deleted: "danger"
      };
      return statusMap[status];
    }
  },
  data() {
    return {
      UserInfo: {
        userName: "",
        realName: "",
        departmentId: 1,
        phoneNumber: "",
        address: "",
        salary: null,
        birthday: null,
        sex: "1",
        introduction: "",
        joinTime: null,
        photo: ""
      },
      error: {
        nameError: "",
        UploadError: "只能上传Jpeg格式，且<100k"
      },
      Authorization: "Bearer " + state.token,
      PhotoArray: [],
      imageUrl: "",
      ActionUrl: process.env.VUE_APP_BASE_API + "/user/setStaffphoto",
      flag: false,
      /////
      fileList: [],
      options: null,
      list: null,
      listLoading: true,
      total: 0,
      listQuery: {
        page: 1,
        limit: 10
      },
      FindByname: "",
      formLabelWidth: "120px",
      AdddialogFormVisible: false,
      ChangedialogFormVisible: false
    };
  },
  created() {
    this.GetAllDepartments();
    this.getList();
  },
  methods: {
    //编辑员工
    ChangeUserInfo(row) {
      //console.log(row);
      this.UserInfo = row;
      this.UserInfo.sex = row.sex.toString();
      this.UserInfo.phoneNumber == null ? "" : row.phoneNumber * 1;
      if (row.departmentId == 0) {
        this.UserInfo.departmentId = "请选择";
      }
    },
    //上传图片成功
    onSuccess(response) {
      //console.log(response);
      this.$refs.upload.clearFiles();
    },
    isHaveImg(file) {},
    checThiskUserName(value) {
      if (value.length <= 15 && value.length >= 5) {
        CheckUserName({ userName: value.toString() }).then(Response => {
          if (Response.data === 19999) {
            this.error.nameError = Math.random().toString();
            this.$nextTick(() => {
              this.error.nameError = "该用户名已存在";
            });
            this.flag = false;
            return false;
          } else {
            this.flag = true;
          }
        });
      }
    },
    submitUpload() {
      this.$refs.upload.submit();
    },
    checkImg(file, fileList) {
      const isJPG = file.raw.type === "image/jpeg";
      const isLt2M = file.raw.size < 102400;

      if (!isJPG) {
        this.$message.error("上传的照片只能是 JPG 格式!");
        this.$refs.upload.clearFiles();
        this.flag = false;
      } else if (!isLt2M) {
        this.$message.error("上传的照片大小不能超过 100Kb!");
        this.$refs.upload.clearFiles();
        this.flag = false;
      } else {
        this.flag = true;
      }
      return isJPG && isLt2M;
    },
    OnChangeSubmit() {
      this.$refs.ChangeUserInfo.validate(validate => {
        if (validate) {
          SetStaffInfo(this.UserInfo);
        }
      });
    },
    onAddSubmit() {
      //再次检查用户名
      var value = this.UserInfo.userName;
      if (value.length <= 15 && value.length >= 5) {
        CheckUserName({ userName: value }).then(Response => {
          if (Response.data === 19999) {
            this.error.nameError = Math.random().toString();
            this.$nextTick(() => {
              this.error.nameError = "该用户名已存在";
            });
            this.flag = false;
          } else {
            this.flag = true;
          }
        });
      }
      //检查表单
      this.$refs.UserInfo.validate(validate => {
        if (!validate) {
          this.flag = false;
          return false;
        } else {
          this.flag = true;
        }
      });
      //console.log("flag:" + this.flag);
      if (this.flag) {
        AddStaff(this.UserInfo).then(res => {
          if (res.code == 20000) {
            this.getList();
            //此时员工文字信息添加成功 开始尝试添加照片
            //先检查照片
            //检查照片
            this.$refs.upload.submit();
            if (!this.flag) {
              this.error.UploadError = "请选择照片";
            } else {
              this.error.UploadError = "只能上传Jpeg格式，且<100k";
            }
          } else {
            this.$message.error("员工添加失败");
          }
        });
      }
    },
    async getList() {
      this.listLoading = true;
      const { limit, page } = this.listQuery;
      const { data } = await getStaffList({
        limit: limit,
        page: limit * (page - 1)
      });
      this.list = data.list;
      this.total = data.total;
      setTimeout(() => {
        this.listLoading = false;
      }, 250);
    },
    SexFormat(sex) {
      return sex == 1 ? "男" : "女";
    },
    async GetAllDepartments() {
      const { data } = await GetAllDepartments();
      this.options = data;
    },
    DepartmengFormat(departmentId) {
      var name = "暂未分配";
      this.options.forEach(data => {
        if (data.departmentId == departmentId) {
          name = data.departmentName;
        }
      });
      return name;
    },

    handleAvatarSuccess(res, file) {
      this.$message.success("照片更新成功");
      //this.imageUrl = URL.createObjectURL(file.raw);
      this.GetUserInfo();
    },
    FindColleagueByName(name) {
      if (false) {
        this.$message.error("姓名不得为空");
        // return false;
      } else {
        this.listLoading = true;
        const { limit, page } = this.listQuery;
        FindColleagueByName({
          data: name,
          limit: limit,
          page: limit * (page - 1)
        }).then(res => {
          const { data } = res;
          this.list = data.list;
          this.total = data.total;
        });
        setTimeout(() => {
          this.listLoading = false;
        }, 250);
      }
    }
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
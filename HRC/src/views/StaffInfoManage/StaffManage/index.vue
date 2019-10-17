<template>
  <div id="fengye" class="zdy-border">
    <div>
      <div>
        <!-- 添加员工的dialog -->
        <el-dialog title="添加员工" :visible.sync="AdddialogFormVisible" width="70%" top="5vh">
          <!-- 添加员工的表单开始 -->
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
                <el-form-item
                  label="入职日期"
                  :rules="[{required:true,message:'不得留空'}]"
                  prop="joinTime"
                >
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
                <el-form-item
                  label="出生日期"
                  :rules="[{required:true,message:'不得留空'}]"
                  prop="birthday"
                >
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
                  :rules="[{required:true,message:'不能留空'}]"
                >
                  <el-input
                    v-model="UserInfo.phoneNumber"
                    show-word-limit
                    maxlength="11"
                    placeholder="请输入手机"
                    style="width: 80%"
                  >
                    <template slot="prepend">
                      <svg-icon icon-class="phone-fill" />
                    </template>
                  </el-input>
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
              <!-- <el-button type="primary" @click="OnAddSubmit">保存</el-button> -->
            </el-form-item>
          </el-form>
          <!-- 表单结束 -->
          <div slot="footer" class="dialog-footer">
            <el-button @click="AdddialogFormVisible = false">取 消</el-button>
            <el-button type="primary" @click="OnAddSubmit">确 定</el-button>
          </div>
        </el-dialog>
        <!-- 编辑员工的dialog -->
        <el-dialog title="编辑员工" :visible.sync="ChangedialogFormVisible" width="70%" top="5vh">
          <!-- 表单开始 -->
          <el-form ref="ChangeUserInfo" :model="UserInfo" label-width="120px">
            <el-row>
              <el-col :span="12">
                <el-form-item label="照片">
                  <!-- 2019年10月16日 10时22分07秒 编辑照片不写了 -->
                  <div class="demo-image__preview">
                    <el-image style="width: 100px; height: 135px;" :src="imgSrc"></el-image>
                  </div>
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
                <el-form-item
                  label="入职日期"
                  :rules="[{required:true,message:'不得留空'}]"
                  prop="joinTime"
                >
                  <el-date-picker
                    v-model="UserInfo.joinTime"
                    type="date"
                    placeholder="暂无数据"
                    style="width: 80%;"
                    format="yyyy 年 MM 月 dd 日"
                    :default-value="new Date()"
                  />
                </el-form-item>
                <!-- 薪资 -->
                <el-form-item label="薪资" prop="salary" :rules="[{required:true,message:'不得留空'}]">
                  <el-input
                    v-model="UserInfo.salary"
                    placeholder="请输入薪资"
                    style="width: 80%"
                    clearable
                  >
                    <template slot="prepend">￥</template>
                  </el-input>
                </el-form-item>
              </el-col>
            </el-row>

            <el-row>
              <el-col :span="12">
                <!-- 姓名 -->
                <el-form-item label="姓名" prop="realName" :rules="[{required:true,message:'不能留空'}]">
                  <el-input
                    v-model="UserInfo.realName"
                    placeholder="请输入内容"
                    style="width: 80%"
                    clearable
                  ></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <!-- 出生日期 -->
                <el-form-item
                  label="出生日期"
                  :rules="[{required:true,message:'不得留空'}]"
                  prop="birthday"
                >
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
                  :rules="[{required:true,message:'不能留空'}]"
                >
                  <el-input
                    v-model.number="UserInfo.phoneNumber"
                    show-word-limit
                    maxlength="11"
                    placeholder="请输入电话"
                    style="width: 80%"
                    clearable
                  >
                    <template slot="prepend">
                      <svg-icon icon-class="phone-fill" />
                    </template>
                  </el-input>
                </el-form-item>
              </el-col>
            </el-row>

            <el-row>
              <el-col :span="12">
                <!-- 地址 -->
                <el-form-item label="地址" :rules="[{required:true,message:'不能留空'}]" prop="address">
                  <el-input
                    v-model="UserInfo.address"
                    placeholder="请输入内容"
                    style="width: 80%"
                    clearable
                  ></el-input>
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
              <!-- <el-button type="primary" @click="OnAddSubmit">保存</el-button> -->
            </el-form-item>
          </el-form>
          <!-- 表单结束 -->
          <div slot="footer" class="dialog-footer">
            <el-button @click="ChangedialogFormVisible = false">取 消</el-button>
            <el-button type="primary" @click="OnChangeSubmit()">确 定</el-button>
          </div>
        </el-dialog>
      </div>

      <!-- 其它功能选项 -->
      <div class="table-head">
        <!-- 搜索框 -->
        <el-input
          v-permission="['Staff_Find']"
          placeholder="请输入..."
          class="input-with-select"
          clearable
          v-model="OrderPropKeyword.keyword"
          style="width: 350px;"
          @clear="GetList"
        >
          <el-select v-model="OrderPropKeyword.select" slot="prepend" style="width:80px">
            <el-option label="编号" value="id"></el-option>
            <el-option label="姓名" value="realName"></el-option>
            <el-option label="部门" value="departmentId"></el-option>
          </el-select>
          <el-button slot="append" icon="el-icon-search" @click="GetList" @keyup.enter="GetList"></el-button>
        </el-input>
        <!-- 添加员工 -->
        <el-button
          type="primary"
          v-permission="['Staff_Add']"
          @click="AdddialogFormVisible = true"
        >添加员工</el-button>
      </div>
    </div>

    <!-- 员工信息表格 -->
    <div style="margin-top:1vh">
      <el-table
        v-loading="listLoading"
        :data="list"
        element-loading-text="加载中.."
        empty-text="查无此人"
        border
        fit
        highlight-current-row
        :stripe="true"
        height="65vh"
        @sort-change="tableOrder"
      >
        <!-- <el-table-column type="selection" width="45" align="center"></el-table-column> -->

        <el-table-column align="center" label="编号" width="95" prop="id" sortable></el-table-column>

        <el-table-column align="center" label="姓名" width="95">
          <template slot-scope="scope">
            <span class="link-type" @click="ChangeUserInfo(scope.row)">{{ scope.row.realName }}</span>
          </template>
        </el-table-column>

        <el-table-column label="出生日期" align="center" width="200">
          <el-date-picker
            v-model="scope.row.birthday"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :default-value="new Date()"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column label="性别" width="60" align="center">
          <template slot-scope="scope">
            <el-tag
              :type="scope.row.sex === 1 ? 'primary' : 'success'"
            >{{SexFormat (scope.row.sex)}}</el-tag>
          </template>
        </el-table-column>

        <el-table-column
          label="部门"
          align="center"
          :formatter="DepartmengFormat"
          prop="departmentId"
          sortable
        ></el-table-column>

        <el-table-column label="电话" align="center" width="108" prop="phoneNumber"></el-table-column>

        <el-table-column label="薪资" width="80" align="center" prop="salary" sortable>
          <template slot-scope="{row}">
            <span :style="salaryStyle(row.salary)">{{row.salary}}</span>
          </template>
        </el-table-column>

        <el-table-column label="地址" min-width="166px" align="center" prop="address"></el-table-column>

        <el-table-column label="入职日期" align="center" width="200">
          <el-date-picker
            v-model="scope.row.joinTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column
          align="center"
          label="操作"
          width="90"
          v-permission="['Staff_Set']"
          fixed="right"
        >
          <template slot-scope="scope">
            <!-- 编辑按钮 -->
            <el-button @click="ChangeUserInfo(scope.row)" type="text" v-permission="['Staff_Set']">
              <svg-icon icon-class="edit-fill" />
            </el-button>
            <!-- 删除按钮 -->
            <el-button @click="deletedBtn(scope.row)" type="text" v-permission="['Staff_Del']">
              <svg-icon icon-class="delete-fill" />
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <!-- 分页 -->
    <div>
      <pagination
        :total="total"
        :page.sync="listQuery.page"
        :limit.sync="listQuery.limit"
        @pagination="GetList()"
      />
    </div>
  </div>
</template>

<script>
import Pagination from "@/components/Pagination";
import { GetAllDepartments } from "@/api/department";
import {
  SetStaffInfo,
  FindColleagueByName,
  GetUserinfo,
  DeleteAStaff,
  GetStaffPhoto,
  GetStaffListByOrder
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
      ActionUrl: process.env.VUE_APP_BASE_API + "/user/setStaffphoto",
      flag: false,
      imgSrc: "",
      fileList: [],
      options: null,
      list: null,
      listLoading: true,
      //分页相关
      total: 0,
      listQuery: {
        page: 1,
        limit: 10
      },
      FindByname: "",
      formLabelWidth: "120px",
      AdddialogFormVisible: false,
      ChangedialogFormVisible: false,
      OrderPropKeyword: {
        Order: "",
        prop: "",
        Keyword: "",
        select: "realName"
      }
    };
  },
  created() {
    this.GetAllDepartments();
    this.GetList();
  },
  methods: {
    //编辑员工
    async ChangeUserInfo(row) {
      this.ChangedialogFormVisible = true;
      this.UserInfo = row;
      this.UserInfo.sex = row.sex.toString();
      this.UserInfo.phoneNumber == null ? "" : row.phoneNumber * 1;
      if (row.departmentId == 0) {
        this.UserInfo.departmentId = "请选择";
      }
      const { data } = await GetStaffPhoto(row);
      this.imgSrc = data.value;
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
    OnAddSubmit() {
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
            this.GetList();
            //此时员工文字信息添加成功 开始尝试添加照片
            //先检查照片
            //检查照片
            this.$refs.upload.submit();
            if (!this.flag) {
              this.error.UploadError = "请选择照片";
            } else {
              this.error.UploadError = "只能上传Jpg/Jpeg格式，且<100k";
            }
          } else {
            this.$message.error("员工添加失败");
          }
        });
      }
    },
    async GetList() {
      this.listLoading = true;
      const { limit, page } = this.listQuery;
      const { data } = await GetStaffListByOrder({
        limit: limit,
        page: limit * (page - 1),
        data: this.OrderPropKeyword
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
    DepartmengFormat(row, column) {
      var name = "暂未分配";
      this.options.forEach(data => {
        if (data.departmentId == row.departmentId) {
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
    FindColleagueByName() {
      if (false) {
        this.$message.error("姓名不得为空");
        // return false;
      } else {
        this.listLoading = true;
        const { limit, page } = this.listQuery;
        FindColleagueByName({
          data: this.keyword,
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
    },
    //删除一个员工
    deletedBtn(row) {
      this.$confirm(`确认删除编号为\"${row.id}\"的员工？`, "提示", {
        type: "warning"
      })
        .then(_ => {
          DeleteAStaff(row);
          this.GetList();
        })
        .catch(_ => {});
    },
    //给工资添加样式
    salaryStyle(salary) {
      if (salary < 5000) {
        return "color:blue";
      } else if (salary >= 5000 && salary < 10000) {
        return "color:orange";
      } else if (salary >= 10000) {
        return "color:red";
      }
    },
    //表格排序事件
    async tableOrder({ prop, order }) {
      this.OrderPropKeyword = {
        prop,
        order,
        keyword: this.OrderPropKeyword.keyword,
        select: this.OrderPropKeyword.select
      };
      this.GetList();
    }
  }
};
</script>


<style>
.input-with-select .el-input-group__prepend {
  background-color: #fff;
}

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
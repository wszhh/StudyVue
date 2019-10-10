<template>
  <div id="DepartmentManage " class="zdy-border">
    <div id="ControBtn" class="Padding">
      <el-button v-permission="['Department_Add']" type="primary" @click="open">增加部门</el-button>
      <el-tooltip class="item" effect="dark" content="没写" placement="bottom">
        <el-button v-permission="['Department_Del']" type="danger">删除选中</el-button>
      </el-tooltip>
    </div>
    <div>
      <el-table
        v-loading="listLoading"
        :data="list"
        element-loading-text="加载中.."
        border
        fit
        highlight-current-row
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="50" align="center"></el-table-column>
        <el-table-column align="center" label="ID" width="95">
          <template slot-scope="scope">{{ scope.row.departmentId}}</template>
        </el-table-column>

        <el-table-column label="部门" align="center">
          <template slot-scope="{row}">
            <template v-if="row.edit">
              <el-input
                v-model="row.departmentName"
                class="edit-input"
                style="width:80%"
                size="small"
              />
              <el-button
                class="cancel-btn"
                size="small"
                icon="el-icon-refresh"
                type="warning"
                @click="cancelEdit(row)"
              >取消</el-button>
            </template>
            <span v-else>{{ row.departmentName }}</span>
          </template>
        </el-table-column>

        <el-table-column
          align="center"
          label="选项"
          width="190"
          fixed="right"
          v-if="checkPermission(['Department_Del','Department_Set'])"
        >
          <template slot-scope="{row}">
            <el-button
              v-if="row.edit"
              type="success"
              size="small"
              icon="el-icon-circle-check-outline"
              @click="confirmEdit(row)"
            >确定</el-button>

            <el-button
              v-permission="['Department_Set']"
              v-else
              type="primary"
              size="small"
              icon="el-icon-edit"
              @click="row.edit=!row.edit"
            >编辑</el-button>

            <el-button
              v-permission="['Department_Del']"
              slot="reference"
              type="danger"
              size="small"
              icon="el-icon-delete"
              @click="deletedBtn(row)"
            >删除</el-button>
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
    <el-dialog title="提示" :visible.sync="dialogVisible" width="30%">
      <span>
        确定要删除"
        <span style="color:#F56C6C">{{dialogMsg}}</span>"吗?
      </span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="deleteDepartment(),dialogVisible = false">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import Pagination from "@/components/Pagination";
import {
  getDepartmentList,
  deleteADepartment,
  addDepartment,
  editDepartment
} from "@/api/department";
import checkPermission from "@/utils/permission"; // 权限判断函数
export default {
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
      rowdata: null,
      dialogMsg: "",
      dialogVisible: false,
      list: null,
      listLoading: true,
      total: 0,
      listQuery: {
        page: 1,
        limit: 10
      }
    };
  },
  created() {
    this.getList();
  },
  methods: {
    checkPermission,
    async getList() {
      this.listLoading = true;
      const { limit, page } = this.listQuery;
      const { data } = await getDepartmentList({
        limit: limit,
        page: limit * (page - 1)
      });
      const items = data.list;
      this.total = data.total;
      this.list = items.map(v => {
        this.$set(v, "edit", false);
        v.originaldepartmentId = v.departmentId;
        return v;
      });
      setTimeout(() => {
        this.listLoading = false;
      }, 250);
    },
    cancelEdit(row) {
      row.departmentId = row.originaldepartmentId;
      row.edit = false;
      this.$message({
        message: "未更改",
        type: "warning"
      });
    },
    confirmEdit(row) {
      row.edit = false;
      row.originaldepartmentId = row.departmentId;
      editDepartment(row).then(respone => {
        var code = respone.code;
        var value = respone.data.departmentName;
        if (code == 20000) {
          //   this.$message({
          //     message: "已更改为" + value + "",
          //     type: "success"
          //   });
          this.getList();
        }
      });
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
    },
    open() {
      this.$prompt("请输入部门名称", "添加一个部门", {
        confirmButtonText: "提交",
        cancelButtonText: "取消",
        inputValidator: function(value) {
          if (value == null) {
            return "不能为空";
          }
        },
        inputErrorMessage: "不能为空"
      })
        .then(({ value }) => {
          addDepartment({ DepartmentName: value }).then(res => {
            var code = res.code;
            //var value = res.data.departmentName;
            if (code == 20000) {
              this.getList();
            }
          });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "取消添加"
          });
        });
    },
    deletedBtn(row) {
      this.dialogVisible = true;
      this.dialogMsg = row.departmentName;
      this.rowdata = row;
    },
    deleteDepartment() {
      var row = this.rowdata;
      delete row.edit;
      deleteADepartment(row).then(res => {
        var code = res.code;
        var value = res.data.departmentName;
        if (code == 20000) {
          //   this.$message({
          //     type: "success",
          //     message: "你所删除的部门是: " + value
          //   });
          this.getList();
        }
      });
    }
  }
};
</script>

<style>
</style>
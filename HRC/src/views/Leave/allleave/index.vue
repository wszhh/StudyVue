<template>
  <div class="zdy-border">
    <div id="head" class="table-head">
      <el-button type="primary" @click="ApplyFormVisible=false">XXXXX</el-button>
      <div id="dialog">
        <!-- 申请请假的dialog -->
        <el-dialog title="申请请假" :visible.sync="ApplyFormVisible">
          <el-form :model="ApplyForm" ref="ApplyForm">
            <el-row>
              <el-col :span="12">
                <el-form-item
                  label="开始时间"
                  prop="LeaveStartTime"
                  :rules="[{required:true,message:'不得留空'}]"
                >
                  <el-date-picker
                    v-model="ApplyForm.LeaveStartTime"
                    type="date"
                    placeholder="开始时间"
                    style="width: 80%;"
                    format="yyyy 年 M 月 d 日"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item
                  label="结束时间"
                  prop="LeaveEndTime"
                  :rules="[{required:true,message:'不得留空'}]"
                >
                  <el-date-picker
                    v-model="ApplyForm.LeaveEndTime"
                    type="date"
                    placeholder="结束时间"
                    style="width: 80%;"
                    format="yyyy 年 M 月 d 日"
                  />
                </el-form-item>
              </el-col>
            </el-row>
            <el-form-item label="时间段" prop="LeaveHalfDay">
              <el-select v-model="ApplyForm.LeaveHalfDay">
                <el-option label="全天" value="全天"></el-option>
                <el-option label="上午" value="上午"></el-option>
                <el-option label="下午" value="下午"></el-option>
              </el-select>
            </el-form-item>
            <el-form-item label="请假原因" prop="LeaveReason" :rules="[{required:true,message:'不得留空'}]">
              <el-input
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 4}"
                placeholder="请输入内容"
                v-model="ApplyForm.LeaveReason"
              ></el-input>
            </el-form-item>
          </el-form>
          <div slot="footer" class="dialog-footer">
            <el-button @click="ApplyFormVisible = false">取 消</el-button>
            <el-button type="primary" @click="ApplyFormSubmit">确 定</el-button>
          </div>
        </el-dialog>
      </div>
    </div>
    <div id="table">
      <el-table
        v-loading="listLoading"
        :data="list"
        element-loading-text="Loading"
        border
        fit
        height="65vh"
        highlight-current-row
      >
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-form label-position="left" inline class="demo-table-expand">
              <el-form-item label="申请人编号" align="center">
                <span>{{ props.row.userId }}</span>
              </el-form-item>
              <br />

              <el-form-item label="请假号" align="center">
                <span>{{ props.row.leaveId }}</span>
              </el-form-item>
              <br />

              <el-form-item label="请假天数" align="center">
                <span>{{ props.row.leaveDays }}</span>
              </el-form-item>
              <br />

              <el-form-item label="审批人编号" align="center">
                <span>{{ props.row.approverId }}</span>
              </el-form-item>
              <br />

              <el-form-item label="备注" align="center" prop="approverReason">
                <span>{{ props.row.approverReason }}</span>
              </el-form-item>
            </el-form>
          </template>
        </el-table-column>

       <el-table-column label="姓名" width="100" align="center" prop="realName"></el-table-column>

        <el-table-column label="审批状态" width="90" align="center">
          <template slot-scope="{row}">
            <el-tag :type="tagType(row.leaveState)">{{stateFormat(row.leaveState)}}</el-tag>
          </template>
        </el-table-column>

        <el-table-column label="申请时间" align="center" width="190">
          <el-date-picker
            v-model="scope.row.leaveTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column label="起始时间" align="center" width="190">
          <el-date-picker
            v-model="scope.row.leaveStartTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column label="结束时间" align="center" width="190">
          <el-date-picker
            v-model="scope.row.leaveEndTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column label="时间段" align="center" width="70">
          <template slot-scope="scope">{{ scope.row.leaveHalfDay }}</template>
        </el-table-column>
        <el-table-column label="请假原因" align="center">
          <template slot-scope="scope">{{ scope.row.leaveReason }}</template>
        </el-table-column>

        <el-table-column label="审批时间" align="center" width="190">
          <el-date-picker
            v-model="scope.row.approvalTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>
      </el-table>
    </div>
    <div id="pagination">
      <pagination
        :total="total"
        :page.sync="listQuery.page"
        :limit.sync="listQuery.limit"
        @pagination="fetchData()"
      />
    </div>
  </div>
</template>

<script>
import {
  GetLeaves,
  GetLeaveStartCategory,
  FormatLeaveType,
  AddLeave
} from "@/api/Leave";
import Pagination from "@/components/Pagination";

export default {
  components: { Pagination },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: "success",
        draft: "gray",
        deleted: "danger"
      };
      return statusMap[status];
    }
  },
  data() {
    return {
      list: null,
      listLoading: true,
      //分页相关
      total: 0,
      listQuery: {
        page: 1,
        limit: 10
      },
      categoryList: null,
      ApplyFormVisible: false,
      ApplyForm: {
        LeaveStartTime: null,
        LeaveEndTime: null,
        LeaveHalfDay: "全天",
        LeaveReason: null,
        LeaveTime: new Date()
      }
    };
  },
  created() {
    this.GetLeaveStartCategory();
    this.fetchData();
  },
  methods: {
    fetchData() {
      this.listLoading = true;
      const { limit, page } = this.listQuery;
      GetLeaves({
        limit: limit,
        page: limit * (page - 1)
      }).then(response => {
        this.list = response.data.list;
        this.total = response.data.total;
        this.listLoading = false;
      });
    },
    async GetLeaveStartCategory() {
      const { data } = await GetLeaveStartCategory();
      this.categoryList = data;
    },
    stateFormat(leaveState) {
      var text = "暂无";
      if (this.categoryList != null) {
        this.categoryList.forEach(item => {
          if (leaveState == item.ciId) {
            text = item.ciName;
          }
        });
      }
      return text;
    },
    tagType(value) {
      return FormatLeaveType(value);
    },
    ApplyFormSubmit() {
      this.$refs.ApplyForm.validate(validate => {
        if (validate) {
          AddLeave(this.ApplyForm);
        } else {
          return false;
        }
      });
    }
  }
};
</script>


<style>
.demo-table-expand {
  font-size: 0;
}
.demo-table-expand label {
  width: 90px;
  color: #99a9bf;
}

.demo-table-expand .el-form-item {
  margin-right: 0;
  margin-bottom: 0;
  /* width: 50%; */
}
</style>
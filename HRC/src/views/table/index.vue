<template>
  <div class="app-container ">
    <el-table
      v-loading="listLoading"
      :data="list"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row
    >
      <el-table-column align="center" label="请假号" width="95">
        <template slot-scope="scope">{{ scope.row.leaveId}}</template>
      </el-table-column>
      <el-table-column label="用户编号" width="95">
        <template slot-scope="scope">{{ scope.row.userId }}</template>
      </el-table-column>
      <el-table-column label="审批状态" width="110" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.leaveState }}</span>
        </template>
      </el-table-column>
      <el-table-column label="请假时间" width="200" align="center">
        <template slot-scope="scope">{{ scope.row.leaveTime }}</template>
      </el-table-column>
      <el-table-column label="请假起始时间" width="200" align="center">
        <template slot-scope="scope">{{ scope.row.leaveStartTime }}</template>
      </el-table-column>
      <el-table-column label="结束时间" width="200" align="center">
        <template slot-scope="scope">{{ scope.row.leaveEndTime }}</template>
      </el-table-column>
      <el-table-column label="时间段" align="center">
        <template slot-scope="scope">{{ scope.row.leaveHalfDay }}</template>
      </el-table-column>
      <el-table-column label="请假天数" align="center">
        <template slot-scope="scope">{{ scope.row.leaveDays }}</template>
      </el-table-column>
      <el-table-column label="请假原因" align="center">
        <template slot-scope="scope">{{ scope.row.leaveReason }}</template>
      </el-table-column>
      <el-table-column label="审批人编号" align="center">
        <template slot-scope="scope">{{ scope.row.approverID }}</template>
      </el-table-column>
      <el-table-column label="审批时间" width="200" align="center">
        <template slot-scope="scope">{{ scope.row.approvalTime }}</template>
      </el-table-column>
      <el-table-column label="审批备注" width="200" align="center">
        <template slot-scope="scope">{{ scope.row.approverReason }}</template>
      </el-table-column>
      <!-- <el-table-column class-name="status-col" label="Status" width="110" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.status | statusFilter">{{ scope.row.status }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="Display_time" width="200">
        <template slot-scope="scope">
          <i class="el-icon-time" />
          <span>{{ scope.row.display_time }}</span>
        </template>
      </el-table-column> -->
    </el-table>
  </div>
</template>

<script>
import { getList } from "@/api/table";

export default {
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
      listLoading: true
    };
  },
  created() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      this.listLoading = true;
      getList().then(response => {
        this.list = response.data;
        this.listLoading = false;
      });
    }
  }
};
</script>

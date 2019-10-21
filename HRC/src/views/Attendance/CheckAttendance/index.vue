<template>
  <div class="zdy-border">
    <div>
      <el-table
        v-loading="listLoading"
        :data="list"
        element-loading-text="加载中.."
        border
        fit
        highlight-current-row
        :stripe="true"
        height="65vh"
      >
        <el-table-column label="考勤时间" align="center" width="200">
          <el-date-picker
            v-model="scope.row.attendanceStartTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column label="姓名" align="center" prop="realName"></el-table-column>

        <el-table-column label="签到时间" align="center" width="200">
          <el-date-picker
            v-model="scope.row.clockTime"
            size="small"
            slot-scope="scope"
            value-format="yyyy-M-d"
            type="date"
            placeholder="暂无数据"
            style="width: 80%;"
            :disabled="true"
          />
        </el-table-column>

        <el-table-column label="备注" align="center" prop="remark"></el-table-column>

        <el-table-column label="考勤状态" align="center" prop="attendanceType">
          <template slot-scope="{row}">
            <el-tag
              :type="TagType(SignFormat(row.attendanceType))"
            >{{ SignFormat( row.attendanceType)}}</el-tag>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div>
      <pagination
        :total="total"
        :page.sync="listQuery.page"
        :limit.sync="listQuery.limit"
        @pagination="GetAttendances"
      />
    </div>
  </div>
</template>
<script>
import Pagination from "@/components/Pagination";
import {
  GetAttendances,
  FormatAttendanceType,
  GetCategory
} from "@/api/Attendance";
export default {
  components: { Pagination },
  data() {
    return {
      listLoading: false,
      list: null,
      Category: null,
      listQuery: {
        page: 1,
        limit: 10
      },
      total: 0
    };
  },
  methods: {
    async GetAttendances() {
      const { data } = await GetAttendances();
      this.list = data;
    },
    async GetCategory() {
      const { data } = await GetCategory();
      this.Category = data;
    },
    SignFormat(data) {
      if (this.Category != null) {
        var result = "";
        this.Category.forEach(item => {
          if (data == item.ciId) {
            result = item.ciName;
          }
        });
        return result;
      }
    },
    TagType(value) {
      return FormatAttendanceType(value);
    }
  },
  created() {
    this.GetCategory();
    this.GetAttendances();
  }
};
</script>

<style>
</style>
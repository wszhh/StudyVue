<template>
  <div id="fengye" class="zdy-border">
    <el-table
      v-loading="listLoading"
      :data="list"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row
    >
      <el-table-column align="center" label="报名号" width="95">
        <template slot-scope="scope">{{ scope.row.bmh}}</template>
      </el-table-column>

      <el-table-column label="姓名" align="center" width="95">
        <template slot-scope="scope">{{ scope.row.xm }}</template>
      </el-table-column>

      <el-table-column label="身份证号" width="200" align="center">
        <template slot-scope="scope">{{ scope.row.sfzh }}</template>
      </el-table-column>

      <el-table-column label="民族" align="center">
        <template slot-scope="scope">{{ scope.row.mzmc }}</template>
      </el-table-column>

      <el-table-column label="户籍所在地" min-width="250" align="center">
        <template slot-scope="scope">{{ scope.row.hjszd }}</template>
      </el-table-column>

      <el-table-column label="加分分数" align="center">
        <template slot-scope="scope">{{ scope.row.score }}</template>
      </el-table-column>

      <el-table-column label="dqdm" align="center">
        <template slot-scope="{row}">
          <template v-if="row.edit">
            <el-input v-model="row.dqdm" type="number" class="edit-input" size="small" />
            <el-button
              class="cancel-btn"
              size="small"
              icon="el-icon-refresh"
              type="warning"
              @click="cancelEdit(row)"
            >取消</el-button>
          </template>
          <span v-else>{{ row.dqdm }}</span>
        </template>
      </el-table-column>

      <el-table-column align="center" label="选项" width="120" fixed="right">
        <template slot-scope="{row}">
          <el-button
            v-if="row.edit"
            type="success"
            size="small"
            icon="el-icon-circle-check-outline"
            @click="confirmEdit(row)"
          >确定</el-button>
          <el-button
            v-else
            type="primary"
            size="small"
            icon="el-icon-edit"
            @click="row.edit=!row.edit"
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
import { getStuList } from "@/api/table";
import { editStuList } from "@/api/table";

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
    async getList() {
      this.listLoading = true;
      const { limit, page } = this.listQuery;
      const { data } = await getStuList({
        limit: limit,
        page: limit * (page - 1)
      });
      const items = data.list;
      this.total = data.total;
      this.list = items.map(v => {
        this.$set(v, "edit", false); // https://vuejs.org/v2/guide/reactivity.html
        v.originaldqdm = v.dqdm; //  will be used when user click the cancel botton
        return v;
      });
      ///
      setTimeout(() => {
        this.listLoading = false;
      }, 250);
    },
    cancelEdit(row) {
      row.dqdm = row.originaldqdm;
      row.edit = false;
      this.$message({
        message: "未更改",
        type: "warning"
      });
    },
    confirmEdit(row) {
      row.edit = false;
      row.originaldqdm = row.dqdm;
      editStuList(row).then(respone => respone);
      this.$message({
        message: "三天之内杀了你",
        type: "success"
      });
    }
  }
};
</script>
<template>
  <div id="fengye" class="zdy-border">
    <!-- 顶端搜索 -->
    <div class="table-head">
      <el-input
        placeholder="请输入同事姓名"
        v-model="FindByname"
        class="input-with-select"
        clearable
        v-permission="['Colleague_Find']"
        style="width:220"
      >
        <!-- <el-select v-model="select" slot="prepend" placeholder="请选择">
          <el-option label="餐厅名" value="1"></el-option>
          <el-option label="订单号" value="2"></el-option>
          <el-option label="用户电话" value="3"></el-option>
        </el-select>-->
        <el-button
          v-permission="['Colleague_Find']"
          slot="append"
          icon="el-icon-search"
          @click="FindColleagueByName(FindByname)"
          @keyup.enter="FindColleagueByName(FindByname)"
        ></el-button>
      </el-input>
    </div>
    <!-- 同事信息表格 -->
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
    >
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
          :disabled="true"
          size="small"
        />
      </el-table-column>

      <el-table-column label="性别" width="100" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.sex === 1 ? 'primary' : 'success'">{{SexFormat(scope.row.sex)}}</el-tag>
        </template>
      </el-table-column>

      <el-table-column label="电话" align="center">
        <template slot-scope="scope">{{ scope.row.phoneNumber }}</template>
      </el-table-column>

      <el-table-column label="工资" align="center">
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
          size="small"
        />
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
import { getColleaguesList, FindColleagueByName } from "@/api/StaffInfoManage";
import permission from "@/directive/permission/index.js"; // 权限判断指令

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
      list: null,
      listLoading: true,
      total: 0,
      listQuery: {
        page: 1,
        limit: 10
      },
      FindByname: ""
    };
  },
  created() {
    this.getList();
  },
  methods: {
    async getList() {
      this.listLoading = true;
      const { limit, page } = this.listQuery;
      const { data } = await getColleaguesList({
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
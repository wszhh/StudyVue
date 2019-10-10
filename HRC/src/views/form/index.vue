<template>
  <div class="zdy-border">
    <!-- 按钮区 -->
    <el-dialog title="收货地址" :visible.sync="RoledialogTableVisible">
      <el-table :data="gridData">
        <el-table-column property="date" label="日期" width="150"></el-table-column>
        <el-table-column property="name" label="姓名" width="200"></el-table-column>
        <el-table-column property="address" label="地址"></el-table-column>
      </el-table>
    </el-dialog>
    <!-- 编辑Claim的dialog -->
    <el-dialog :visible.sync="ClaimdialogVisible" width="30%" top="2vh" :title="RoleName">
      <el-tree :data="Tree" show-checkbox node-key="id" ref="tree"></el-tree>
      <span slot="footer" class="dialog-footer" template>
        <el-button @click="ClaimdialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="ClaimdialogVisible = true,getCheckedNodes()">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 角色表 -->
    <el-table :data="tableData" border style="width: 100%">
      <el-table-column prop="name" label="角色" align="center"></el-table-column>
      <el-table-column
        fixed="right"
        label="操作"
        v-if="checkPermission(['Claim_Del','Claim_Set','Claim_SetRole'])"
      >
        <template slot-scope="{row}">
          <el-button
            v-permission="['Claim_SetRole']"
            type="primary"
            @click="GetUsersInRole(row),RoledialogTableVisible=true"
            size="small"
          >分配角色</el-button>
          <el-button
            v-permission="['Claim_Set']"
            @click="GetClaimTree(row),ClaimdialogVisible = true"
            type="primary"
            size="small"
          >编辑权限</el-button>
          <el-button v-permission="['Claim_Del']" type="danger" size="small">删 除</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import {
  GetClaimTree,
  GetRoles,
  SetRoleClaim,
  GetUsersInRole
} from "@/api/Claim";

import checkPermission from "@/utils/permission"; // 权限判断函数

export default {
  data() {
    return {
      Tree: [],
      tableData: [],
      ClaimdialogVisible: false,
      RoledialogTableVisible: false,
      Claims: [],
      RoleName: "Null",
      gridData: null
    };
  },
  created() {
    this.getTableData();
  },
  methods: {
    checkPermission,
    async GetClaimTree(row) {
      const { data } = await GetClaimTree(row);
      this.Tree = data.claimTree;
      this.RoleName = `编辑\"${row.name}\"的权限`;
      this.$refs.tree.setCheckedKeys(data.result);
    },
    async getCheckedNodes() {
      const { data } = await SetRoleClaim(this.$refs.tree.getCheckedNodes());
    },
    async getTableData() {
      const { data } = await GetRoles();
      this.tableData = data;
    },
    async GetUsersInRole(row) {
      const { data } = await GetUsersInRole(row);
    }
  }
};
</script>

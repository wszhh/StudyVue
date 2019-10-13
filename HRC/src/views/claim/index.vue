<template>
  <div class="zdy-border">
    <!-- 分配角色的dialog -->
    <el-dialog :title="RoleName" :visible.sync="RoledialogTableVisible" width="75vh">
      <el-transfer
        filterable
        filter-placeholder="请输入用户名"
        v-model="Transfer.value"
        :data="Transfer.data"
        :titles="['未分配/Staff权限','已分配本权限']"
        @change="TransferChange"
      >
        <!-- <el-button class="transfer-footer" slot="left-footer" size="small">操作</el-button> -->
      </el-transfer>
      <!-- <el-table :data="gridData">
        <el-table-column property="id" label="ID"></el-table-column>
        <el-table-column property="name" label="姓名"></el-table-column>
      </el-table>-->
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
  GetUsersInRole,
  SetRoleUsers
} from "@/api/Claim";

import checkPermission from "@/utils/permission"; // 权限判断函数

export default {
  name: "claim",
  data() {
    return {
      Transfer: {
        data: [],
        value: []
      },
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
    TransferChange(value, direction, movedKeys) {
      //console.log(value);
      console.log(direction);
      console.log(movedKeys);
      SetRoleUsers({ direction: direction, movedKeys: movedKeys });
    },
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
      this.RoleName = `分配\"${row.name}\"的角色`;
      this.Transfer = await GetUsersInRole(row);
      // this.Transfer = data;
      // this.value = data.value;
    }
  }
};
</script>

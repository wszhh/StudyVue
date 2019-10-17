<template>
  <div class="zdy-border">
    <!-- 分配角色的dialog -->
    <el-dialog :title="RoleName" :visible.sync="RoledialogTableVisible" width="75vh">
      <el-transfer
        filterable
        filter-placeholder="请输入用户名"
        v-model="Transfer.value"
        :data="Transfer.data"
        :titles="TransferTittle"
        @change="TransferChange"
      >
        <el-select
          v-model="Value"
          slot="left-footer"
          placeholder="请选择角色"
          @change="SelectChange(tableData)"
        >
          <el-option v-for="item in tableData" :key="item.id" :label="item.name" :value="item.id"></el-option>
        </el-select>
      </el-transfer>
    </el-dialog>
    <!-- 编辑Claim的dialog -->
    <el-dialog :visible.sync="ClaimdialogVisible" width="30%" top="2vh" :title="RoleName">
      <el-tree :data="Tree" show-checkbox node-key="id" ref="tree"></el-tree>
      <span slot="footer" class="dialog-footer" template>
        <el-button @click="ClaimdialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="ClaimdialogVisible = true,getCheckedNodes()">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 角色表格 -->
    <!-- 暂时就是这么个样式 以后再改 2019年10月15日 21时56分21秒-->
    <el-table
      :data="tableData"
      border
      style="width: 100%"
      fit
      highlight-current-row
      :stripe="true"
      height="87vh"
    >
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
            @click="GetUsersInRolee(row),RoledialogTableVisible=true"
            size="small"
          >分配角色</el-button>
          <el-button
            v-permission="['Claim_Set']"
            @click="GetClaimTree(row),ClaimdialogVisible = true"
            type="primary"
            size="small"
          >编辑声明</el-button>
          <el-button
            v-permission="['Claim_Del']"
            type="danger"
            size="small"
            :disabled="IsDisabled(row)"
            @click="deletedBtn(row)"
          >删 除</el-button>
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
import { type } from "os";

export default {
  name: "claim",
  data() {
    return {
      Transfer: {
        data: [],
        value: []
      },
      TransferTittle: [],
      Tree: [],
      tableData: [],
      ClaimdialogVisible: false,
      RoledialogTableVisible: false,
      deletedialogVisible: false,
      Claims: [],
      RoleName: "Null",
      gridData: null,
      NewTransferRole: null,
      dialogMsg: "Null",
      RightIdentityRole: {
        id: "",
        name: ""
      },
      LeftIdentityRole: {
        id: "",
        name: ""
      },
      Value: ""
    };
  },
  created() {
    this.getTableData();
  },
  methods: {
    //分配角色
    TransferChange(value, direction, movedKeys) {
      SetRoleUsers({
        direction: direction,
        movedKeys: movedKeys,
        Roles: {
          RightIdentityRole: this.RightIdentityRole,
          LeftIdentityRole: this.LeftIdentityRole
        }
      });
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
    async GetUsersInRolee(row, LeftRow) {
      this.RoleName = `分配角色`;
      this.TransferTittle = [
        `${LeftRow == null ? "Staff" : LeftRow.name}`,
        `${row.name}`
      ];
      this.Transfer = await GetUsersInRole({
        RightIdentityRole: row,
        LeftIdentityRole: LeftRow
      });
      this.RightIdentityRole = row;
    },
    IsDisabled(row) {
      return row.name.toString() == "Staff" ? true : false;
    },
    SelectChange(tableData) {
      this.tableData.forEach(item => {
        if (item.id == this.Value) {
          this.LeftIdentityRole = item;
        }
      });
      this.GetUsersInRolee(this.RightIdentityRole, this.LeftIdentityRole);
    },
    deletedBtn(row) {
      this.$confirm(`确认删除\"${row.name}\"？`, { type: "warning" })
        .then(_ => {
          this.$message({
            message: `删除\"${row.name}\"成功`,
            type: "success"
          });
        })
        .catch(_ => {});
    }
  }
};
</script>

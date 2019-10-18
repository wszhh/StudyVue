<template>
  <div class="zdy-border">
    <el-row>
      <el-col :span="14">
        <div>
          <el-calendar>
            <template slot="dateCell" slot-scope="{date, data}">
              <span
                :class="data.isSelected ? 'is-selected' : ''"
              >{{ data.day.split('-').slice(1).join('-') }}</span>
              <br />
              <el-tag v-if="SignFormat(date, data)!=''">{{SignFormat(date, data)}}</el-tag>
            </template>
          </el-calendar>
        </div>
      </el-col>
      <el-col :span="10" class="table-head">
        <el-button type="primary" style="height:10vh;width:100%">
          <span style="font-size:350%">签&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;到</span>
        </el-button>
        <el-collapse v-model="activeName">
          <el-collapse-item title="上班时间" name="1">
            <div>随便瞎写 我也不知道</div>
            <div>随便瞎写 我也不知道</div>
          </el-collapse-item>
          <el-collapse-item title="显示范围" name="2">
            <div>只显示当前月的信息</div>
          </el-collapse-item>
          <el-collapse-item title="效率 Efficiency" name="3">
            <div>简化流程：设计简洁直观的操作流程；</div>
            <div>清晰明确：语言表达清晰且表意明确，让用户快速理解进而作出决策；</div>
            <div>帮助用户识别：界面简单直白，让用户快速识别而非回忆，减少用户记忆负担。</div>
          </el-collapse-item>
          <el-collapse-item title="可控 Controllability" name="4">
            <div>用户决策：根据场景可给予用户操作建议或安全提示，但不能代替用户进行决策；</div>
            <div>结果可控：用户可以自由的进行操作，包括撤销、回退和终止当前操作等。</div>
          </el-collapse-item>
        </el-collapse>
      </el-col>
    </el-row>
  </div>
</template>


<script>
import { GetSignInInfo, GetCategory } from "@/api/Attendance";
export default {
  data() {
    return {
      SignInInfo: null,
      activeName: "1",
      Category: null
    };
  },
  created() {
    this.GetSignInInfo();
    this.GetCategory();
  },
  methods: {
    async GetSignInInfo() {
      const { data } = await GetSignInInfo();
      this.SignInInfo = data;
    },
    async GetCategory() {
      const { data } = await GetCategory();
      this.Category = data;
    },
    SignFormat(date, data) {
      if (this.SignInInfo != null) {
        var result = 0;
        this.SignInInfo.forEach(item => {
          // console.log(data.day + "___" + item.date.substring(0,10));
          if (data.day == item.date.substring(0, 10)) {
            result = item.signInType;
          }
        });
        this.Category.forEach(item => {
          if (result == item.ciId) {
            result = item.ciName;
          }
        });
        return result;
      }
    }
  }
};
</script>
<style>
.is-selected {
  color: #1989fa;
}
</style>
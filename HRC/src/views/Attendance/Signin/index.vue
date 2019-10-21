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
              <el-tag
                :type="TagType(SignFormat(date, data))"
                v-if="SignFormat(date, data)!=''"
              >{{SignFormat(date, data)}}</el-tag>
            </template>
          </el-calendar>
        </div>
      </el-col>
      <el-col :span="10" class="table-head">
        <el-button
          type="primary"
          style="height:10vh;width:100%"
          @click="Checkin"
          :disabled="!BtnChecked"
        >
          <span style="font-size:350%">签&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;到</span>
        </el-button>
        <el-collapse v-model="activeName">
          <el-collapse-item title="正常签到截止时间" name="1">
            <div>上午9:00</div>
          </el-collapse-item>
          <el-collapse-item title="标签解释" name="2">
            <div>
              <el-tag type="success">正常</el-tag>今日签到情况正常
            </div>
            <div>
              <el-tag type="danger">未签到</el-tag>没有进行签到，可能是放假、请假、缺勤
            </div>
          </el-collapse-item>
          <el-collapse-item title="数据范围" name="3">
            <div>只显示当前月的信息</div>
          </el-collapse-item>
        </el-collapse>
      </el-col>
    </el-row>
  </div>
</template>


<script>
import {
  GetSignInInfo,
  GetCategory,
  IsChecked,
  Checkin,
  FormatAttendanceType
} from "@/api/Attendance";
export default {
  data() {
    return {
      SignInInfo: null,
      activeName: "1",
      Category: null,
      BtnChecked: true
    };
  },
  created() {
    this.GetCategory();
    this.GetSignInInfo();
    //this.IsChecked();
  },
  methods: {
    IsChecked() {
      const { data } = IsChecked();
      this.BtnChecked = data;
    },
    async Checkin() {
      await Checkin();
      this.GetSignInInfo();
    },
    TagType(value) {
      return FormatAttendanceType(value);
    },
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
          var date = item.date.substring(0, 10);
          if (data.day == date) {
            result = item.signInType;
          }
        });
        if (this.Category != null) {
          this.Category.forEach(item => {
            if (result == item.ciId) {
              result = item.ciName;
            }
          });
        }
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
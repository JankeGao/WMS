<template>
  <div class="app-container">
    <el-tabs v-model="activeName" type="card" @tab-click="handleClick">
      <el-tab-pane label="垂直行程学习" name="first">
        <el-steps :active="firstActive" finish-status="success">
          <el-step title="步骤 1" description="开始垂直行程学习" />
          <el-step title="步骤 2" description="监视M340 on表示学习结束,并弹框显示前部托架数和后部托架数" />
          <el-step title="步骤 3" description="确认垂直行程学习结束" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="firstNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="水平行程学习" name="second">
        <el-steps :active="secondActive" finish-status="success">
          <el-step title="步骤 1" description="开始水平行程学习" />
          <el-step title="步骤 2" description="监视水平学习状态" />
          <el-step title="步骤 3" description="确认水平行程学习结束" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="secondNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="自动门行程学习" name="third">
        <el-steps :active="thirdActive" finish-status="success">
          <el-step title="步骤 1" description="开始自动门行程学习" />
          <el-step title="步骤 2" description="监视自动门学习状态" />
          <el-step title="步骤 3" description="确认自动门行程学习结束" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="thirdNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="托盘扫描" name="fourth">
        <el-steps :active="fourthActive" finish-status="success">
          <el-step title="步骤 1" description="开始托盘扫描" />
          <el-step title="步骤 2" description="监视托盘扫描状态,当扫描结束时,显示前后部托盘数" />
          <el-step title="步骤 3" description="开始定义" />
          <el-step title="步骤 4" description="监视托盘到达位标示位M392 ON后弹出对话框,输入托盘号,点击确认后对话框关闭" />
          <el-step title="步骤 5" description="继续定义下一个" />
          <el-step title="步骤 6" description="监视M395,ON表示全部完成,OFF 后回到第四步" />
          <el-step title="步骤 7" description="确认全部托盘定义完成" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="fourthNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="自动存取托盘" name="five">
        <el-steps :active="fiveActive" finish-status="success">
          <el-step title="步骤 1" description="往D650写入托盘序号" />
          <el-step title="步骤 2" description="查询M650 在D651中显示托盘所在托架号" />
          <el-step title="步骤 3" description="启动M651" />
          <el-step title="步骤 4" description="在D652中显示物料高度" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="fiveNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="添加托盘" name="six">
        <el-steps :active="sixActive" finish-status="success">
          <el-step title="步骤 1" description="往D700写入需要添加的托盘序号" />
          <el-step title="步骤 2" description="按钮M700开始执行" />
          <el-step title="步骤 3" description="扫描物料高度M701,OFF空间不足,ON空间足够" />
          <el-step title="步骤 4" description="确认添加M702" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="sixNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="删除托盘" name="seven">
        <el-steps :active="sevenActive" finish-status="success">
          <el-step title="步骤 1" description="往D750写入需要删除的托盘序号" />
          <el-step title="步骤 2" description="按钮M750开始执行" />
          <el-step title="步骤 3" description="在D751中显示托盘所在托架号" />
          <el-step title="步骤 4" description="开始运行设备M751" />
          <el-step title="步骤 5" description="监视托盘到位标示位M752 TRUE到位" />
          <el-step title="步骤 6" description="确认删除或取消删除" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="sevenNext">下一步</el-button>
      </el-tab-pane>
      <el-tab-pane label="整理存储空间" name="eight">
        <el-steps :active="eightActive" finish-status="success">
          <el-step title="步骤 1" description="按下开始整理" />
          <el-step title="步骤 2" description="监视M801 TRUE时显示空间利用率" />
          <el-step title="步骤 3" description="确认整理完毕" />
        </el-steps>
        <el-button style="margin-top: 12px;" @click="eightNext">下一步</el-button>
      </el-tab-pane>
    </el-tabs>
    <el-dialog
      v-el-drag-dialog
      title="前部和后部托架数"
      :visible.sync="dialogFirst"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>前部托架数:{{ firstNumber }}</span>
      <span>后部托架数:{{ lastNumber }}</span>
    </el-dialog>
    <el-dialog
      v-el-drag-dialog
      title="前部和后部托盘数"
      :visible.sync="dialogFourth"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>前部托盘数:{{ frontPallets }}</span>
      <span>后部托盘数:{{ rearPallets }}</span>
    </el-dialog>
    <el-dialog
      v-el-drag-dialog
      title="开始定义 写入托盘号"
      :visible.sync="dialogFourthWrite"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>托盘号</span>
      <el-input v-model="RunningContainer.TrayCode" class="dialog-input" />
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="confirmWriteD392()">确认</el-button>
      </div>
    </el-dialog>
    <el-dialog
      v-el-drag-dialog
      title="写入目标托盘号"
      :visible.sync="dialogFiveWrite"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>托盘号</span>
      <el-input v-model="RunningContainer.TrayCode" class="dialog-input" />
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="confirmWriteD650()">确认</el-button>
      </div>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      title="D651中显示的托架号"
      :visible.sync="dialogFiveShelfCode"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>托架号:{{ shlefCode }}</span>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      title="D652中显示的高度"
      :visible.sync="dialogFiveHeightCode"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>高度:{{ heightCode }}</span>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      title="写入目标托盘号"
      :visible.sync="dialogSixWrite"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>托盘号</span>
      <el-input v-model="RunningContainer.TrayCode" class="dialog-input" />
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="confirmWriteD700()">确认</el-button>
      </div>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      title="写入需要删除的目标托盘号"
      :visible.sync="dialogSevenWrite"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>托盘号</span>
      <el-input v-model="RunningContainer.TrayCode" class="dialog-input" />
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="confirmWriteD750()">确认</el-button>
      </div>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      title="确认删除或取消删除"
      :visible.sync="dialogSevenDelete"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>托盘号</span>
      <el-input v-model="RunningContainer.TrayCode" class="dialog-input" />
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="confirmM753()">确认删除</el-button>
        <el-button type="primary" @click="confirmM754()">取消删除</el-button>
      </div>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      title="空间利用率"
      :visible.sync="dialogEight"
      :width="'40%'"
      :close-on-click-modal="false"
    >
      <span>空间利用率:{{ spaceUserRate }}</span>
    </el-dialog>
  </div>
</template>
<script>
import { deletePicture } from '@/api/FileLibrary'
import waves from '@/directive/waves' // 水波纹指令
import elDragDialog from '@/directive/el-dragDialog' // base on element-ui
import { StartM300, GetM340, GetD300, GetD301, FinishM341 } from '@/api/ContainerInitialization'
import { StartM400, GetM440, FinishM441 } from '@/api/ContainerInitialization'
import { StartM450, GetM490, FinishM491 } from '@/api/ContainerInitialization'
import { StartM350, GetM390, GetD390, GetD391, StartM391, GetM392, WriteD392, ConfirmM393, ConfirmM394, GetM395, ConfirmM396 } from '@/api/ContainerInitialization'
import { WriteD650, GetM650, GetD651, StartM651, GetD652 } from '@/api/ContainerInitialization'
import { WriteD700, StartM700, GetM701, ConfirmM702 } from '@/api/ContainerInitialization'
import { WriteD750, StartM750, GetD751, StartM751, GetM752, ConfirmM753, ConfirmM754 } from '@/api/ContainerInitialization'
import { StartM800, GetM801, GetD800, ConfirmM802 } from '@/api/ContainerInitialization'
export default {
  name: 'Box', // 载具箱管理
  directives: {
    elDragDialog,
    waves
  },
  data() {
    return {
      activeName: 'first',
      firstActive: 0,
      dialogFirst: false,
      firstNumber: 0,
      lastNumber: 0,
      secondActive: 0,
      thirdActive: 0,
      fourthActive: 0,
      dialogFourth: false,
      frontPallets: 0,
      rearPallets: 0,
      RunningContainer: {
        TrayCode: undefined,
        ContainerCode: '',
        XLight: undefined
      },
      dialogFourthWrite: false,
      fiveActive: 0,
      dialogFiveWrite: false,
      shlefCode: undefined,
      dialogFiveShelfCode: false,
      heightCode: undefined,
      dialogFiveHeightCode: false,
      sixActive: 0,
      dialogSixWrite: false,
      sevenActive: 0,
      eightActive: 0,
      dialogSevenDelete: false,
      dialogSevenWrite: false,
      spaceUserRate: undefined,
      dialogEight: false
      // firstTimer: undefined
    }
  },
  computed: {

  },
  watch: {

  },
  created() {
  },
  beforeDestroy() {
    clearInterval(this.firstTimer)
  },
  destroyed() {
    if (this.firstTimer) {
      clearInterval(this.firstTimer)
    }
  },
  methods: {
    firstNext() {
      if (this.firstActive === 0) {
        // 执行
        StartM300().then((res) => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.firstActive++

            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM340().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  GetD300().then(res => {
                    var data = JSON.parse(res.data.Content)
                    _this.firstNumber = data.Data
                  })
                  GetD301().then(res => {
                    var data = JSON.parse(res.data.Content)
                    _this.lastNumber = data.Data
                  })
                  _this.dialogFirst = true
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                  _this.firstActive++
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '开始垂直学习失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.firstActive === 1) {
        // 关闭弹框
        if (this.firstActive++ > 2) {
          this.firstActive = 0
        }
      } else if (this.firstActive === 2) {
        // 确认完成
        FinishM341().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.$message({
              title: '成果',
              message: '结束垂直学习成功' + resData.Message,
              type: 'success',
              duration: 2000
            })
            if (this.firstActive++ > 2) {
              this.firstActive = 0
            }
          } else {
            this.$message({
              title: '失败',
              message: '开始垂直学习失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
            if (this.firstActive++ > 2) {
              this.firstActive = 0
            }
          }
        })
      } else {
        if (this.firstActive++ > 2) {
          this.firstActive = 0
        }
      }
    },
    secondNext() {
      if (this.secondActive === 0) {
        // 执行
        StartM400().then((res) => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.secondActive++

            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM440().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                  _this.secondActive++
                  _this.$message({
                    title: '成功',
                    message: '水平学习行程已结束',
                    type: 'success',
                    duration: 2000
                  })
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '开始水平学习行程失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.secondActive === 1) {
        // 关闭弹框
        if (this.secondActive++ > 2) {
          this.secondActive = 0
        }
      } else if (this.secondActive === 2) {
        // 确认完成
        FinishM441().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.$message({
              title: '成果',
              message: '结束水平行程学习成功' + resData.Message,
              type: 'success',
              duration: 2000
            })
            if (this.secondActive++ > 2) {
              this.secondActive = 0
            }
          } else {
            this.$message({
              title: '失败',
              message: '结束水平行程学习失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else {
        if (this.secondActive++ > 2) {
          this.secondActive = 0
        }
      }
    },
    thirdNext() {
      if (this.thirdActive === 0) {
        // 执行
        StartM450().then((res) => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.thirdActive++

            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM490().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  _this.thirdActive++
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                  _this.$message({
                    title: '成功',
                    message: '自动门学习行程已结束',
                    type: 'success',
                    duration: 2000
                  })
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '开始自动门学习行程失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.thirdActive === 1) {
        // 关闭弹框
        if (this.thirdActive++ > 2) {
          this.thirdActive = 0
        }
      } else if (this.thirdActive === 2) {
        // 确认完成
        FinishM491().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.$message({
              title: '成果',
              message: '结束自动门行程学习成功' + resData.Message,
              type: 'success',
              duration: 2000
            })
            if (this.thirdActive++ > 2) {
              this.thirdActive = 0
            }
          } else {
            this.$message({
              title: '失败',
              message: '结束水平行程学习失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else {
        if (this.thirdActive++ > 2) {
          this.thirdActive = 0
        }
      }
    },
    fourthNext() {
      console.log(this.fourthActive)
      if (this.fourthActive === 0) {
        // 执行
        StartM350().then((res) => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.fourthActive++

            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM390().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  GetD390().then(res => {
                    var data = JSON.parse(res.data.Content)
                    _this.frontPallets = data.Data
                  })
                  GetD391().then(res => {
                    var data = JSON.parse(res.data.Content)
                    _this.rearPallets = data.Data
                  })
                  _this.dialogFourth = true
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                  _this.fourthActive++
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '启动托盘扫描失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.fourthActive === 1) {
        // 关闭弹框
        this.fourthActive++
      } else if (this.fourthActive === 2) {
        StartM391().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.fourthActive++
            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM392().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  _this.dialogFourthWrite = true
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '开始定义失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.fourthActive === 4) {
        // 确认完成
        ConfirmM394().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.fourthActive++
            GetM395().then(res => {
              var data = JSON.parse(res.data.Content)
              if (data.Success && data.Data === true) {
                _this.fourthActive++
              } else if (data.Success && data.Data === false) {
                _this.fourthActive = 3
                _this.firstTimer = setInterval(function() { // 定时器开始
                  GetM392().then(res => {
                    var data = JSON.parse(res.data.Content)
                    if (data.Success && data.Data === true) {
                      _this.dialogFourthWrite = true
                      clearInterval(_this.firstTimer)// 满足条件时 停止计时
                    }
                  })
                }, 5000)
              } else {
                this.$message({
                  title: '失败',
                  message: '获取M395状态失败' + data.Message,
                  type: 'error',
                  duration: 2000
                })
              }
            })
          } else {
            this.$message({
              title: '失败',
              message: '开始垂直学习失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.fourthActive === 6) {
        // 确认完成
        ConfirmM396().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.$message({
              title: '成功',
              message: '结束托盘扫描成功' + resData.Message,
              type: 'success',
              duration: 2000
            })
            if (this.fourthActive++ > 6) {
              this.fourthActive = 0
            }
          } else {
            this.$message({
              title: '失败',
              message: '结束托盘扫描失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else {
        if (this.fourthActive++ > 6) {
          this.fourthActive = 0
        }
      }
    },
    confirmWriteD392() {
      WriteD392(this.RunningContainer).then(res => {
        var resData = JSON.parse(res.data.Content)
        if (resData.Success) {
          ConfirmM393().then(res => {
            var resData = JSON.parse(res.data.Content)
            if (resData.Success) {
              this.$message({
                title: '成功',
                message: '写入托盘号成功' + resData.Message,
                type: 'success',
                duration: 2000
              })
              this.fourthActive++
              this.dialogFourthWrite = false
            } else {
              this.$message({
                title: '失败',
                message: '写入托盘号失败' + resData.Message,
                type: 'error',
                duration: 2000
              })
            }
          })
        } else {
          this.$message({
            title: '失败',
            message: '写入托盘号失败' + resData.Message,
            type: 'error',
            duration: 2000
          })
        }
      })
    },
    fiveNext() {
      if (this.fiveActive === 0) {
        this.dialogFiveWrite = true
      } else if (this.fiveActive === 1) {
        GetM650().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success && resData.Data === true) {
            GetD651().then(res => {
              var data = JSON.parse(res.data.Content)
              if (data.Success) {
                this.dialogFiveShelfCode = true
                this.shlefCode = data.Data
                this.fiveActive++
              } else {
                this.$message({
                  title: '失败',
                  message: '获取托架号失败' + resData.Message,
                  type: 'error',
                  duration: 2000
                })
              }
            })
          } else {
            this.$message({
              title: '失败',
              message: '获取状态失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.fiveActive == 2) {
        StartM651().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.fiveActive++
          } else {
            this.$message({
              title: '失败',
              message: '启动失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.fiveActive === 3) {
        GetD652().then(res => {
          var data = JSON.parse(res.data.Content)
          if (data.Success) {
            this.dialogFiveHeightCode = true
            this.heightCode = data.Data
            this.fiveActive++
          } else {
            this.$message({
              title: '失败',
              message: '获取高度失败' + data.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else {
        if (this.fiveActive++ > 3) {
          this.fiveActive = 0
        }
      }
    },
    confirmWriteD650() {
      WriteD650(this.RunningContainer).then(res => {
        var resData = JSON.parse(res.data.Content)
        if (resData.Success) {
          this.fiveActive++
          this.dialogFiveWrite = false
        } else {
          this.$message({
            title: '失败',
            message: '写入托盘号失败' + resData.Message,
            type: 'error',
            duration: 2000
          })
        }
      })
    },
    sixNext() {
      if (this.sixActive === 0) {
        this.dialogSixWrite = true
      } else if (this.sixActive === 1) {
        StartM700().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.sixActive++
            this.$message({
              title: '成功',
              message: '启动成功' + resData.Message,
              type: 'success',
              duration: 2000
            })
          } else {
            this.$message({
              title: '失败',
              message: '启动失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.sixActive == 2) {
        GetM701().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            if (resData.Data == true) {
              this.sixActive++
            } else {
              this.$message({
                title: '失败',
                message: '空间不足' + resData.Message,
                type: 'error',
                duration: 2000
              })
            }
          } else {
            this.$message({
              title: '失败',
              message: '获取状态失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.sixActive === 3) {
        ConfirmM702().then(res => {
          var data = JSON.parse(res.data.Content)
          if (data.Success) {
            this.$message({
              title: '成功',
              message: '确认添加成功' + data.Message,
              type: 'success',
              duration: 2000
            })
            this.sixActive++
          } else {
            this.$message({
              title: '失败',
              message: '确认添加失败' + data.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else {
        if (this.sixActive++ > 3) {
          this.sixActive = 0
        }
      }
    },
    confirmWriteD700() {
      WriteD700(this.RunningContainer).then(res => {
        var resData = JSON.parse(res.data.Content)
        if (resData.Success) {
          this.sixActive++
          this.dialogSixWrite = false
        } else {
          this.$message({
            title: '失败',
            message: '写入托盘号失败' + resData.Message,
            type: 'error',
            duration: 2000
          })
        }
      })
    },
    sevenNext() {
      if (this.sevenActive === 0) {
        this.dialogSevenWrite = true
      } else if (this.sevenActive === 1) {
        StartM750().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.sevenActive++
            this.$message({
              title: '成功',
              message: '启动成功' + resData.Message,
              type: 'success',
              duration: 2000
            })
          } else {
            this.$message({
              title: '失败',
              message: '启动失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.sevenActive === 2) {
        GetD751().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.sevenActive++
            this.$message({
              title: '成功',
              message: '托架号' + resData.Data,
              type: 'success',
              duration: 2000
            })
          } else {
            this.$message({
              title: '失败',
              message: '获取状态失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.sevenActive === 3) {
        StartM751().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.sevenActive++

            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM752().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  _this.$message({
                    title: '成功',
                    message: '托盘已到位',
                    type: 'success',
                    duration: 2000
                  })
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                  _this.sevenActive++
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '运行失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.sevenActive === 5) {
        this.dialogSevenDelete = true
      } else {
        this.sevenActive++
        if (this.sevenActive > 6) {
          console.log(this.sevenActive)
          this.sevenActive = 0
        }
      }
    },
    confirmWriteD750() {
      WriteD750(this.RunningContainer).then(res => {
        var resData = JSON.parse(res.data.Content)
        if (resData.Success) {
          this.sevenActive++
          this.dialogSevenWrite = false
        } else {
          this.$message({
            title: '失败',
            message: '写入托盘号失败' + resData.Message,
            type: 'error',
            duration: 2000
          })
        }
      })
    },
    confirmM753() {
      ConfirmM753().then(res => {
        var resData = JSON.parse(res.data.Content)
        if (resData.Success) {
          this.$message({
            title: '成功',
            message: '确认删除成功',
            type: 'success',
            duration: 2000
          })
          this.sevenActive++
          this.dialogSevenDelete = false
        } else {
          this.$message({
            title: '失败',
            message: '确认删除失败' + resData.Message,
            type: 'error',
            duration: 2000
          })
        }
      })
    },
    confirmM754() {
      ConfirmM754().then(res => {
        var resData = JSON.parse(res.data.Content)
        if (resData.Success) {
          this.$message({
            title: '成功',
            message: '取消删除成功',
            type: 'success',
            duration: 2000
          })
          this.sevenActive++
          this.dialogSevenDelete = false
        } else {
          this.$message({
            title: '失败',
            message: '取消删除失败' + resData.Message,
            type: 'error',
            duration: 2000
          })
        }
      })
    },
    eightNext() {
      if (this.eightActive === 0) {
        // 执行
        StartM800().then((res) => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            const _this = this
            _this.eightActive++

            _this.firstTimer = setInterval(function() { // 定时器开始
              GetM801().then(res => {
                var data = JSON.parse(res.data.Content)
                if (data.Success && data.Data === true) {
                  GetD800().then(res => {
                    var data = JSON.parse(res.data.Content)
                    _this.spaceUserRate = data.Data
                  })
                  _this.dialogEight = true
                  clearInterval(_this.firstTimer)// 满足条件时 停止计时
                  _this.eightActive++
                }
              })
            }, 5000)
          } else {
            this.$message({
              title: '失败',
              message: '开始整理存储空间失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else if (this.eightActive === 1) {
        // 关闭弹框
        if (this.eightActive++ > 2) {
          this.eightActive = 0
        }
      } else if (this.eightActive === 2) {
        // 确认完成
        ConfirmM802().then(res => {
          var resData = JSON.parse(res.data.Content)
          if (resData.Success) {
            this.$message({
              title: '成功',
              message: '整理完毕' + resData.Message,
              type: 'success',
              duration: 2000
            })
            if (this.eightActive++ > 2) {
              this.eightActive = 0
            }
          } else {
            this.$message({
              title: '失败',
              message: '确认整理完毕失败' + resData.Message,
              type: 'error',
              duration: 2000
            })
          }
        })
      } else {
        if (this.eightActive++ > 2) {
          this.eightActive = 0
        }
      }
    },
    handleClick(tab, event) {

    }
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
</style>

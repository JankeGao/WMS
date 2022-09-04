using System.ComponentModel;

namespace Bussiness.Enums
{
    public enum DeviceAlarmEnum
    {


        [Description("紧急停止")]
        Urgencye = 200,

        [Description("冲顶保护")]
        Reach = 201,

        [Description("托底保护")]
        Fault = 202,

        [Description("维修门开启")]
        Maintain = 203,

        [Description("电机1过热")]
        DiaphragmKeepOut = 204,

        [Description("电机2过热")]
        ElectricalMachineryOverheating = 205,

        [Description("伺服驱动器故障")]
        UuivertorOverload = 206,

        [Description("变频器故障")]
        UuivertorOverheating = 207,

        [Description("安全光栅遮拦")]
        InputDefaultPhase = 208,

        [Description("物料高度超限")]
        OutputDefaultPhase = 209,

        [Description("垂直定位偏差超限")]
        UuivertorCPU = 210,

        [Description("水平定位偏差超限")]
        UuivertorPowerFailure = 211,

        [Description("取料口托盘位置不正确")]
        AlarmHappen = 212,

        [Description("勾爪位置不正确")]
        AlarmReset = 213,

        [Description("托架被占用")]
        StartingUpReset = 214,

        [Description("货柜空间不足")]
        Affirm = 215,
        [Description("物料遮挡高度光栅")]
        MaterialHigher = 216,
        [Description("物料超重")]
        MaterialWeighter = 217,
        [Description("安全门超限")]
        SafeDoor = 218,
        [Description("安全回路未激活")]
        SafeNotLive = 219,
        [Description("托盘号不存在")]
        TrayNotFound = 220,
        [Description("托架号不存在")]
        TrayShelfNotFound = 221,
        [Description("托盘号重复")]
        TrayAreadyExist = 222,

    }

    public enum DeviceAlarmEnum1
    {
        [Description("紧急停止")]
        Urgencye = 200,

        [Description("安全触边动作")]
        Reach = 201,

        [Description("偏载报警")]
        Fault = 202,

        [Description("维修面板开启")]
        Maintain = 203,

        [Description("安全光栏遮挡")]
        DiaphragmKeepOut = 204,

        [Description("电机过热")]
        ElectricalMachineryOverheating = 205,

        [Description("变频器过载")]
        UuivertorOverload = 206,

        [Description("变频器过热")]
        UuivertorOverheating = 207,

        [Description("输入缺相")]
        InputDefaultPhase = 208,

        [Description("输出缺相")]
        OutputDefaultPhase = 209,

        [Description("变频器CPU故障")]
        UuivertorCPU = 210,

        [Description("变频器内部电路故障")]
        UuivertorPowerFailure = 211,

        [Description("报警发生")]
        AlarmHappen = 212,

        [Description("报警重置")]
        AlarmReset = 213,

        [Description("开机报警重置")]
        StartingUpReset = 214,

        [Description("触摸屏确认按钮")]
        Affirm = 215,

    }
}

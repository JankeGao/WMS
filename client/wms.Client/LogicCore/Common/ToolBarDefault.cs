﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace wms.Client.LogicCore.Common
{
    /// <summary>
    /// 功能定义
    /// </summary>
    public class ToolBarDefault<T> : ViewModelBase where T : class, new()
    {
        private bool hide = false;
        private bool isVisibility;
        private int authvalue;
        private string mark;
        private string moduleName;
        private RelayCommand<T> command;
        private string iconString;

        /// <summary>
        /// 图标名称
        /// </summary>
        public string IconSting
        {
            get { return iconString; }
            set { iconString = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hide
        {
            get { return hide; }
            set { hide = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 是否隐藏在功能列表， 集成在表格中
        /// </summary>
        public bool IsVisibility
        {
            get { return isVisibility; }
            set { isVisibility = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 权限定义值
        /// </summary>
        public int AuthValue
        {
            get { return authvalue; }
            set { authvalue = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 显示符号代码
        /// </summary>
        public string Mark
        {
            get { return mark; }
            set { mark = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 功能命令命令
        /// </summary>
        public RelayCommand<T> Command { get { return command; } set { command = value; RaisePropertyChanged(); } }

    }
}

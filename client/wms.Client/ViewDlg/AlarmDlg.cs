﻿using System.Windows;
using wms.Client.LogicCore.Common;
using wms.Client.LogicCore.Interface;
using wms.Client.View;
using wms.Client.ViewModel;
using wms.Client.ViewModel.Base;

namespace wms.Client.ViewDlg
{
    [Autofac(true)]
    public class AlarmDlg : BaseView<AlarmView, AlarmViewModel>, IModel
    {

    }
}

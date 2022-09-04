using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bussiness.Contracts;
using Bussiness.Dtos;
using Bussiness.Entitys;
using Bussiness.Enums;
using GalaSoft.MvvmLight.Command;
using HP.Core.Data;
using HP.Core.Dependency;
using HP.Core.Mapping;
using MaterialDesignThemes.Wpf;
using wms.Client.Core.Interfaces;
using wms.Client.LogicCore.Common;
using wms.Client.LogicCore.Configuration;
using wms.Client.LogicCore.Enums;
using wms.Client.LogicCore.Helpers.Files;
using wms.Client.LogicCore.Interface;
using wms.Client.Model.Entity;
using wms.Client.Service;
using wms.Client.UiCore.Template;
using wms.Client.View;
using wms.Client.ViewModel.Base;
using wms.Client.ViewModel;
using RunningContainer = wms.Client.Model.Entity.RunningContainer;

namespace wms.Client.ViewModel
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [Module(ModuleType.InManage, "InTaskDlg", "入库任务")]
    public class InTaskViewModel : DataProcess<InTask>
    {
        private readonly string _basePath = ConfigurationManager.AppSettings["ServerIP"];
        /// <summary>
        /// 入库任务契约
        /// </summary>
        private readonly IInTaskContract InTaskContract;

        /// <summary>
        /// 仓库契约
        /// </summary>
        private readonly IWareHouseContract WareHouseContract;


        /// <summary>
        /// 物料契约
        /// </summary>
        private readonly IMaterialContract MaterialContract;

        /// <summary>
        /// 条码契约
        /// </summary>
        private readonly ILabelContract LabelContract;

        /// <summary>
        /// 条码契约
        /// </summary>
        private readonly IMapper Mapper;


        private readonly IRepository<Bussiness.Entitys.Container, int> ContainerRepository;
        public InTaskViewModel()
        {
            ContainerRepository = IocResolver.Resolve<IRepository<Bussiness.Entitys.Container, int>>();
            InTaskContract = IocResolver.Resolve<IInTaskContract>();
            LabelContract = IocResolver.Resolve<ILabelContract>();
            WareHouseContract = IocResolver.Resolve<IWareHouseContract>();
            Mapper = IocResolver.Resolve<IMapper>();
            MaterialContract = IocResolver.Resolve<IMaterialContract>();
            UserLoginCommand = new RelayCommand<string>(ShowLogin);
            LoginOutCommand = new RelayCommand<string>(LoginOut);
            ScanBarcodeCommand = new RelayCommand<string>(ScanBarcode);
            SelectItemCommand= new RelayCommand<InTaskMaterialDto>(SelectInTaskItem);
            HandShelfCommand = new RelayCommand(HandShelf);
            RunningCommand = new RelayCommand (RunningContainer);
            RunningTakeInCommand = new RelayCommand(RunningTakeInContainer);
            SubmitCommand = new RelayCommand(Submit);
            PrintItemCommand= new RelayCommand<InTaskMaterialDto>(PrintItem);
            this.ReadConfigInfo();
            this.GetAllPageData();
        }


        private bool _IsCancel = true;

        /// <summary>
        /// 禁用按钮
        /// </summary>
        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; RaisePropertyChanged(); }
        }



        #region 任务组

        private ObservableCollection<InTaskGroup> _ModuleGroups = new ObservableCollection<InTaskGroup>();

        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<InTaskGroup> ModuleGroups
        {
            get { return _ModuleGroups; }
            set { _ModuleGroups = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<InTaskMaterialDto> _InTaskMaterial= new ObservableCollection<InTaskMaterialDto>();

        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<InTaskMaterialDto> InTaskMaterial
        {
            get { return _InTaskMaterial; }
            set { _InTaskMaterial = value; RaisePropertyChanged(); }
        }


        #endregion

        #region 命令(Binding Command)

        private RelayCommand _signCommand;

        public RelayCommand SignCommand
        {
            get
            {
                if (_signCommand == null)
                {
                    _signCommand = new RelayCommand(() => GetMyPageData());
                }
                return _signCommand;
            }
        }

        private RelayCommand _allCommand;

        public RelayCommand AllCommand
        {
            get
            {
                if (_allCommand == null)
                {
                    _allCommand = new RelayCommand(() => GetAllPageData());
                }
                return _allCommand;
            }
        }

        private RelayCommand _userLoginCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<string> UserLoginCommand { get; private set; }

        /// <summary>
        /// 退出登录
        /// </summary>
        private RelayCommand _LoginOutCommand;

        public RelayCommand<string> LoginOutCommand { get; private set; }

        private RelayCommand _scanBarcodeCommand;

        /// <summary>
        /// 扫描入库条码
        /// </summary>
        public RelayCommand<string> ScanBarcodeCommand { get; private set; }



        private RelayCommand _inTaskCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<string> InTaskCommand { get; private set; }



        private RelayCommand _selectItemCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<InTaskMaterialDto> SelectItemCommand { get; private set; }


        private RelayCommand _PrintItemCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<InTaskMaterialDto> PrintItemCommand { get; private set; }


        /// <summary>
        /// 确认存入指令
        /// </summary>
        private RelayCommand _handShelfCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand  HandShelfCommand { get; private set; }


        /// <summary>
        /// 完成提交
        /// </summary>
        private RelayCommand _returnCommand;

        public RelayCommand ReturnCommand
        {
            get
            {
                if (_returnCommand == null)
                {
                    _returnCommand = new RelayCommand(() => ChangeTabPageIndex());
                }
                return _returnCommand;
            }
        }

        /// <summary>
        /// 完成提交
        /// </summary>
        private RelayCommand _submitCommand;
        public RelayCommand SubmitCommand { get; private set; }


        /// <summary>
        /// 驱动货柜运转
        /// </summary>
        private RelayCommand _RunningCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand RunningCommand { get; private set; }

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand RunningTakeInCommand { get; private set; }
        

        /// <summary>
        /// 清空数据
        /// </summary>
        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                if (_ClearCommand == null)
                {
                    _ClearCommand = new RelayCommand(() => Clear());
                }
                return _ClearCommand;
            }
        }

        /// <summary>
        /// 手动入库的物料
        /// </summary>
        private string selectMaterialCode = string.Empty;
        public string SelectMaterialCode
        {
            get { return selectMaterialCode; }
            set { selectMaterialCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 扫描的条码实体
        /// </summary>
        public LabelClient LabelEntity { get; set; } = new LabelClient();
        /// <summary>
        /// 客户端货柜编码
        /// </summary>
        private string ContainerCode = string.Empty;

        /// <summary>
        /// 搜索条码
        /// </summary>
        private string searchBarcode = string.Empty;
        public string SearchBarcode
        {
            get { return searchBarcode; }
            set { searchBarcode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 自动存入
        /// </summary>
        private bool _AutoOprate = true;
        public bool AutoOprate
        {
            get { return _AutoOprate; }
            set { _AutoOprate = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前托盘-货柜运行后
        /// </summary>
        private string curTratCode = string.Empty;
        public string CurTratCode
        {
            get { return curTratCode; }
            set { curTratCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前操作托盘号
        /// </summary>
        private string trayCode = string.Empty;
        public string TrayCode
        {
            get { return trayCode; }
            set { trayCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前操作储位
        /// </summary>
        private string locationCode = string.Empty;
        public string LocationCode
        {
            get { return locationCode; }
            set { locationCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前操作储位
        /// </summary>
        private int _XLight = 0;
        public int XLight
        {
            get { return _XLight; }
            set { _XLight = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前操作储位
        /// </summary>
        private int _YLight = 0;
        public int YLight
        {
            get { return _YLight; }
            set { _YLight = value; RaisePropertyChanged(); }
        }

        private string boxUrl = string.Empty;
        public string BoxUrl
        {
            get { return boxUrl; }
            set { boxUrl = value; RaisePropertyChanged(); }
        }

        private string materialUrl = string.Empty;
        public string MaterialUrl
        {
            get { return materialUrl; }
            set { materialUrl = value; RaisePropertyChanged(); }
        }

        private string boxName = string.Empty;
        public string BoxName
        {
            get { return boxName; }
            set { boxName = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 当前操作储位
        /// </summary>
        private decimal inQuantity = 0;
        public decimal InQuantity
        {
            get { return inQuantity; }
            set { inQuantity = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 当前操作的任务明细
        /// </summary>
        private InTaskMaterialDto inTaskMaterialEntity = new InTaskMaterialDto();
        public InTaskMaterialDto InTaskMaterialEntity
        {
            get { return inTaskMaterialEntity; }
            set { inTaskMaterialEntity = value; RaisePropertyChanged(); }
        }


        #endregion

        /// <summary>
        /// 核验登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckLogin()
        {
            // 系统用户注销时间
            var checkTime = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["LoginOutTime"].ToString());
            // 如果未登录
            if (string.IsNullOrWhiteSpace(GlobalData.loginTime))
            {
                return false;
            }
            else
            {
                var login = Convert.ToDateTime(GlobalData.loginTime).AddMinutes(checkTime);
                var now = DateTime.Now;

                // 如果时间已过期
                if (DateTime.Compare(now, login) > 0)
                {
                    // 系统登录
                    return false;
                }
                else
                {
                    // 核查用户是否有此模块操作权限
                    var user = ServiceProvider.Instance.Get<IUserService>();
                    var authCheck = user.GetCheckAuth(GlobalData.LoginModule);
                    if (!authCheck.Result.Success)
                    {
                        if (await Msg.Question("抱歉，您无该模块操作权限！"))
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }



        /// <summary>
        /// 核验登录人员
        /// </summary>
        /// <param name="code"></param>
        public async void ShowLogin(string code)
        {
            try
            {
                // 重新读取登录信息
                //this.ReadConfigInfo();
                if (string.IsNullOrWhiteSpace(code)) return;

                // 查询全部任务
                var containerCode = InTaskContract.InTaskDtos.FirstOrDefault(a => a.Code == code).ContainerCode;
                if (containerCode != ContainerCode)
                {
                    Msg.Warning("该任务不属于本货柜,请选择正确的任务执行");
                    return;
                }
                GlobalData.LoginModule = "ReceiptTask";
                GlobalData.LoginPageCode = "InTaskDlg";
                GlobalData.LoginPageName = "入库任务";
                //如果登录
                if (await CheckLogin())
                {
                    // 获取当前任务的明细
                    var inTaskMaterialList = InTaskContract.InTaskMaterialDtos.Where(a => a.InTaskCode == code && a.Status != (int)InTaskStatusCaption.Finished).OrderBy(a => a.SuggestTrayCode).ToList();
                    InTaskMaterial.Clear();
                    inTaskMaterialList.ForEach((arg) => InTaskMaterial.Add(arg));
                    TabPageIndex = 1;
                    GlobalData.IsFocus = true;
                }
                else    //如果未登录
                {
                    var dialog = ServiceProvider.Instance.Get<IShowContent>();
                    dialog.BindDataContext(new UserLoginWindow(), new UserLoginModel());
                    dialog.Show();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="code"></param>
        public async void LoginOut(string code)
        {
            // 重新读取登录信息
            if (await Msg.Question("是否退出登录？"))
            {
                // 清除登录信息
                GlobalData.loginTime = "";
                GlobalData.UserCode = "";
                GlobalData.UserName = "";
                GlobalData.PictureUrl = "";
                var obj = new MainViewModel();
                if (obj == null) return;
                obj.ExitPage(MenuBehaviorType.ExitAllPage, "");
            }
        }
        
        /// <summary>
        /// 打印条码
        /// </summary>
        public async void PrintItem(Bussiness.Dtos.InTaskMaterialDto entity)
        {
            // 判断是否启用单包条码管理
            var materialEntity = MaterialContract.Materials.FirstOrDefault(a => a.Code == entity.MaterialCode);

            decimal tempQuantity = 0;
            // 判断是否启用条码
            if (materialEntity.IsPackage)
            {
                tempQuantity = (decimal)materialEntity.PackageQuantity;
            }
            var label = new LabelClient()
            {
                MaterialCode= entity.MaterialCode,
                MaterialName= entity.MaterialName,
                BatchCode= entity.BatchCode,
                SupplyCode= entity.SupplierCode,
                Quantity= tempQuantity
            };
            var dialog = ServiceProvider.Instance.Get<IShowContent>();
            dialog.BindDataContext(new BarCodeView(), new BarCodeViewModel() { LabelEntity = label});
            //    dialog.D
            // 条码打印
            dialog.Show();
        }

        /// <summary>
        /// 扫描入库条码-物料条码-物料编码-储位码
        /// </summary>
        public async void ScanBarcode(string code)
        {
            try
            {
                if (String.IsNullOrEmpty(SearchBarcode))
                {
                    return;
                }
                var labelEnity = LabelContract.LabelDtos.Where(a => a.Code == SearchBarcode).FirstOrDefault();
                if (labelEnity == null)
                {
                    // 获取入库任务行项目
                    var selectIntask = InTaskMaterial.FirstOrDefault(a => a.MaterialCode == SearchBarcode && (a.Status != (int)InTaskStatusCaption.Finished && a.Status != (int)InTaskStatusCaption.Cancel));
                    // 判断是否为物料编码
                    if (selectIntask != null)
                    {
                        if (String.IsNullOrEmpty(TrayCode) || TrayCode != selectIntask.SuggestTrayCode)
                        {
                            // 默认选择一项相同物料的
                            if (GlobalData.DeviceStatus == (int)DeviceStatusEnum.Fault)
                            {
                                GlobalData.IsFocus = true;
                                Msg.Warning("设备离线状态，无法启动货柜！");
                                return;
                            }
                            SelectInTaskItem(selectIntask);
                        }
                        else
                        {
                            Msg.Warning("已选择入库行项目");
                            return;
                        }

                        Clear();
                    }
                    else
                    {
                        // 如果是储位码
                        var locationCode = WareHouseContract.Locations.FirstOrDefault(a => a.Code == SearchBarcode);
                        //  var dialog = ServiceProvider.Instance.Get<IShowContent>();
                        if (locationCode != null)
                        {
                            // 如果是当前储位
                            if (locationCode.Code == LocationCode)
                            {
                                // 获取入库行项目明细
                                if (InTaskMaterialEntity==null)
                                {
                                    Msg.Warning("请选择入库行项目明细！");
                                    return;
                                }
                                var material = MaterialContract.MaterialDtos.FirstOrDefault(a => a.Code == InTaskMaterialEntity.MaterialCode);
                                LabelEntity.MaterialCode = InTaskMaterialEntity.MaterialCode;
                                LabelEntity.MaterialName = material.Name;
                                LabelEntity.MaterialUrl = _basePath + material.PictureUrl;

                                // 如果是自动存入
                                if (AutoOprate && InQuantity > 0)
                                {
                                    HandShelf();
                                }
                                else
                                {
                                    Clear();
                                    //dialog.BindDataContext(new MsgBox(), new MsgBoxViewModel() { Msg = "请输入入库数量！", Icon = "CommentProcessingOutline", Color = "#FF4500", BtnHide = true });
                                    //dialog.Show();
                                    //await Task.Delay(3000);
                                    //DialogHost.CloseDialogCommand.Execute(null, null);
                                    Msg.Info("请输入入库数量！");
                                    return;
                                }
                            }
                            else // 如果不是当前储位
                            {
                                var selectInTaskMaterial = InTaskMaterial.FirstOrDefault(a => a.SuggestLocation == locationCode.Code && a.Status != (int)InTaskStatusCaption.Finished);
                                // 如果扫描储位是待入库行项目
                                if (selectInTaskMaterial != null)
                                {
                                    SelectInTaskItem(selectInTaskMaterial);
                                }
                                else
                                {
                                    Clear();
                                    //dialog.BindDataContext(new MsgBox(), new MsgBoxViewModel() { Msg = "未选择入库行项目，或该储位不属于该明细！", Icon = "CommentProcessingOutline", Color = "#FF4500", BtnHide = true });
                                    //dialog.Show();
                                    //await Task.Delay(3000);
                                    //DialogHost.CloseDialogCommand.Execute(null, null);
                                    Msg.Info("未选择入库行项目，或该储位不属于该明细！");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // Clear();
                            //dialog.BindDataContext(new MsgBox(), new MsgBoxViewModel() { Msg = "未获取到条码信息！", Icon = "CommentProcessingOutline", Color = "#FF4500", BtnHide = true });
                            //dialog.Show();
                            //await Task.Delay(3000);
                            //DialogHost.CloseDialogCommand.Execute(null, null);
                            Msg.Info("未获取到条码信息！");
                            return;
                        }
                    }
                }
                else // 如果是物料条码
                {
                    LabelEntity.LabelCode = labelEnity.Code;
                    LabelEntity.MaterialCode = labelEnity.MaterialCode;
                    LabelEntity.MaterialName = labelEnity.MaterialName;
                    LabelEntity.Quantity = labelEnity.Quantity;
                    LabelEntity.SupplyName = labelEnity.SupplyName;
                    LabelEntity.BatchCode = labelEnity.BatchCode;
                    LabelEntity.MaterialUrl = _basePath + labelEnity.MaterialUrl;
                    // 本次入库数量
                    InQuantity = labelEnity.Quantity;

                    // 如果是自动存入
                    if (AutoOprate)
                    {
                        HandShelf();
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }


        /// <summary>
        /// 选择任务行项目
        /// </summary>
        public async void SelectInTaskItem(Bussiness.Dtos.InTaskMaterialDto entity)
        {
            try
            {
                // 验证是否有操作权限
                GlobalData.IsFocus = false;
                if (entity == null)
                {
                    Msg.Warning("未获取到选中信息");
                    return;
                }

                if (entity.Status == (int)InTaskStatusCaption.Finished)
                {
                    Msg.Warning("该物料已完成");
                    return;
                }
                // 核查用户是否有此模块操作权限
                var user = ServiceProvider.Instance.Get<IUserService>();
                var authCheck = user.GetCheckTrayAuth((int)entity.SuggestTrayId);
                if (!authCheck.Result.Success)
                {
                    Msg.Warning("抱歉，您无操作该托盘权限！");
                    return;
                }
                TrayCode = entity.SuggestTrayCode;
                LocationCode = entity.SuggestLocation;
                InTaskMaterialEntity = entity;
                BoxUrl = _basePath + entity.BoxUrl;
                MaterialUrl = _basePath + entity.MaterialUrl;
                XLight = entity.XLight;
                YLight = entity.YLight;
                BoxName = entity.BoxName;
                InQuantity = entity.Quantity- entity.RealInQuantity;
                SelectMaterialCode = entity.MaterialCode;
                RunningContainer();
                GlobalData.IsFocus = true;
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }


        /// <summary>
        /// 启动货柜
        /// </summary>
        public async void RunningContainer()
        {
            try
            {
                if (GlobalData.DeviceStatus== (int)DeviceStatusEnum.Fault)
                {
                    GlobalData.IsFocus = true;
                    Msg.Warning("设备离线状态，无法启动货柜！");
                    return;
                }
                GlobalData.IsFocus = false;

                if (String.IsNullOrEmpty(TrayCode))
                {
                    GlobalData.IsFocus = true;
                    Msg.Warning("未选择入库的行项目，请先选择一项！");

                    return;
                }
                if (TrayCode != CurTratCode)
                {
                    GlobalData.IsFocus = true;

                    if (!await Msg.Question("是否驱动货柜至新的托盘处？"))
                    {
                        return;
                    }
                }

                // await dialog.c
                // 读取PLC 状态信息
                var baseControlService = ServiceProvider.Instance.Get<IBaseControlService>();

                var runingEntity = new RunningContainer()
                {
                    ContainerCode = ContainerCode,
                    TrayCode = Convert.ToInt32(TrayCode),
                    XLight = XLight
                };
                var container = ContainerRepository.Query().FirstOrDefault(a => a.Code == ContainerCode);
                if (container!=null)
                {
                    runingEntity.ContainerType = container.ContainerType;
                    runingEntity.IpAddress = container.Ip;
                    runingEntity.Port = int.Parse(container.Port);
                }
                // 货柜运行
                var runningContainer = baseControlService.PostStartRunningContainer(runingEntity);

                if (runningContainer.Result.Success)
                {
                    GlobalData.IsFocus = true;
                    //Msg.Warning("货柜运行中");
                    //var dialog = ServiceProvider.Instance.Get<IShowContent>();
                    //dialog.BindDataContext(new MsgBox(), new MsgBoxViewModel() { Msg = "货柜运行中", Icon = "CommentProcessingOutline", Color = "#FF4500", BtnHide = true });
                    //dialog.Show();
                    //await Task.Delay(1000);
                    //DialogHost.CloseDialogCommand.Execute(null, null);
                    CurTratCode = TrayCode;

                }
                else
                {
                    GlobalData.IsFocus = true;
                    Msg.Error(runningContainer.Result.Message);
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }


        public async void RunningTakeInContainer()
        {
            try
            {
                if (GlobalData.DeviceStatus == (int)DeviceStatusEnum.Fault)
                {
                    GlobalData.IsFocus = true;
                    Msg.Warning("设备离线状态，无法启动货柜！");
                    return;
                }

                GlobalData.IsFocus = false;


                string CurrentRunningTray = "";
                string cfgINI = AppDomain.CurrentDomain.BaseDirectory + wms.Client.LogicCore.Configuration.SerivceFiguration.INI_CFG;
                if (System.IO.File.Exists(cfgINI))
                {
                    wms.Client.LogicCore.Helpers.Files.IniFile ini = new wms.Client.LogicCore.Helpers.Files.IniFile(cfgINI);
                    CurrentRunningTray = ini.IniReadValue("ClientInfo", "CurrentRunningTray");
                }

                if (!string.IsNullOrEmpty(CurrentRunningTray))
                {
                    // await dialog.c
                    // 读取PLC 状态信息
                    var baseControlService = ServiceProvider.Instance.Get<IBaseControlService>();

                    var runingEntity = new RunningContainer()
                    {
                        ContainerCode = ContainerCode,
                        TrayCode = Convert.ToInt32(CurrentRunningTray),
                        //XLight = XLight
                    };
                    var container = ContainerRepository.Query().FirstOrDefault(a => a.Code == ContainerCode);
                    if (container != null)
                    {
                        runingEntity.ContainerType = container.ContainerType;
                        runingEntity.IpAddress = container.Ip;
                        runingEntity.Port = int.Parse(container.Port);
                    }

                    // 货柜运行
                   // baseControlService.WriteD650_In(runingEntity);
                    var runningContainer = baseControlService.PostStartRestoreContainer(runingEntity).Result;//WriteD650_In(runingEntity).Result;
                    if (runningContainer.Success)
                    {
                        //var result = baseControlService.SetM654True().Result;
                        //if (!result.Success)
                        //{
                        //    Msg.Error(result.Message);
                        //}
                    }
                    else
                    {
                        Msg.Error(runningContainer.Message);
                    }

                    GlobalData.IsFocus = true;
                    // baseControlService.PostStartRunningContainer(runingEntity);
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);

            }

        }
        /// <summary>
        /// 获取全部货柜任务
        /// </summary>
        public  async void GetAllPageData()
        {
            try
            {
                var query = InTaskContract.InTaskDtos.Where(a =>
                    a.Status != (int)InTaskStatusCaption.Finished);

                if (!String.IsNullOrEmpty(SearchText))
                {
                    query = query.Where(p => p.InCode.Contains(SearchText) || p.BillCode.Contains(SearchText));
                }

                // 查询全部任务
                var groupList = query.Where(a=>a.ContainerCode==ContainerCode).GroupBy(a=>a.ContainerCode).Select(a=>a.ContainerCode).ToList();
                _ModuleGroups.Clear();

                foreach (var item in groupList)
                {
                    var container = WareHouseContract.ContainerDtos.FirstOrDefault(a => a.Code == item);

                    var inTaskGroup = new InTaskGroup()
                    {
                        GroupIcon = container.BrandDescription,
                        GroupName = "货柜号：" + container.Code,
                        GroupWarehouse = _basePath + container.PictureUrl
                    };
                    // 查询全部任务
                    var inTaskContainerList = InTaskContract.InTaskDtos.Where(a => (a.Status != (int)InTaskStatusCaption.Finished && a.Status != (int)InTaskStatusCaption.Cancel) && a.ContainerCode == item).ToList();

                    foreach (var intask in inTaskContainerList)
                    {
                        var inTaskItem = new InTaskItem()
                        {
                            Code = intask.Code,
                            Name = intask.StatusCaption,
                            InCode = intask.InCode,
                            ContainerCode= container.Code
                        };
                        inTaskGroup.Modules.Add(inTaskItem);
                    }

                    _ModuleGroups.Add(inTaskGroup);
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取本机货柜任务
        /// </summary>
        public async void GetMyPageData()
        {
            try
            {
                var container = WareHouseContract.ContainerDtos.FirstOrDefault(a => a.Code == ContainerCode);
                _ModuleGroups.Clear();
                var inTaskGroup = new InTaskGroup()
                {
                    GroupIcon= container.BrandDescription,
                    GroupName = "货柜号："+container.Code,
                    GroupWarehouse = _basePath + container.PictureUrl
                };
                var query = InTaskContract.InTaskDtos.Where(a =>
                             (a.Status != (int)InTaskStatusCaption.Finished && a.Status != (int)InTaskStatusCaption.Cancel) && a.ContainerCode == ContainerCode);

                // 根据入库单或订单号查询
                if (!String.IsNullOrEmpty(SearchText))
                {
                    query = query.Where(p => p.InCode.Contains(SearchText)||p.BillCode.Contains(SearchText));
                }
                    
                // 查询全部任务
                var inTaskContainerList = query.ToList();

                foreach (var intask in inTaskContainerList)
                {
                    var inTaskItem = new InTaskItem()
                    {
                        Code = intask.Code,
                        Name = intask.StatusCaption,
                        InCode = intask.InCode,
                        ContainerCode = container.Code
                    };
                    inTaskGroup.Modules.Add(inTaskItem);
                }

                _ModuleGroups.Add(inTaskGroup);
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }


        /// <summary>
        /// 确认存入
        /// </summary>
        public void HandShelf()
        {
            try
            {
                if (String.IsNullOrEmpty(TrayCode))
                {
                    // 获取入库任务行项目
                    var selectIntask = InTaskMaterial.FirstOrDefault(a=>a.MaterialCode== LabelEntity.MaterialCode&& (a.Status != (int)InTaskStatusCaption.Finished && a.Status != (int)InTaskStatusCaption.Cancel));
                    if (selectIntask != null)
                    {
                        // 默认选择一项相同物料的
                        SelectInTaskItem(selectIntask);
                    }
                    else
                    {
                        Msg.Warning("未选择入库的行项目，请先选择一项！");
                        return;
                    }
                }

                if (CurTratCode!= TrayCode)
                {
                    Msg.Warning("当前货柜未运行至此托盘，请启动货柜！");
                    return;
                }

                // 判断是否启用单包条码管理
                var materialEntity = MaterialContract.Materials.FirstOrDefault(a => a.Code == InTaskMaterialEntity.MaterialCode);
                var labelEnity = LabelContract.LabelDtos.Where(a => a.Code == SearchBarcode).FirstOrDefault();

                // 判断是否启用条码
                if (materialEntity.IsPackage)
                {
                    // 判断条码是否为空，以及是否为物料条码
                    if (String.IsNullOrEmpty(SearchBarcode))
                    {
                        InQuantity = 0;
                        SearchBarcode = "";
                        Msg.Warning("该物料已经启用条码管理，请扫描入库条码");
                        return;
                    }
                    if (labelEnity==null)
                    {
                        InQuantity = 0;
                        SearchBarcode = "";
                        Msg.Warning("该物料已经启用条码管理，请扫描入库条码");
                        return;
                    }

                }
                // 核对入库物料是否正确
                if (LabelEntity.MaterialCode != InTaskMaterialEntity.MaterialCode)
                {
                    Clear();
                    Msg.Warning("该物料条码不属于本入库项目，请核对扫描的物料信息！");
                    return;
                }
                if (labelEnity == null)
                {
                    InTaskMaterialEntity.MaterialLabel = "";
                }
                else
                {
                    InTaskMaterialEntity.MaterialLabel = SearchBarcode;
                }

                this.IsCancel = false;
                InTaskMaterialEntity.InTaskMaterialQuantity = (decimal) InQuantity;

                // 实际上架储位-后期可维护成可做修改的
                InTaskMaterialEntity.LocationCode = LocationCode;

                var intaskService = ServiceProvider.Instance.Get<IInTaskService>();

                // 物料实体映射

                var inTask = intaskService.PostDoHandShelf(InTaskMaterialEntity);

                if (inTask.Result.Success)
                {
                    Clear();
                   // Msg.Warning("物料入库成功！");
                    // 获取当前任务的明细
                    var inTaskMaterialList = InTaskContract.InTaskMaterialDtos.Where(a => a.InTaskCode == InTaskMaterialEntity.InTaskCode&&a.Status != (int)InTaskStatusCaption.Finished).OrderBy(a => a.SuggestTrayCode).ToList();
                    InTaskMaterial.Clear();
                    inTaskMaterialList.ForEach((arg) => InTaskMaterial.Add(arg));
                    if (InTaskMaterial.Count>0)
                    {
                        // 获取入库任务行项目
                        var selectIntask = InTaskMaterial.FirstOrDefault(a => a.Status != (int)InTaskStatusCaption.Finished && a.Status != (int)InTaskStatusCaption.Cancel);
                        if (selectIntask != null)
                        {
                            // 默认选择一项相同物料的
                            SelectInTaskItem(selectIntask);
                        }
                    }
                }
                else
                {
                    Clear();
                    Msg.Error("物料入库失败：" + inTask.Result.Message);
                }
            }
            catch (Exception ex)
            {
                Msg.Error("物料入库失败：");
            }
            finally
            {
                this.IsCancel = true;
            }
        }



        /// <summary>
        /// 完成提交
        /// </summary>
        public void Submit()
        {
            try
            {
                if (InTaskMaterialEntity.InCode == null)
                {
                    Msg.Warning("未执行入库操作");
                    return;
                }

                var inTaskList = InTaskContract.InTaskDtos.Where(a => a.InCode == InTaskMaterialEntity.InCode&& a.Status != (int)InTaskStatusCaption.Finished).ToList();

                // 仍有未完成的任务
                if (inTaskList.Count > 0)
                {
                    // 全局变量设置需要指引的入库单
                    GlobalData.GuideCode = InTaskMaterialEntity.InCode;
                    GlobalData.GuideType = (int)ModuleType.InManage;
                    // 清空操作数据
                    Reset();
                    var dialog = ServiceProvider.Instance.Get<IShowContent>();
                    dialog.BindDataContext(new GuideWindow(), new GuideModel());
                    // 系统登录
                    dialog.Show();
                } 
                else // 直接跳转至任务界面
                {
                    TabPageIndex = 0;
                    this.GetAllPageData();
                }
            }
            catch (Exception ex)
            {
                Msg.Question("物料入库失败");
            }
            finally
            {
                this.IsCancel = true;
            }
        }
        /// <summary>
        /// 清空扫描
        /// </summary>
        private void Clear()
        {
            try
            {
                GlobalData.IsFocus = false;
                // 清空
                LabelEntity.MaterialName = "";
                LabelEntity.MaterialCode = "";
                LabelEntity.BatchCode = "";
                LabelEntity.Quantity = 0;
                LabelEntity.SupplyName = "";
                InQuantity = 0;
                SearchBarcode = "";
                SelectMaterialCode = "";
                LabelEntity.MaterialUrl = "";
                GlobalData.IsFocus = true;
            }
            catch (Exception ex)
            {
                Msg.Question("物料入库失败");
            }
            finally
            {
                this.IsCancel = true;
            }
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        private void Reset()
        {
            try
            {
                // 清空
                InTaskMaterialEntity = new InTaskMaterialDto();
                InTaskMaterial.Clear();
                LocationCode = "";
                TrayCode = "";
                BoxUrl = "";
                MaterialUrl = "";
                Clear();
            }
            catch (Exception ex)
            {
                Msg.Question("物料入库失败");
            }
            finally
            {
                this.IsCancel = true;
            }
        }

        public void ChangeTabPageIndex()
        {
            TabPageIndex = 0;
            this.GetAllPageData();
        }


        /// <summary>
        /// 读取本地配置信息--人员登录时间
        /// </summary>
        public void ReadConfigInfo()
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                ContainerCode = ini.IniReadValue("ClientInfo", "code");
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bussiness.Contracts;
using Bussiness.Dtos;
using Bussiness.Entitys;
using Bussiness.Enums;
using GalaSoft.MvvmLight.Command;
using HP.Core.Data;
using HP.Core.Dependency;
using HP.Core.Mapping;
using HP.Utility;
using HPC.BaseService.Contracts;
using HPC.BaseService.Models;
using MaterialDesignThemes.Wpf;
using wms.Client.Common;
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
using wms.Client.ViewModel;
using ILabelContract = Bussiness.Contracts.ILabelContract;
using RunningContainer = wms.Client.Model.Entity.RunningContainer;

namespace wms.Client.ViewModel
{
    /// <summary>
    /// 手动入库
    /// </summary>
    [Module(ModuleType.InManage, "ManualInDlg", "手动入库")]
    public class ManualInViewModel : Base.DataProcess<InTask>
    {
        private readonly string _basePath = ConfigurationManager.AppSettings["ServerIP"];

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
        private readonly ISupplyContract SupplyContract;

        /// <summary>
        /// 字典契约
        /// </summary>
        private readonly IDictionaryContract DictionaryContract;


        /// <summary>
        /// 字典契约
        /// </summary>
        private readonly IStockContract StockContract;

        /// <summary>
        /// 货柜契约
        /// </summary>
       // private readonly Bussiness.Contracts.IContrainerRunContract


        private readonly IRepository<Material, int> MaterialRepository;


        private readonly IRepository<Container, int> ContainerRepository;

        /// <summary>
        /// 条码契约
        /// </summary>
       // private readonly IMapper Mapper;


        // 称重接口
        WeightReader weightReader = new WeightReader();
        Action<string> updateWeightAction;
        private string COM;

        //public ObservableCollection<SearchModel> SearchItemsSourceCollection { get; set; }

        public ManualInViewModel()
        {
            MaterialRepository = IocResolver.Resolve<IRepository<Material, int>>();

            ContainerRepository = IocResolver.Resolve<IRepository<Container, int>>();
            SupplyContract = IocResolver.Resolve<ISupplyContract>();
            StockContract = IocResolver.Resolve<IStockContract>();
            DictionaryContract = IocResolver.Resolve<IDictionaryContract>();
            LabelContract = IocResolver.Resolve<ILabelContract>();
            WareHouseContract = IocResolver.Resolve<IWareHouseContract>();
          //  Mapper = IocResolver.Resolve<IMapper>();
            MaterialContract = IocResolver.Resolve<IMaterialContract>();
            ScanBarcodeCommand = new RelayCommand<string>(ScanBarcode);
            ScanBarcodeKeyDownCommand = new RelayCommand<object>(ScanBarcodeKeyDown);
            RunningCommand = new RelayCommand(RunningContainer);
            RunningTakeInCommand = new RelayCommand(RunningTakeInContainer);
            HandShelfCommand = new RelayCommand (HandShelf);
            DelectItemCommand = new RelayCommand<InTaskMaterialDto>(DelectInTaskItem);
            SubmitCommand = new RelayCommand(Submit);
            PrintItemCommand = new RelayCommand<InTaskMaterialDto>(PrintItem);
            LoginOutCommand = new RelayCommand<string>(LoginOut);

            weightReader.StartKey = string.Empty;
            weightReader.UnitKey = string.Empty;
            weightReader.MatchPattern = @".+[\r|\n]";
            COM = System.Configuration.ConfigurationSettings.AppSettings["WeightCOM"].ToString();
            weightReader.Changed += new EventHandler(weightReader_Changed);


            this.ReadConfigInfo(); // 读取货柜信息
            this.GetAllPageData();
            this.SelectInit();
            this.ShowLogin();
           // this.GetAllMaterial();
           
        }

        private void GetAllMaterial()
        {
            //SearchItemsSourceCollection = new ObservableCollection<SearchModel>();
            ////for (int i = 0; i < 10000; i++)
            ////{
            ////    SearchModel sm = new SearchModel();
            ////    sm.Name = "测试数据" + i;
            ////    sm.Id = Guid.NewGuid().ToString();
            ////    sm.SearchField = "搜索测试" + i + "附加数据" + i;
            ////    SearchItemsSourceCollection.Add(sm);
            ////}
            //var list = this.MaterialRepository.Query().Select(a => new SearchModel()
            //{
            //    Name = a.Code+"-"+a.Name,
            //    Id = a.Code,
            //    SearchField = a.Code + a.Name


            //}).ToList();
            //list.ForEach(p => SearchItemsSourceCollection.Add(p));
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

        /// <summary>
        /// 禁用按钮
        /// </summary>
        public bool _IsShowClear = false;
        public bool IsShowClear
        {
            get { return _IsShowClear; }
            set { _IsShowClear = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 禁用按钮
        /// </summary>
        public string _SearchTips = "测试";
        public string SearchTips
        {
            get { return _SearchTips; }
            set { _SearchTips = value; RaisePropertyChanged(); }
        }



        #region 任务组

        // 物料分组
        private ObservableCollection<Material> _ModuleGroups = new ObservableCollection<Material>();
        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<Material> ModuleGroups
        {
            get { return _ModuleGroups; }
            set { _ModuleGroups = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 托盘分组
        /// </summary>
        private ObservableCollection<LocationVIEW> _TaryGroups = new ObservableCollection<LocationVIEW>();
        public ObservableCollection<LocationVIEW> TrayGroups
        {
            get { return _TaryGroups; }
            set { _TaryGroups = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 储位分组
        /// </summary>
        private ObservableCollection<LocationVIEW> _LocationGroups = new ObservableCollection<LocationVIEW>();
        public ObservableCollection<LocationVIEW> LocationGroups
        {
            get { return _LocationGroups; }
            set { _LocationGroups = value; RaisePropertyChanged(); }
        }

        // 供应商分组
        private ObservableCollection<Supply> _SupplyGroups = new ObservableCollection<Supply>();
        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<Supply> SupplyGroups
        {
            get { return _SupplyGroups; }
            set { _SupplyGroups = value; RaisePropertyChanged(); }
        }

        // 供应商分组
        private ObservableCollection<Dictionary> _InTypeGroups = new ObservableCollection<Dictionary>();
        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<Dictionary> InTypeGroups
        {
            get { return _InTypeGroups; }
            set { _InTypeGroups = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<InTaskMaterialDto> InTaskMaterial
        {
            get { return _InTaskMaterial; }
            set { _InTaskMaterial = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<InTaskMaterialDto> _InTaskMaterial = new ObservableCollection<InTaskMaterialDto>();

        #endregion

        #region 命令(Binding Command)

        /// <summary>
        /// 退出登录
        /// </summary>
        private RelayCommand _LoginOutCommand;

        public RelayCommand<string> LoginOutCommand { get; private set; }

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
        /// 选择本次入库的物料
        /// </summary>
        private RelayCommand _materialCommand;

        public RelayCommand MaterialCommand
        {
            get
            {
                if (_returnCommand == null)
                {
                    _returnCommand = new RelayCommand(() => SelectMaterial());
                }
                return _returnCommand;
            }
        }

        /// <summary>
        /// 选择本次入库的物料
        /// </summary>
        private RelayCommand _trayCommand;

        public RelayCommand TrayCommand
        {
            get
            {
                if (_trayCommand == null)
                {
                    _trayCommand = new RelayCommand(() => SelectTray());
                }
                return _trayCommand;
            }
        }


        /// <summary>
        /// 选择本次入库的物料
        /// </summary>
        private RelayCommand _locationCommand;

        public RelayCommand LocationCommand
        {
            get
            {
                if (_locationCommand == null)
                {
                    _locationCommand = new RelayCommand(() => SelectLocation());
                }
                return _locationCommand;
            }
        }


        /// <summary>
        /// 完成提交
        /// </summary>
        private RelayCommand _supplerCommand;

        public RelayCommand SupplerCommand { get; private set; }

        private RelayCommand _scanBarcodeCommand;

        /// <summary>
        /// 扫描入库条码
        /// </summary>
        public RelayCommand<string> ScanBarcodeCommand { get; private set; }

        public RelayCommand<object> ScanBarcodeKeyDownCommand { get; private set; }



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
        private RelayCommand _submitCommand;
        public RelayCommand SubmitCommand { get; private set; }


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
        private RelayCommand _WeightCheckCommand;

        public RelayCommand WeightCheckCommand
        {
            get
            {
                if (_WeightCheckCommand == null)
                {
                    _WeightCheckCommand = new RelayCommand(() => WeightCheck());
                }
                return _WeightCheckCommand;
            }
        }

        private RelayCommand _delectItemCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<InTaskMaterialDto> DelectItemCommand { get; private set; }


        private RelayCommand _PrintItemCommand;

        /// <summary>
        /// 打开新页面，string: 模块名称
        /// </summary>
        public RelayCommand<InTaskMaterialDto> PrintItemCommand { get; private set; }

        /// <summary>
        /// 手动入库的物料
        /// </summary>
        private string _IsMoreThanTwo = "Hidden";
        public string IsMoreThanTwo
        {
            get { return _IsMoreThanTwo; }
            set { _IsMoreThanTwo = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 客户端货柜编码
        /// </summary>
        private string ContainerCode = string.Empty;


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
        /// 手动入库的物料
        /// </summary>
        private string selectMaterialName = string.Empty;
        public string SelectMaterialName
        {
            get { return selectMaterialName; }
            set { selectMaterialName = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 当前操作托盘号
        /// </summary>
        private string trayCode = string.Empty;
        public string SelectTrayCode
        {
            get { return trayCode; }
            set { trayCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前操作托盘号
        /// </summary>
        private int trayId = 0;
        public int TrayId
        {
            get { return trayId; }
            set { trayId = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前操作储位
        /// </summary>
        private string locationCode = string.Empty;
        public string SelectLocationCode
        {
            get { return locationCode; }
            set { locationCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前选择供应商
        /// </summary>
        private string selectSupplerCode = string.Empty;
        public string SelectSupplerCode
        {
            get { return selectSupplerCode; }
            set { selectSupplerCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 搜索条码
        /// </summary>
        private string selectSuppler = string.Empty;
        public string SelectSupplerName
        {
            get { return selectSuppler; }
            set { selectSuppler = value; RaisePropertyChanged(); }
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
        /// 生产日期
        /// </summary>
        private DateTime? manufactureDate = DateTime.Now;
        public DateTime? ManufactureDate
        {
            get { return manufactureDate; }
            set { manufactureDate = value; RaisePropertyChanged(); }
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
        /// 关联订单
        /// </summary>
        private string billCode = string.Empty;
        public string BillCode
        {
            get { return billCode; }
            set { billCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 搜索条码
        /// </summary>
        private string batchCode = string.Empty;
        public string BatchCode
        {
            get { return batchCode; }
            set { batchCode = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 关联订单
        /// </summary>
        private string inType = string.Empty;
        public string InType
        {
            get { return inType; }
            set { inType = value; RaisePropertyChanged(); }
        }

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
        /// 当前称重
        /// </summary>
        private decimal inWeight = 0;
        public decimal InWeight
        {
            get { return inWeight; }
            set { inWeight = value; RaisePropertyChanged(); }
        }

        private string boxName = string.Empty;
        public string BoxName
        {
            get { return boxName; }
            set { boxName = value; RaisePropertyChanged(); }
        }

        private string boxUrl = string.Empty;
        public string BoxUrl
        {
            get { return boxUrl; }
            set { boxUrl = value; RaisePropertyChanged(); }
        }

        private decimal _AviQuantity = 0;
        public decimal AviQuantity
        {
            get { return _AviQuantity; }
            set { _AviQuantity = value; RaisePropertyChanged(); }
        }



        private decimal _CurQuantity = 0;
        public decimal CurQuantity
        {
            get { return _CurQuantity; }
            set { _CurQuantity = value; RaisePropertyChanged(); }
        }

        private decimal _AviCount = 0;
        public decimal AviCount
        {
            get { return _AviCount; }
            set { _AviCount = value; RaisePropertyChanged(); }
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

        private int orgXLight = 0;

        private string materialUrl = string.Empty;
        public string MaterialUrl
        {
            get { return materialUrl; }
            set { materialUrl = value; RaisePropertyChanged(); }
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

        /// <summary>
        /// 当前操作的任务明细
        /// </summary>
        private List<LocationVM> aviLocationList = new List<LocationVM>();
        public List<LocationVM> AviLocationList
        {
            get { return aviLocationList; }
            set { aviLocationList = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 扫描的条码实体
        /// </summary>
        public LabelClient LabelEntity { get; set; } = new LabelClient();

        #endregion


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
        /// 核验登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckLogin()
        {
            // 验证是否有操作权限
            GlobalData.IsFocus = false;
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
                    // 光标默认
                    GlobalData.IsFocus = true;
                    return true;
                }
            }
        }


        /// <summary>
        /// 核验登录人员
        /// </summary>
        /// <param name="code"></param>
        public async void ShowLogin()
        {
            try
            {
                // 入库任务
                GlobalData.LoginModule = "ReceiptTask";
                GlobalData.LoginPageCode = "ManualInDlg";
                GlobalData.LoginPageName = "手动入库";
                //如果登录
                if (await CheckLogin())
                {
                    this.GetAllPageData();
                    GlobalData.IsFocus = true;
                }
                else //如果未登录
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
        /// 扫描入库条码-物料条码-物料编码-储位码
        /// </summary>
        public async void ScanBarcode(string code)
        {
            try
            {
                // 清空料盒提示
                IsMoreThanTwo = "Hidden";
                if (String.IsNullOrEmpty(SearchBarcode))
                {
                    return;
                }
                var materialCode = MaterialContract.MaterialDtos.FirstOrDefault(a => a.Code == SearchBarcode);
                if (materialCode == null)
                {
                    var labelEnity = LabelContract.LabelDtos.Where(a => a.Code == SearchBarcode).FirstOrDefault();
                    if (labelEnity == null)
                    {
                        // 如果是储位码
                        var locationCode = WareHouseContract.LocationVMs.FirstOrDefault(a => a.Code == SearchBarcode);
                        var dialog = ServiceProvider.Instance.Get<IShowContent>();
                        if (locationCode != null)
                        {
                            var material = new MaterialDto();
                            if (String.IsNullOrEmpty(locationCode.LockMaterialCode))
                            {
                                material = MaterialContract.MaterialDtos.FirstOrDefault(a => a.Code == locationCode.SuggestMaterialCode);
                                if (material==null)
                                {
                                    Msg.Warning("该储位未维护存放物料，请扫描物料信息！");
                                    return;
                                }
                            }
                            else
                            {
                                 material = MaterialContract.MaterialDtos.FirstOrDefault(a => a.Code == locationCode.LockMaterialCode);
                                 if (material == null)
                                 {
                                     Msg.Warning("该储位未维护存放物料，请扫描物料信息！");
                                     return;
                                 }
                            }

                            LabelEntity.MaterialCode = locationCode.SuggestMaterialCode;
                            LabelEntity.MaterialName = material.Name;
                            SelectMaterialName = material.Name;
                            LabelEntity.MaterialUrl = _basePath + material.PictureUrl;
                            if (String.IsNullOrEmpty(locationCode.LockMaterialCode))
                            {
                                SelectMaterialCode = locationCode.SuggestMaterialCode;
                            }
                            else
                            {
                                SelectMaterialCode = locationCode.LockMaterialCode; // 该储物当前存放的物料
                            }
                            SelectTrayCode = locationCode.TrayCode;
                  
                            XLight = locationCode.XLight;
                            YLight = locationCode.YLight;
                        }
                        else
                        {
                            Clear();
                            Msg.Warning("未获取到条码信息！");
                            return;
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
                        BatchCode = labelEnity.BatchCode;
                        SelectMaterialCode = labelEnity.MaterialCode;
                        SelectMaterialName = labelEnity.MaterialName;
                    }
                    SelectMaterial();
                }
                else
                {
                    LabelEntity.MaterialUrl = _basePath + materialCode.PictureUrl;
                    LabelEntity.MaterialName = materialCode.Name;
                    SelectMaterialName = materialCode.Name;
                    SelectMaterialCode = materialCode.Code;
                    SelectMaterial();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        public async void ScanBarcodeKeyDown(Object obj)
        {
           // var code = obj as SearchableTextBox.SearchableTextBox;
            ScanBarcode("");
        }

        /// <summary>
        /// 删除入库项目
        /// </summary>
        /// <param name="entity"></param>
        public async void DelectInTaskItem(Bussiness.Dtos.InTaskMaterialDto entity)
        {
            try
            {
                if (entity!=null)
                {
                    if (await Msg.Question("是否删除本入库明细，请确保已将物料从货柜中取出!"))
                    {
                        InTaskMaterial.Remove(entity);
                    }
                }

            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// 获取本货柜下全部物料
        /// </summary>
        public  async void GetAllPageData()
        {
            try
            {
                //// 查询货柜下可存放的物料--不包括模具
                //var materialGroup = WareHouseContract.LocationVIEWs.Where(it => it.ContainerCode == ContainerCode&&it.MaterialType!=(int)MaterialTypeEnum.Mould&&!string.IsNullOrEmpty(it.MaterialCode))
                //    .GroupBy(it => it.MaterialCode)
                //    .AndBy(it => it.MaterialName)
                //    .Select(it => new { MaterialCode = it.MaterialCode , MaterialName = it.MaterialName }).Take(20).ToList();


                //_ModuleGroups.Clear();

                //foreach (var item in materialGroup)
                //{
                //    var materialItem = new Material()
                //    {
                //        Code = item.MaterialCode,
                //        Name = item.MaterialName
                //    };
                //    _ModuleGroups.Add(materialItem);
                //}
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        /// <summary>
        /// 选择物料
        /// </summary>
        public async void SelectMaterial()
        {
            try 
            {
                if (String.IsNullOrEmpty(SelectMaterialCode))
                {
                    return;
                }
                var materialEntity = MaterialContract.Materials.FirstOrDefault(a => a.Code == SelectMaterialCode);
                MaterialUrl = _basePath+ materialEntity.PictureUrl;
                if (String.IsNullOrEmpty(BatchCode)&& materialEntity.IsBatch)
                {
                    Msg.Warning("该物料已经批次管理，请输入物料批次");
                    SelectMaterialCode = "";
                    SearchBarcode = "";
                    return;
                }
                //if (InQuantity<=0)
                //{
                //    Msg.Warning("入库数量必须大于0或者未输入入库数量");
                //    SelectMaterialCode = "";
                //    SearchBarcode = "";
                //    return;
                //}
                InTaskMaterialEntity = new InTaskMaterialDto()
                {
                    BillCode=BillCode,
                    BatchCode= BatchCode,
                    MaterialCode= SelectMaterialCode,
                    MaterialLabel= SearchBarcode,
                    InTaskMaterialQuantity = InQuantity,
                    SuggestContainerCode=ContainerCode
                };
                var intaskService = ServiceProvider.Instance.Get<IInTaskService>();
                var inTask = intaskService.PostClientLocationList(InTaskMaterialEntity);

                if (inTask.Result.Success)
                {
                    AviLocationList = JsonHelper.DeserializeObject<List<LocationVM>> (inTask.Result.Data.ToString());

                    if (AviLocationList.Count==0)
                    {
                        Msg.Warning("您暂无该物料所在托盘的操作权限或无可存放储位");
                        Clear();
                        return;
                    }

                    var trayList = AviLocationList.GroupBy(a => a.TrayCode).ToArray();
                    _TaryGroups.Clear();

                    foreach (var item in trayList)
                    {
                        var LocationVIEW = new LocationVIEW()
                        {
                            TrayCode = item.Key
                        };
                        _TaryGroups.Add(LocationVIEW);
                    }

                    SelectTrayCode = trayList[0].Key;
                }
                else
                {
                    // 未授权则登录
                    if (inTask.Result.Message == "Unauthorized")
                    {
                        var dialog = ServiceProvider.Instance.Get<IShowContent>();
                        dialog.BindDataContext(new UserLoginWindow(), new UserLoginModel());
                        // 系统登录
                        dialog.Show();
                    }
                    else
                    {
                        Msg.Error("获取可存放托盘失败：" + inTask.Result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }



        /// <summary>
        /// 选择托盘
        /// </summary>
        public async void SelectTray()
        {
            try
            {
                // 查询该物料可存放的储位
                var locationGroup = AviLocationList.Where(a => a.TrayCode == SelectTrayCode).OrderBy(a=>a.AviQuantity).ToList();
                _LocationGroups.Clear();

                foreach (var item in locationGroup)
                {
                    TrayId = (int)item.TrayId;
                    var LocationVIEW = new LocationVIEW()
                    {
                        Code = item.Code
                    };
                    _LocationGroups.Add(LocationVIEW);
                }

                if (locationGroup.Count>0)
                {
                    SelectLocationCode = _LocationGroups[0].Code;
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        /// <summary>
        /// 选择储位
        /// </summary>
        public async void SelectLocation()
        {
            try
            {
                // 查询该储位最多可存放的物料数量
                LocationVM location = AviLocationList.FirstOrDefault(a => a.Code == SelectLocationCode);
                if (location==null)
                {
                    return;
                }

                var stock = StockContract.Stocks
                    .Where(a => a.LocationCode == location.Code && a.MaterialCode == SelectMaterialCode)
                    .FirstOrDefault();
                if (stock != null)
                {
                    CurQuantity = (decimal)stock.Quantity;
                }
                else
                {
                    CurQuantity = 0;
                }
                BoxUrl = _basePath + location.BoxUrl;
                BoxName = location.BoxName;
                AviQuantity = location.AviQuantity.GetValueOrDefault(0);
                XLight = location.XLight;
                YLight = location.YLight;
                AviCount = AviLocationList.Count;
                if (AviCount>1)
                {
                    IsMoreThanTwo = "Visible";
                }

                if (SelectTrayCode == CurTratCode)
                {
                    RunningContainer();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        /// <summary>
        /// ComboBox 下拉数据初始化
        /// </summary>
        public async void SelectInit()
        {
            try
            {
                // 查询系统中的供应商
                var list = SupplyContract.Supplys.Where(a => a.Code.Contains(SelectSupplerCode) || a.Name.Contains(SelectSupplerCode)).ToList();
                    list= list.Where(a=>!(bool)a.IsDeleted).Take(20).ToList();
                
                _SupplyGroups.Clear();
                foreach (var item in list)
                {
                    _SupplyGroups.Add(item);
                }


                // 查询系统中的入库业务类别
                var inTypelist = DictionaryContract.Dictionaries.Where(a => a.TypeCode == "InType" && a.Enabled).ToList();

                _InTypeGroups.Clear();
                foreach (var item in inTypelist)
                {
                    _InTypeGroups.Add(item);
                }

            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        private bool IsReadingGrossWeight = false;//是否正在进行称毛重读取

        /// <summary>
        /// 打印条码
        /// </summary>
        public async void PrintItem(Bussiness.Dtos.InTaskMaterialDto entity)
        {
            try
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
                    MaterialCode = entity.MaterialCode,
                    MaterialName = entity.MaterialName,
                    BatchCode = entity.BatchCode,
                    SupplyCode = entity.SupplierCode,
                    Quantity = tempQuantity
                };
                var dialog = ServiceProvider.Instance.Get<IShowContent>();
                dialog.BindDataContext(new BarCodeView(), new BarCodeViewModel() { LabelEntity = label });
                //    dialog.D
                // 条码打印
                dialog.Show();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 核验重量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightCheck()
        {
            try
            {
                // weightReader.Open(COM, 9600);
                if (weightReader.Open(COM, 9600))
                {
                    var sendData = weightReader.strToHexByte("01 03 00 00 00 02 c4 0b");
                    weightReader.SendData(sendData);
                    IsReadingGrossWeight = true;
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }


        /// <summary>
        /// 获取重量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void weightReader_Changed(object sender, EventArgs e)
        {
            InWeight = Convert.ToDecimal(weightReader.WeightInformationObj.WData.ToString());

            if (InWeight > 0)
            {
                // 解除托盘的重量临时锁定
                //  托盘实体
                var trayEntity = WareHouseContract.TrayWeightMapRepository.GetEntity(a => a.TrayId == TrayId);

                //  如果维护了托盘最大称重
                if (trayEntity.MaxWeight > 0)
                {
                    var aviWeight = trayEntity.MaxWeight - trayEntity.LockWeight + trayEntity.TempLockWeight;

                    if (InWeight <= aviWeight)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() => {
                            Msg.Warning("核验成功，重量可存放！");
                        }));
                           
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke(new Action(() => {
                            Msg.Warning("核验失败，已超过最大存放重量！");
                        }));
                      
                    }
                }
            }
        }


        /// <summary>
        /// 启动货柜
        /// </summary>
        public async void RunningContainer()
        {
            try
            {
                //if (GlobalData.DeviceStatus == (int)DeviceStatusEnum.Fault)
                //{
                //    GlobalData.IsFocus = true;
                //    Msg.Warning("设备离线状态，无法启动货柜！");
                //    return;
                //}

                GlobalData.IsFocus = false;
                if (String.IsNullOrEmpty(SelectTrayCode))
                {
                    GlobalData.IsFocus = true;
                    Msg.Warning("未选择执行托盘，请先选择一项！");

                    return;
                }

                if (SelectTrayCode != CurTratCode)
                {
                    GlobalData.IsFocus = true;

                    if (!await Msg.Question("是否驱动货柜至新的托盘处？"))
                    {
                        return;
                    }
                }
                else
                {
                    if (XLight!=0)
                    {
                        Msg.Warning("储位在同层！");
                    }
                }

                // 判断是否启用单包条码管理
                if (InQuantity > AviQuantity)
                {
                    GlobalData.IsFocus = true;
                    Msg.Warning("超出该容器可存放的最多数量！");
                    return;
                }
                // 核查用户是否有此模块操作权限
                var user = ServiceProvider.Instance.Get<IUserService>();
                var authCheck = user.GetCheckTrayAuth((int)TrayId);
                if (!authCheck.Result.Success)
                {
                    Msg.Warning("抱歉，您无操作该托盘权限！");
                    return;
                }
            

                // await dialog.c
                // 读取PLC 状态信息
                var baseControlService = ServiceProvider.Instance.Get<IBaseControlService>();

                var runingEntity = new RunningContainer()
                {
                    ContainerCode = ContainerCode,
                    TrayCode = Convert.ToInt32(SelectTrayCode),
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
                if (runningContainer.Result.Success)//
                {
                    GlobalData.IsFocus = true;
                    CurTratCode = SelectTrayCode;
                    orgXLight = XLight;
                }
                else
                {
                    GlobalData.IsFocus = true;
                    Msg.Error(runningContainer.Result.Message);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void OffXLight()
        {
            if (GlobalData.DeviceStatus == (int)DeviceStatusEnum.Fault)
            {
                GlobalData.IsFocus = true;
                Msg.Warning("设备离线状态，无法启动货柜！");
                return;
            }

            GlobalData.IsFocus = false;
            // await dialog.c
            // 读取PLC 状态信息
            var baseControlService = ServiceProvider.Instance.Get<IBaseControlService>();

            var runingEntity = new RunningContainer()
            {
                ContainerCode = ContainerCode,
                TrayCode = Convert.ToInt32(SelectTrayCode),
                XLight = XLight
            };
            var container = ContainerRepository.Query().FirstOrDefault(a => a.Code == ContainerCode);
            if (container != null)
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
                CurTratCode = SelectTrayCode;
                orgXLight = XLight;
            }
            else
            {
                GlobalData.IsFocus = true;
                Msg.Error(runningContainer.Result.Message);
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
                        runingEntity.Port = int.Parse( container.Port);
                    }

                    // 货柜运行
                    //var runningContainer = baseControlService.WriteD650_In(runingEntity).Result;
                    var runningContainer = baseControlService.PostStartRestoreContainer(runingEntity).Result;
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
        /// 确认存入
        /// </summary>
        public void HandShelf()
        {
            try
            {
                //if (InTaskMaterial.Any(a => a.LocationCode == SelectLocationCode && a.MaterialCode != SelectMaterialCode))
                //{
                //    Msg.Warning("");
                //    return;
                //}

                if (InQuantity == 0)
                {
                    Msg.Warning("请输入入库数量");
                    return;
                }
                // 判断是否启用单包条码管理
                if (InQuantity > AviQuantity)
                {
                    GlobalData.IsFocus = true;
                    Msg.Warning("超出该容器可存放的最多数量！");
                    return;
                }
                if (CurTratCode != SelectTrayCode)
                {
                    Msg.Warning("当前货柜未运行至此托盘，请启动货柜！");
                    return;
                }

                if (orgXLight != XLight)
                {
                    Msg.Warning("当前货柜未运行至此储位，请启动货柜！");
                    return;
                }

                if (InQuantity == 0)
                {
                    Msg.Warning("请输入入库数量");
                    return;
                }
                var materialEntity = MaterialContract.Materials.FirstOrDefault(a => a.Code == SelectMaterialCode);

                // 允许混批
                if (materialEntity.IsMaxBatch)
                {
                    if (InTaskMaterial.Any(a => a.LocationCode == SelectLocationCode))
                    {
                        Msg.Warning("该储位在手动入库明细中，请重新选择存放储位或删除该明细重新入库！");
                        return;
                    }
                }
                else // 不允许混批
                {
                    if (InTaskMaterial.Any(a => a.LocationCode == SelectLocationCode && a.MaterialCode != SelectMaterialCode&&a.BatchCode!= BatchCode))
                    {
                        Msg.Warning("该储位已存放物料，请重新选择存放储位！");
                        return;
                    }
                }
      
                GlobalData.IsFocus = false;
 


                // 判断是否启用条码
                if (materialEntity.IsPackage)
                {
                    // 判断条码是否为空
                    if (String.IsNullOrEmpty(SearchBarcode))
                    {
                        InQuantity = 0;
                        Msg.Warning("该物料已经启用条码管理，请扫描入库条码");
                        return;
                    }
                }

                decimal? inWeight = InQuantity * materialEntity.UnitWeight; // inQuantity 本次入库的重量
                /* 计算托盘是否可承重*/
                //  托盘实体
                var trayEntity =
                    WareHouseContract.TrayWeightMapRepository.GetEntity(a =>
                        a.TrayId == TrayId);

                var availabelTray = trayEntity.MaxWeight - trayEntity.LockWeight - trayEntity.TempLockWeight;

                if (availabelTray< inWeight)
                {
                    InQuantity = 0;
                    Msg.Warning("核验失败，已超过最大存放重量！");
                    return;
                }
                this.IsCancel = false;
                InTaskMaterialEntity.ShelfTime = DateTime.Now;
                InTaskMaterialEntity.SuggestLocation = SelectLocationCode;
                InTaskMaterialEntity.LocationCode = SelectLocationCode;
                InTaskMaterialEntity.SuggestTrayCode = SelectTrayCode;
                InTaskMaterialEntity.MaterialName = materialEntity.Name;
                InTaskMaterialEntity.Quantity = InQuantity;
                InTaskMaterialEntity.RealInQuantity= InQuantity;
                InTaskMaterialEntity.InTaskMaterialQuantity = InQuantity;
                InTaskMaterialEntity.BatchCode = BatchCode;
                InTaskMaterialEntity.SuggestContainerCode = ContainerCode;
                InTaskMaterialEntity.ManufactrueDate = ManufactureDate;
                InTaskMaterialEntity.SupplierCode = SelectSupplerCode;
                InTaskMaterialEntity.InDict = InType;
                // 新增入库任务明细
                InTaskMaterial.Add(InTaskMaterialEntity);
                // 灭灯
                XLight = 0;
                YLight = 0;
                //RunningContainer();
               // OffXLight();
                Clear();
            }
            catch (Exception ex)
            {
                Msg.Error("添加手动入库物料失败！");
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
                if (InTaskMaterial.Count==0)
                {
                    Msg.Warning("未执行入库操作,无法提交");
                    return;
                }

                var inMaterial = JsonHelper.SerializeObject(InTaskMaterial);

                var intaskService = ServiceProvider.Instance.Get<IInTaskService>();

                var inTask = new InTaskDto()
                {
                    InTaskMaterialList= inMaterial
                };
                var inTaskPost = intaskService.PostManualInList(inTask);


                // 仍有未完成的任务
                if (inTaskPost.Result.Success)
                {
                    Clear();
                    InTaskMaterial.Clear();
                    Msg.Info("手动入库执行成功！");
                    GlobalData.IsFocus = true;
                    GlobalData.LoginModule = "ReceiptTask";
                    GlobalData.LoginPageCode = "ManualInDlg";
                    GlobalData.LoginPageName = "手动入库";
                    // 刷新界面
                    var obj = new MainViewModel();
                    if (obj == null) return;
                    obj.UpdatePage(GlobalData.LoginPageName, GlobalData.LoginPageCode);
                    return;
                }
                else // 直接跳转至任务界面
                {
                    Msg.Warning("手动入库执行失败:"+ inTaskPost.Result.Message);
                }
            }
            catch (Exception ex)
            {
                Msg.Question("物料入库失败：");
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
                MaterialUrl = "";
                InQuantity = 0;
                BatchCode = "";
                SelectMaterialCode = "";
                SelectMaterialName = "";
                SelectTrayCode = "";
                BoxUrl = "";
                BoxName = "";
                AviQuantity = 0;
                SelectSupplerCode = "";
                CurQuantity = 0;
                XLight = 0;
                YLight = 0;
                AviCount = 0;
                SearchBarcode = "";
                LabelEntity.MaterialUrl = "";
                GlobalData.IsFocus = true;
                IsMoreThanTwo = "Hidden";
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

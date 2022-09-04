using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using wms.Client.LogicCore.Configuration;
using wms.Client.LogicCore.Enums;
using wms.Client.LogicCore.Helpers.Files;
using wms.Client.Service;
using wms.Client.ViewModel;

namespace wms.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
         DispatcherTimer dTimer;
        public MainWindow()
        {
            InitializeComponent(); 
            this.Zone.MouseDoubleClick += (sender, e) => { Max(); };
            //构造一个DispatcherTimer类实例
            dTimer = new System.Windows.Threading.DispatcherTimer();
            //设置事件处理函数
            dTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            EventManager.RegisterClassHandler(typeof(Button), Button.ClickEvent, new RoutedEventHandler(UpdataText));
        }
        
        #region Messenger

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="msg"></param>
        public void Max(bool Mask = false)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        /// <summary>
        /// 如果在时间没无人操作任何按钮，退出全部页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // 清除登录信息
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("Login", "UserCode", "");
            ini.IniWriteValue("Login", "UserName", "");
            ini.IniWriteValue("Login", "PictureUrl", "");
            ini.IniWriteValue("Login", "LoginTime", "");
            ini.IniWriteValue("Login", "Name", "");
            GlobalData.loginTime = "";
            var obj = new MainViewModel();
            if (obj == null) return;
            obj.ExitPage(MenuBehaviorType.ExitAllPage, "");
        }



        private void UpdataText(object sender, RoutedEventArgs e)
        {
            Button textBox = sender as Button;
            KeyEventArgs keyEventArgs = e as KeyEventArgs;
            int i = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["NoOp"].ToString())*60;
            //定时器时间间隔1s
            if (dTimer.Interval != null)
            {
                dTimer.Interval = new TimeSpan(0, 0, i);
                dTimer.Start();
            }
        }

        //触摸后重新给“i”赋值
        //private void SurfaceWindow_TouchDown(object sender, TouchEventArgs e)
        //{
        //    int i = 60;
        //    //定时器时间间隔1s
        //    if (dTimer.Interval != null)
        //    {
        //        dTimer.Interval = new TimeSpan(0, 0, i);
        //        dTimer.Start();
        //    }
        //}
        //private void SurfaceWindow_TouchDown(object sender, MouseEventArgs e)
        //{
        //    int i = 30;
        //    //定时器时间间隔1s
        //    if (dTimer.Interval != null)
        //    {
        //        dTimer.Interval = new TimeSpan(0, 0, i);
        //        dTimer.Start();
        //    }
        //}
        #endregion

    }
}

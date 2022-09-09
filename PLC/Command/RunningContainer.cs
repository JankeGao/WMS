﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCServer.Command
{
    public class RunningContainer
    {
        /// <summary>
        /// 货架编码
        /// </summary>
        public string ContainerCode { get; set; }
        /// <summary>
        /// 托盘序号
        /// </summary>
        public int TrayCode { get; set; }
        /// <summary>
        /// X轴灯号
        /// </summary>
        public int XLight{ get; set; }

        public string IpAddress { get; set; }

        public int Port { get; set; }
        /// <summary>
        /// 类型  1  2 3 4
        /// </summary>
        public int ContainerType { get; set; }

        /// <summary>
        /// 上一次运转货柜
        /// </summary>
        public string LastTrayCode { get; set; }
    }
}

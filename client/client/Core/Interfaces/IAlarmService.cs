﻿using System.Threading.Tasks;
using Bussiness.Dtos;
using Bussiness.Entitys;
using HP.Utility.Data;

namespace wms.Client.Core.Interfaces
{
    public interface IAlarmService
    {
        /// <summary>
        /// 复位服务器报警信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DataResult> PostRestAllAlarmServer(DeviceAlarm model);
    }
}
